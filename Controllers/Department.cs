using System;
using EmployeeAdminPortal.Data;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly ApplicationDBContext _dbContext;
    public DepartmentController(ApplicationDBContext context)
    {
        _dbContext = context;
    }

    [HttpPost]
    [Route("add/")]
    public async Task<IActionResult> Post(Department department)
    {
        await _dbContext.Departments.AddAsync(department);
        await _dbContext.SaveChangesAsync();
        return Ok(department);
    }
}
