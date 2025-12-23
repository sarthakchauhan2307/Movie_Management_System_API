using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace UserService.Api.Messaging
{
    public class RabbitMQSubscriber : BackgroundService
    {
        #region Configuration
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;
        #endregion

        public RabbitMQSubscriber(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 1️⃣ Create connection
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            // 2️⃣ Declare exchange + queue
            await _channel.ExchangeDeclareAsync(
                exchange: "trigger",
                type: ExchangeType.Fanout,
                cancellationToken: stoppingToken
            );

            var queue = await _channel.QueueDeclareAsync(cancellationToken: stoppingToken);

            await _channel.QueueBindAsync(
                queue: queue.QueueName,
                exchange: "trigger",
                routingKey: "",
                cancellationToken: stoppingToken
            );

            // 3️⃣ Create consumer
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var booking = JsonSerializer.Deserialize<BookingConfirmed>(json);

                Console.WriteLine("Booking Event Received in UserService");
                Console.WriteLine($"BookingId : {booking?.BookingId}");
                Console.WriteLine($"Movie     : {booking?.Title}");
                Console.WriteLine($"Show Time : {booking?.ShowTime}");
                Console.WriteLine($"Seats     : {booking?.SeatCount}");

                await Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(
                queue: queue.QueueName,
                autoAck: true,
                consumer: consumer,
                cancellationToken: stoppingToken
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null) await _channel.CloseAsync(cancellationToken);
            if (_connection != null) await _connection.CloseAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
