using EmailService.Api.Messaging;
using EmailService.Api.Model;
using EmailService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddHttpClient<MicroServiceGateway>(client =>
//{
//    client.BaseAddress =
//        new Uri(builder.Configuration["MicroServiceUrls:BookingService"]!);
//});

builder.Services.Configure<EmailSetting>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddHttpClient<MicroServiceGateway>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddHostedService<BookingConfirmedConsumer>();

builder.Services.AddHttpClient<MicroServiceGateway>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
