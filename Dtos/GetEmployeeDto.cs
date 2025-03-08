namespace TodoApi.Dtos;

public record class GetEmployeeDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; } // = Department.Id (referencing)
    public string? Department { get; set; }
    public EmpStatus Status { get; set; }
}
