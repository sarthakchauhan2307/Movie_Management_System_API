using EmailService.Api.Messaging;
using EmailService.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class BookingConfirmedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    private IConnection? _connection;
    private IChannel? _channel;

    public BookingConfirmedConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConnectRabbitAsync(stoppingToken);
        await StartConsumerAsync(stoppingToken);
    }

    private async Task ConnectRabbitAsync(CancellationToken token)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"]!)
        };

        _connection = await factory.CreateConnectionAsync(token);
        _channel = await _connection.CreateChannelAsync(cancellationToken: token);

        await _channel.ExchangeDeclareAsync(
            exchange: "trigger",
            type: ExchangeType.Fanout
        );
    }

    // Consumer Start
    private async Task StartConsumerAsync(CancellationToken token)
    {
        if (_channel == null)
            throw new Exception("RabbitMQ channel not created");

        var queue = await _channel.QueueDeclareAsync();
        await _channel.QueueBindAsync(queue.QueueName, "trigger", "");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var booking = JsonSerializer.Deserialize<BookingConfirmed>(
                    Encoding.UTF8.GetString(ea.Body.ToArray())
                );

                if (booking == null) return;

                var gateway = scope.ServiceProvider.GetRequiredService<MicroServiceGateway>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                var pdfBytes = await gateway.DownloadTicketAsync(booking.BookingId);

                var userEmail = await gateway.GetUserEmailAsync(booking.UserId);

                var emailBody = $@"
                    <h2>Booking Confirmed 🎉</h2>
                    <p><b>Movie:</b> {booking.Title}</p>
                    <p><b>Show Time:</b> {booking.ShowTime}</p>
                    <p>Ticket attached.</p>
                ";

                await emailSender.SendAsync(
                    userEmail,
                    "Your Movie Ticket 🎟️",
                    emailBody,
                    pdfBytes
                );

                Console.WriteLine($"Email sent for Booking {booking.BookingId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Consumer Error: " + ex.Message);
            }
        };

        await _channel.BasicConsumeAsync(queue.QueueName, true, consumer);
    }
}
