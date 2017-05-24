using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Wrap.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {


            string queue1 = "test-1";
            var mqClient = Client.Build("admin", "123456", "LogHost", "192.168.117.158");
            //mqClient.Subscriber.QueueDeclare(queue1);

            Task.Run(async () =>
            {
                while (true)
                {
                    await mqClient.Publisher.Publish(queue1, "test-" + DateTime.Now);
                }
            });

            Task.Run(() =>
            {
                var tag = mqClient.Subscriber.Subscribe(queue1, message =>
                {
                    Console.WriteLine($"Recevice Data > {message}，Time > {DateTime.Now}");
                    return true;
                });
                Console.WriteLine($"Subscribe Tag > {tag} ， Time > {DateTime.Now}");
            });

            Console.ReadKey();
        }


        static async Task<int> Download(int taskID)
        {
            WebClient webClient = new WebClient();
            var contentTask = webClient.DownloadStringTaskAsync(new Uri("http://www.baidu.com"));
            Downloading(taskID);
            var content = await contentTask;
            return content.Length;
        }


        static void Downloading(int taskID)
        {
            Task.Delay(new Random().Next(100));
            Console.WriteLine($"TaskID：{taskID}  Downloading > " + DateTime.Now);
        }

    }
}
