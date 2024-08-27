using System.ComponentModel.DataAnnotations;
using EmployeeAdminPortal.Models;

public class Employee
{
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required, EmailAddress]
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public decimal Salary { get; set; }
    public EmpStatus Status { get; set; } 
}

public enum EmpStatus
{
    Active,
    Inactive,
}

