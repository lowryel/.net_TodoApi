namespace EmployeeAdminPortal.Models;

public record class AddEmployeeDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public decimal Salary { get; set; }
}