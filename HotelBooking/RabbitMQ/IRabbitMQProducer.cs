namespace Hotel.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        public void SendHotelBookingtMessage<T>(T message);
    }
}
