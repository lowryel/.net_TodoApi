using System.Text.Json.Serialization;

namespace EmployeeAdminPortal.Models;

public record class AddEmployeeDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public decimal Salary { get; set; }
    public string? Position { get; set; } 
    public int DepartmentId { get; set; } // = Department.Id (referencing)
    // 
    // public Department? Department { get; set; }

}


public enum EmpStatus
{
    Active,
    Inactive,
}