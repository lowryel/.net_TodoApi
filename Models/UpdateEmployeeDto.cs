namespace EmployeeAdminPortal.Models;

public record class UpdateEmployeeDto
{
    public string? Position { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public decimal Salary { get; set; }
    
    public int DepartmentId { get; set; } // = Department.Id (referencing)
}

