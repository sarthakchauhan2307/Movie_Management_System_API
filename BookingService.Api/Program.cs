using BookingService.Api.Data;
using BookingService.Api.Messaging;
using BookingService.Api.Repository;
using BookingService.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});



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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Booking Service", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));

builder.Services.AddAuthentication(options =>
{
    // Set the default scheme to "Bearer"
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidateAudience = true,
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});


builder.Services.AddDbContext<BookingServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookingServiceConnection")));

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingServices>();
builder.Services.AddScoped<IBookingSeatRepository, BookingSeatRepository>();
builder.Services.AddHttpClient<MicroServiceGateway>();

//Log.Logger = new LoggerConfiguration()
//         .MinimumLevel.Information()
//         .Enrich.FromLogContext()
//         .WriteTo.Seq("http://localhost:5341")
//        .CreateLogger();
//add configuration for MessageBusClient
builder.Services.AddSingleton<IMessageBusClient,MessageBusClient>();
//builder.Host.UseSerilog();

//adding Dapper Context
builder.Services.AddScoped<DapperContext>();

var app = builder.Build();

//using seq
app.UseSerilogRequestLogging();

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
