using System;
using System.Net;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Wrap.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string queue1 = "test-1";
            var mqClient = Client.Build("admin", "123456", "LogHost", "192.168.117.158");
            Task.Run(async () =>
            {
                mqClient.Publisher.QueueDeclare(queue1);
                while (true)
                {
                    await mqClient.Publisher.Publish(queue1, "test-" + DateTime.Now);
                }
            });

            Task.Run(() =>
            {
                mqClient.Subscriber.QueueDeclare(queue1);
                var tag = mqClient.Subscriber.Subscribe(queue1, message =>
                {
                    Console.WriteLine($"Recevice Data > {message}，Time > {DateTime.Now}");
                    return true;
                });
                Console.WriteLine($"Subscribe Tag > {tag} ， Time > {DateTime.Now}");
            });

            Console.ReadKey();
        }
    }
}
