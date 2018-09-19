QQ群：55087803

# RabbitMQ.Client.Wrap Demo

Support automatic recovery reconnection and need reference RabbitMQ.Client SDK


            string queue1 = "queue-demo";
            
            //define a MQ client
            var mqClient = Client.Build("admin", "123456", "vHost", "192.168.xxx.xxx");
            
            //Start Publish
            mqClient.Publisher.QueueDeclare(queue1);
            while (true)
            {
                await mqClient.Publisher.Publish(queue1, "test-" + DateTime.Now);
            }            

            //Start Subscribe
            mqClient.Subscriber.QueueDeclare(queue1);
            var tag = mqClient.Subscriber.Subscribe(queue1, message =>
            {
                Console.WriteLine($"Recevice Data > {message}，Time > {DateTime.Now}");
                return true;
            });
            Console.WriteLine($"Subscribe Tag > {tag} ， Time > {DateTime.Now}");
            
            
.NET CORE demo

    public interface IStatusFlowMessageBus
    {
        Task PublishAsync(StatusFlow statusFlow);
        void Subscribe(Action<StatusFlow> func);
    }
    
    public class StatusFlowMessageBus: IStatusFlowMessageBus
    {
        private readonly IPublisher _publisher;
        private readonly ISubscriber _subscriber;

        public StatusFlowMessageBus(RabbitMqConfigOption option)
        {
            _publisher = RabbitMqFactory.CreatePublisher(option);
            _subscriber = RabbitMqFactory.CreateSubscriber(option);
        }

        public async Task PublishAsync(StatusFlow statusFlow)
        {
            await _publisher.PublishAsync(statusFlow);
        }

        public void Subscribe(Action<StatusFlow> func)
        {
            _subscriber.Subscribe(func);
        }
    }
    
    //IOC 在使用上，各自可以根据实际情况灵活运用              
    services.AddSingleton<IStatusFlowMessageBus>(a => new StatusFlowMessageBus(new RabbitMqConfigOption
    {
        ConnectionString = Configuration.GetConnectionString("StatusFlowMQ"),
        Logger = a.GetService<ILoggerFactory>().CreateLogger("StatusFlowMQ"),
        Topic = "WaitProcess"
    }));
