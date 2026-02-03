using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var authenticationProviderKey = "IdentityServiceKey";

// ? SERILOG MUST BE CONFIGURED HERE (MOST IMPORTANT)
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// --- Services Registration ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));

//2.Register Authentication Services
builder.Services.AddAuthentication(options =>
{
    // Set the default scheme to "Bearer"
    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultAuthenticateScheme = authenticationProviderKey;
    options.DefaultChallengeScheme = authenticationProviderKey;
})
.AddJwtBearer(authenticationProviderKey, options =>
{
    options.Authority = "https://localhost:7055";
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
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = authenticationProviderKey;
//    options.DefaultChallengeScheme = authenticationProviderKey;
//})
//.AddJwtBearer(authenticationProviderKey, options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),

//        // Gateway must be lenient
//        ValidateIssuer = false,
//        ValidateAudience = false,

//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero,

//        RoleClaimType = "role"
//    };
//});


builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

//Serilog request logging (NOW it will work)
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

//  Only once
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Ocelot MUST be last
await app.UseOcelot();

app.Run();
