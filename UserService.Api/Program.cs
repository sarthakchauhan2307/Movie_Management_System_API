using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using UserService.Api.Data;
using UserService.Api.Messaging;
using UserService.Api.Repositories;
using UserService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserServiceConnection")));

//Adding Repository Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Adding Services DI
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddHttpClient<MicroServiceGateway>();

//add configuration for MessageBusClient
builder.Services.AddHostedService<RabbitMQSubscriber>();

//adding Dapper Context
builder.Services.AddScoped<DapperContext>();

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

//Adding caching
builder.Services.AddMemoryCache();

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
