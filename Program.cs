using System.Security.Claims;
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using TodoApi;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(
    optionsBuilder => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// builder.Services.AddAuthorization();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
    options.RequireHttpsMetadata = false;
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:messages", policy => policy.Requirements.Add(new
    HasScopeRequirement("read:messages", builder.Configuration["Auth0:Domain"])));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

builder.Services.AddScoped<IMyKeyedServices, MyServices>();

builder.Services.AddScoped<IMyDependency, MyDependency2>(); // Custom logger dependency injection

// middlewares
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.MapControllers();

app.Run();
