using System;
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TodoApi;
using Microsoft.Extensions.Caching.Distributed;
using ServiceStack.Redis;
using StackExchange.Redis;

namespace EmployeeControllerService.Controller;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    // Dependency injection for the database context
    private readonly ApplicationDBContext dbContext;

    private readonly IMyKeyedServices _myServices; // demo cache service


    // static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
    private readonly IDatabase _redis;



    private readonly JwtService _jwtService;
    public EmployeeController(ApplicationDBContext db, IMyKeyedServices myServices, JwtService jwtService, IConnectionMultiplexer multiplexer)
    {
        dbContext = db;
        _myServices = myServices;
        _jwtService = jwtService;
        _redis = multiplexer.GetDatabase();
    }


    // GET endpoint to retrieve all employees
    [HttpGet]
    [Authorize]
    // [Route("employee/")]
    [Produces("application/json"), ValidatemailAddress("ellowry09@gmail.com")]
    public async Task<IActionResult> Get()
    {
        _myServices.CachePage("Get all Employees Service");
        var employees = await dbContext.Employees
                        .Include(e => e.Department)
                        .ToListAsync();
        return employees == null
                ? NotFound()
                : Ok(employees);
        // return Ok(await dbContext.Employees.ToListAsync());
    }


    // GET endpoint to retrieve a specific employee by ID
    [HttpGet("{employeeId}")]
    public async Task<Employee?> Get(Guid employeeId)
    {
        return await dbContext.Employees.FindAsync(employeeId);
    }


    // POST endpoint to insert a new employee
    [HttpPost]
    [Route("add/")]
    public async Task<IActionResult> CreateUser([FromBody] AddEmployeeDto employeeDto)
    {
        if (employeeDto.Email == "")
        {
            return NotFound("Email is a required field");
        }
        var employeeEntity = new Employee()
        {
            Name = employeeDto.Name,
            Email = employeeDto.Email,
            Salary = employeeDto.Salary,
            Phone = employeeDto.Phone,
            Position = employeeDto.Position,
            // Status = (EmpStatus)employeeDto.Status,
            DepartmentId = employeeDto.DepartmentId
        };
        await dbContext.Employees.AddAsync(employeeEntity);
        await dbContext.SaveChangesAsync();
        return Ok(employeeEntity);
    }


    // UPDATE Employee info
    [HttpPut("{employeeId}")]
    public async Task<IActionResult> UpdateEmployee(Guid employeeId, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await dbContext.Employees.FindAsync(employeeId); // get employee with id
        if (employee is null) // conduct a null check
        {
            return NotFound();
        }
        employee.Name = updateEmployeeDto.Name;
        employee.Email = updateEmployeeDto.Email;
        employee.Salary = updateEmployeeDto.Salary;
        employee.Phone = updateEmployeeDto.Phone;
        employee.Position = updateEmployeeDto.Position;
        employee.DepartmentId = updateEmployeeDto.DepartmentId;
        // employee.Status = (EmpStatus)updateEmployeeDto.Status;

        await dbContext.SaveChangesAsync();
        return Ok(employee);
    }


    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteAsync(Guid employeeId)
    {
        var employeeEntity = dbContext.Employees.Find(employeeId);
        if (employeeEntity is null)
        {
            return NotFound();
        };
        dbContext.Employees.Remove(employeeEntity);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginModel model)
    {

        // retrieve userId using the email
        var user = await dbContext.Employees
            .FirstOrDefaultAsync(emp => emp.Email == model.Email);

        if (user == null)
        {
            return NotFound("User not found");
        }
        Console.WriteLine(user.Name);

        Guid userId = user.Id;
        // Validate user credentials (replace with your actual authentication logic)
        if (IsValidUser(model.Email))
        {
            var token = _jwtService.GenerateToken(userId.ToString(), model.Email);
            _redis.StringGetSet("loginToken", token);
            return Ok(new { token });
        }
        return Unauthorized();
    }

    private bool IsValidUser(string email)
    {
        // Implement your user validation logic here
        return true; // For demonstration purposes
    }

}

public class LoginModel
{
    public required string Email { get; set; }
}