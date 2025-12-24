using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using TheatreMasterService.Api.Messaging;
using TheatreMasterService.Api.Service;

namespace TheatreMasterService.Api.Subscriber
{
    public class RabbitMQSubscriber : BackgroundService
    {
        #region Configuration
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IConfiguration _configuration;
        public RabbitMQSubscriber(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQHost"],
                    Port = int.Parse(_configuration["RabbitMQPort"]!)
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync("trigger", ExchangeType.Fanout);
                var queue = await _channel.QueueDeclareAsync();

                await _channel.QueueBindAsync(queue.QueueName, "trigger", "");

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (sender, ea) =>
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var booking = JsonSerializer.Deserialize<BookingConfirmed>(json);

                    if (booking != null)
                        await ReduceSeatsAsync(booking);

                    await Task.CompletedTask;
                };
                await _channel.BasicConsumeAsync(queue.QueueName, true, consumer);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("RabbitMQ error: " + ex.Message);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        private async Task ReduceSeatsAsync(BookingConfirmed booking)
        {
            using var scope = _scopeFactory.CreateScope();

            var showService = scope.ServiceProvider.GetRequiredService<IShowService>();

            await showService.ReduceSeatsAsync(
                booking.ShowId,
                booking.SeatCount
            );

            Console.WriteLine(
                $"Seats reduced for Show {booking.ShowId} by {booking.SeatCount}");
        }


    }
}
