using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
    public int DepartmentId { get; set; } // = Department.Id (referencing)
    public Department? Department { get; set; }
    public EmpStatus Status { get; set; } = EmpStatus.Active;
}

public enum EmpStatus
{
    Inactive,
    Active,
}

public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Name { get; set; }
    [JsonIgnore] // prevent circular reference in the related models
    public ICollection<Employee> Employees { get; } = new List<Employee>();
}

