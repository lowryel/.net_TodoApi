using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;


public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    [Required]
    public string? Name { get; set; }

    public string? Phone { get; set; }
}

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}
