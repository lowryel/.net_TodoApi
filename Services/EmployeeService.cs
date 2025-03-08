using System;
using EmployeeAdminPortal.Data;
using StackExchange.Redis;
using TodoApi.Dtos;

using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TodoApi;
using Microsoft.Extensions.Caching.Distributed;
using ServiceStack.Redis;
using Microsoft.EntityFrameworkCore;
using EmployeeAdminPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Services;

public class EmployeeService : IEmployee
{
    private readonly ApplicationDBContext _dbContext;
    private readonly IMyKeyedServices _myServices; // demo cache service
    private readonly ILogger<JwtService> _logger;


    // static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
    private readonly IDatabase _redis;



    private readonly JwtService _jwtService;
    public EmployeeService(ApplicationDBContext context, JwtService jwtService, IMyKeyedServices myServices, IConnectionMultiplexer multiplexer, ILogger<JwtService> logger)
    {
        _dbContext = context;
        _myServices = myServices;
        _jwtService = jwtService;
        _redis = multiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<Employee> AddEmployee([FromBody] AddEmployeeDto employeeDto)
    {
        var employeeEntity = new Employee()
        {
            Name = employeeDto.Name,
            Email = employeeDto.Email,
            Salary = employeeDto.Salary,
            Phone = employeeDto.Phone,
            Position = employeeDto.Position,
            DepartmentId = employeeDto.DepartmentId
        };
        await _dbContext.Employees.AddAsync(employeeEntity);
        await _dbContext.SaveChangesAsync();
        return employeeEntity;
    }

    public async Task<bool> DeleteEmployee(Guid employeeId)
    {
        var employee = await _dbContext.Employees.FindAsync(employeeId);
        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {employeeId} not found.");
        }
        _dbContext.Employees.Remove(employee);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public Task<GetEmployeeDto> GetEmployee(Guid employeeId)
    {
        var employee = _dbContext.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee != null)
        {
            var employeeDto = employee.Result;
            var employeeDtoResult = new GetEmployeeDto
            {
                Id = employeeDto!.Id,
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Department = employeeDto.Department!.Name,
                DepartmentId = employeeDto.DepartmentId,
                Phone = employeeDto.Phone,
                Position = employeeDto.Position,
                Status = employeeDto.Status,
                Salary = employeeDto.Salary
            };
            return Task.FromResult(employeeDtoResult);
        }
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<GetEmployeeDto>> GetEmployees()
    {
        _myServices.CachePage("Get all Employees Service");
        var employees = await _dbContext.Employees
                        .Include(e => e.Department)
                        .ToListAsync();
        return employees.Select(e => new GetEmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            Department = e.Department!.Name,
            DepartmentId = e.DepartmentId,
            Phone = e.Phone,
            Position = e.Position,
            Status = e.Status,
            Salary = e.Salary
        });
    }

    public async Task<string> LoginAsync(LoginModel model)
    {
        // Find user by email
        var user = await _dbContext.Employees
            .FirstOrDefaultAsync(emp => emp.Email == model.Email);

        if (user == null)
        {
            _logger.LogWarning("Login attempt failed: User with email {Email} not found", model.Email);
            return "User not found";
        }

        _logger.LogInformation("User found: {Email}", user.Email);

        // Validate user credentials
        if (IsValidUser(model.Email))
        {
            var token = _jwtService.GenerateToken(user.Id.ToString(), model.Email);
            await _redis.StringSetAsync("loginToken", token);
            return token;
        }

        _logger.LogWarning("Login attempt failed: Invalid credentials for {Email}", model.Email);
        return "Invalid credentials";
    }

    public async Task<int> UpdateEmployee(UpdateEmployeeDto updateEmployeeDto, Guid employeeId)
    {
        // First check if department exists
        // var departmentExists = await _dbContext.Departments.AnyAsync(d => d.Id == updateEmployeeDto.DepartmentId);
        // if (!departmentExists)
        // {
        //     throw new InvalidOperationException($"Department with ID {updateEmployeeDto.DepartmentId} does not exist.");
        // }

        var employee = await _dbContext.Employees.FindAsync(employeeId);
        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {employeeId} not found.");
        }

        employee.Name = updateEmployeeDto.Name ?? employee.Name;
        employee.Email = updateEmployeeDto.Email ?? employee.Email;
        employee.Salary = updateEmployeeDto.Salary;
        employee.Phone = updateEmployeeDto.Phone;
        employee.Position = updateEmployeeDto.Position;
        // employee.DepartmentId = updateEmployeeDto.DepartmentId;

        return await _dbContext.SaveChangesAsync();
    }

    private bool IsValidUser(string email)
    {
        // Implement your user validation logic here
        return true; // For demonstration purposes
    }
}





public interface IEmployee
{
    Task<GetEmployeeDto> GetEmployee(Guid employeeId);
    Task<IEnumerable<GetEmployeeDto>> GetEmployees();
    Task<Employee> AddEmployee(AddEmployeeDto employee);
    Task<int> UpdateEmployee(UpdateEmployeeDto employeeDto, Guid employeeId);
    Task<bool> DeleteEmployee(Guid employeeId);
    Task<string> LoginAsync(LoginModel model);
}



public class LoginModel
{
    public required string Email { get; set; }
}