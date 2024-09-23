using System;

namespace TodoApi.Services;

public class MyServices : IMyKeyedServices
{

    // a return cache method
    public void CachePage(string msg)
    {
        Console.WriteLine($"Calling from a cache page: {msg}");
    }
}

public interface IMyKeyedServices
{
    void CachePage(string msg);
}


