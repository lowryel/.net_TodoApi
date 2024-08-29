using System.ComponentModel.DataAnnotations;
using EmployeeAdminPortal.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class Employee : IConvention
{
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required, EmailAddress]
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public decimal Salary { get; set; }
    
    public EmpStatus Status { get; set; } = EmpStatus.Active;
}

public enum EmpStatus
{
    Active,
    Inactive,
}

public class Accountant
{
    public Guid Id { get; set; }
    public required Employee AC { get; set; }
}