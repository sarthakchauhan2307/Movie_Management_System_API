using Microsoft.EntityFrameworkCore;
using UserService.Api.Data;
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
