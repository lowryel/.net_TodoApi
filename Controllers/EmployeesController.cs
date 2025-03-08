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
using TodoApi.Dtos;
using ServiceStack;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace EmployeeControllerService.Controller;

[ApiController]
[Route("employees")]
public class EmployeeController : ControllerBase
{
    // Dependency injection for the database context
    private readonly IEmployee _employee;


    public EmployeeController(IEmployee employee)
    {
        _employee = employee;
    }


    // GET endpoint to retrieve all employees
    [HttpGet]
    // [Authorize]
    // [Route("employee/")]
    [Produces("application/json"), ValidatemailAddress("ellowry09@gmail.com")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _employee.GetEmployees());
        // return Ok(await dbContext.Employees.ToListAsync());
    }


    // GET endpoint to retrieve a specific employee by ID
    [HttpGet("{employeeId}")]
    public async Task<GetEmployeeDto?> Get(Guid employeeId)
    {
        return await _employee.GetEmployee(employeeId);
    }


    // POST endpoint to insert a new employee
    [HttpPost]
    // [Route("add/")]
    public async Task<IActionResult> CreateUser([FromBody] AddEmployeeDto employeeDto)
    {
        if (employeeDto.Email == "")
        {
            return NotFound("Email is a required field");
        }
        var employeeEntity = await _employee.AddEmployee(employeeDto);
        return Ok(employeeEntity);
    }


    // UPDATE Employee info
    [HttpPut("{employeeId}")]
    public async Task<IActionResult> UpdateEmployee(Guid employeeId, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _employee.UpdateEmployee(updateEmployeeDto,employeeId);
        return Ok(employee);
    }


    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteAsync(Guid employeeId)
    {
        var employeeEntity = await _employee.DeleteEmployee(employeeId);
        if (!employeeEntity)
        {
            return NotFound();
        };

        return NoContent();
    }

    [HttpGet("query")]
    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> Query([FromQuery] EmployeeFilter query)
    {
        var employees = await _employee.EmployeeFilter(query);

        if (!employees.Any())
        {
            return NotFound("No employees match the specified criteria");
        }

        return Ok(employees);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginModel model)
    {

        // retrieve userId using the email
        var user = await _employee.LoginAsync(model);

        if (user == null)
        {
            return NotFound("User not found");
        }
        Console.WriteLine(user);
        return Unauthorized();
    }



}
