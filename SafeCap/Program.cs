using DotNetEnv;
using Microsoft.OpenApi.Models;
using SafeCap.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using SafeCap.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Env.Load();

builder.Services.AddAutoMapper(typeof(UserMapping));
builder.Services.AddAutoMapper(typeof(SensorReadingMapping));
builder.Services.AddAutoMapper(typeof(AlertMapping));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API do projeto SafeCap",
        Version = "v1",
        Description = "API do projeto SafeCap da Global Solution do primeiro semestre de 2025.",
        Contact = new OpenApiContact
        {
            Name = "Thomaz Bartol",
            Email = "rm555323@fiap.com.br"
        }
    });
});

var oracleConnectionString = Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(oracleConnectionString));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Garante que escute no Docker
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
