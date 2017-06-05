using System;
using System.Net;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Wrap.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //添加一个日志记录操作对象类
            Logger.RegisiterLogger(new MyLogger());
            string queueName = "queue-demo";
            var client = Client.Build("admin", "123456", "LogHost", "192.168.117.158");
            Task.Run(async () => { await Publish(client, queueName); });
            //Subscribe(client, queueName);

            Console.ReadKey();
        }

        static async Task Publish(Client client, string queue)
        {
            client.Publisher.QueueDeclare(queue);
            while (true)
            {
                string messageData = "test-" + DateTime.Now;
                await client.Publisher.Publish(queue, messageData);
            }
        }

        static void Subscribe(Client client, string queue)
        {
            var tag = client.Subscriber.Subscribe(queue, message =>
            {
                Console.WriteLine($"Recevice Data > {message}，Time > {DateTime.Now}");
                return true;
            });
        }
    }
}
