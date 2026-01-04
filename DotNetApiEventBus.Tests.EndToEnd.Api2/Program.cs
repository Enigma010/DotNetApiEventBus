using DotNetApiEventBus;
using DotNetApiEventBus.Tests.EndToEnd;
using DotNetApiEventBus.Tests.EndToEnd.Api2.Di;
using DotNetApiEventBusCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DddConfig.ConfigureEnvironmentVariables(TestsConfig.Domain, "Api2");
EventBusConfig.ConfigureEnvironmentVariables(TestsConfig.DefaultHostName, TestsConfig.DefaultUsername,
    TestsConfig.DefaultPassword, TestsConfig.DefaultPort);

builder.AddDependencies();

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
