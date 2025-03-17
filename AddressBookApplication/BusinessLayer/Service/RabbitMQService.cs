using RabbitMQ.Client;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Service
{
    public class RabbitMQService
    {
        private readonly IConfiguration _configuration;
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _exchange;
        private readonly string _queue;
        private readonly string _routingKey;
        private readonly ConnectionFactory _factory;
        public RabbitMQService()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "localhost", // Ensure RabbitMQ is running locally
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
        }



        public void PublishMessage(string message)
        {
            try
            {
                using var connection = _factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "addressbook_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = System.Text.Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: "addressbook_queue",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Message Sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ Error: {ex.Message}");
            }
        }
    }
}
