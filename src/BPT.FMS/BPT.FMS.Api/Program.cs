using BPT.FMS.Api.Controllers;
using BPT.FMS.Application.Features.ChartOfAccount.Commands;
using BPT.FMS.Domain;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure;
using BPT.FMS.Infrastructure.Data;
using BPT.FMS.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddMediatR(cfg =>
{
    // If your handlers are in a different assembly, register that instead
    cfg.RegisterServicesFromAssembly(typeof(ChartOfAccountAddCommand).Assembly);
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ChartOfAccountValidator>();

builder.Services.AddScoped<IChartOfAccountRepository,ChartOfAccountRepository>();
builder.Services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
