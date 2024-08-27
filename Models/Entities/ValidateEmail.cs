using System;
using System.Net.Quic;
using System.Text.RegularExpressions;
namespace EmployeeAdminPortal.Models;

public class ValidatemailAddress : Attribute
{
    public string _email;
    public ValidatemailAddress(string email)
    {
        _email = email;
        if (!_email.Contains('@')) {
            Console.WriteLine("email not valid");
        }
        else
        {
            Console.WriteLine("email seems Ok");
        }
    }
}

public interface IMyDependency
{
    void WriteMessage(string message);
}


public class MyDependency : IMyDependency
{
    public void WriteMessage(string message)
    {
        Console.WriteLine($"MyDependency.WriteMessage Message: {message}");
    }
}

// =====================OR========================
// can improve the IMyDependency Interface using the built-in logging API like:

public class MyDependency2 : IMyDependency
{
    private readonly ILogger<MyDependency2> _logger;
    public MyDependency2(ILogger<MyDependency2> logger)
    {
        _logger = logger;
    }
    public void WriteMessage(string message)
    {
        _logger.LogInformation($"MyDependency.WriteMessage Message: {message}");
    }
}

