using TheatreMaster.Api.Data;
using Microsoft.EntityFrameworkCore;
using TheatreMasterService.Api.Repository;
using TheatreMasterService.Api.Service;
using TheatreMasterService.Api.Subscriber;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TheatreMasterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TheatreMasterServiceConnection")));

//services registration for repository and service layers goes here
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

//adding http client factory for interservice communication
builder.Services.AddHttpClient<MicroServiceGateway>();

//adding background service for rabbitmq subscriber
builder.Services.AddHostedService<RabbitMQSubscriber>();

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
