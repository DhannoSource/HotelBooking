using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Transaction.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Channels;
using Transaction.Repository;

namespace Hotel.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IModel _channel;
        
        private readonly IConnection _connection;

        private readonly TransactionRespository _transactionRepository;

        public RabbitMQConsumer(TransactionRespository transactionRepository)
        {
            var factory = new ConnectionFactory
            {
                HostName = "RabbitMQ",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            //Create the RabbitMQ connection using connection factory details as mentioned above
            _connection = factory.CreateConnection();
            //Here we create channel with session and model
            _channel = _connection.CreateModel();
            _transactionRepository = transactionRepository;

        }
        public void ReceiveBookingMessage(Action<string> onMessageReceived)
        {
            //declare the queue after mentioning name and a few property related to that
            _channel.QueueDeclare("hotelbooking", exclusive: false);
            //Set Event object which listen message from chanel which is sent by producer
            //_channel.QueueBind(queue: "hotelbooking", exchange: "hotel", routingKey: "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onMessageReceived(message);
                Console.WriteLine($"Hotel Booking message received: {message}");
            };
            //read the message
            _channel.BasicConsume(queue: "hotelbooking", autoAck: true, consumer: consumer);
            
        }
        
        public void Start()
        {
            Transaction.Models.Transaction msg = new Transaction.Models.Transaction();

            ReceiveBookingMessage(message =>
            {
                // Save the message to the database

                Transaction.Models.Transaction msg = JsonConvert.DeserializeObject<Transaction.Models.Transaction>(message);
                msg.Id = Guid.NewGuid();
                _transactionRepository.ReceiveTransaction(msg);

            });
        }

        public void SubscribeToTopic(Action<string> onMessageReceived)
        {
            //declare the queue after mentioning name and a few property related to that
            //_channel.QueueDeclare("hotelbooking", exclusive: false);
            //_channel.ExchangeDeclare(exchange: settings.TopicName, type: "fanout");
            //var queueName = _channel.QueueDeclare().QueueName;
            //_channel.QueueBind(queue: queueName, exchange: settings.TopicName, routingKey: "");
            //_channel.BasicConsume(queue: queueName, onMessage: (model, message) =>
            //{
            //    var body = message.Body;
            //    var messageText = Encoding.UTF8.GetString(body);
            //    onMessageReceived(messageText);
            //});
        }
    }
    }


