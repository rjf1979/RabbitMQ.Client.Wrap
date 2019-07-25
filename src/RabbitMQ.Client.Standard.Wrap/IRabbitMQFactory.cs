using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap
{
    public interface IRabbitMQFactory
    {
        IPublisher GetPublisher(string name);
        ISubscriber GetSubscriber(string name);
    }
}
