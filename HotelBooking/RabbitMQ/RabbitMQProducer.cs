using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using System.Runtime;
using System.Threading.Channels;

namespace Hotel.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public void SendHotelBookingtMessage<T>(T message)
        {
           // Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
             var factory = new ConnectionFactory
             {
                 HostName = "RabbitMQ",
                 Port = 5672,
                 UserName = "guest",
                 Password = "guest"
             };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using var channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("hotelbooking", exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the hotelbooking queue
            channel.BasicPublish(exchange: "", routingKey: "hotelbooking", body: body);
        }
    }
}


