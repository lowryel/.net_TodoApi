using System;
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeControllerService.Controller;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    // Dependency injection for the database context
    private readonly ApplicationDBContext dbContext;

    private readonly IMyKeyedServices _myServices; // demo cache service
    
    public EmployeeController(ApplicationDBContext db, IMyKeyedServices myServices)
    {
        dbContext = db;
        _myServices = myServices;
    }


    // GET endpoint to retrieve all employees
    [HttpGet]
    // [Authorize("read:messages")]
    // [Route("employee/")]
    [Produces("application/json"), ValidatemailAddress("ellowry09@gmail.com")]
    public async Task<IActionResult> Get()
    {
        _myServices.CachePage("Get all Employees Service");
        return Ok(await dbContext.Employees.ToListAsync());
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
    public async Task Insert([FromBody] AddEmployeeDto employeeDto)
    {
        var employeeEntity = new Employee()
        {
            Name = employeeDto.Name,
            Email = employeeDto.Email,
            Salary = employeeDto.Salary,
            Phone = employeeDto.Phone,
        };
        await dbContext.Employees.AddAsync(employeeEntity);
        await dbContext.SaveChangesAsync();
        Ok(employeeEntity);
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
}

