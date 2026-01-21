using BookingService.Api.Data;
using BookingService.Api.Messaging;
using BookingService.Api.Repository;
using BookingService.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Connection String for raddis
var reddisConnection = builder.Configuration.GetConnectionString("RaddisURL");

// Configure Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = reddisConnection;
    options.InstanceName = "BookingServiceInstance";
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BookingServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookingServiceConnection")));

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingServices>();
builder.Services.AddScoped<IBookingSeatRepository, BookingSeatRepository>();
builder.Services.AddHttpClient<MicroServiceGateway>();

//add configuration for MessageBusClient
builder.Services.AddSingleton<IMessageBusClient,MessageBusClient>();

//adding Dapper Context
builder.Services.AddScoped<DapperContext>();

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
