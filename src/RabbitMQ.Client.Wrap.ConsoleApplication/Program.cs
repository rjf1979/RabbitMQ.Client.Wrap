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

            for (int i = 0; i < 100; i++)
            {
                Task.Run(() =>
                {
                    int n = i;
                    var task = Download(n);
                    Console.WriteLine($"TaskID：{i}  Downloaded Length：{task.Result} > " + DateTime.Now);
                });
            }

            //Stopwatch stopwatch = Stopwatch.StartNew();
            //Console.WriteLine("start... > " + DateTime.Now);
            //IList<Task> tasks = new List<Task>();
            //string queue1 = "test-1";
            //var mqClient = Client.Build("admin", "123456", "LogHost", "192.168.117.158");
            //mqClient.Publisher.QueueDeclare(queue1);
            //for (int i = 0; i < 10000; i++)
            //{
            //    var task = mqClient.Publisher.Publish(queue1, "test_" + i);
            //    task.Wait();
            //    //tasks.Add(task);
            //    //task.Start();
            //}
            ////Task.WaitAll(tasks.ToArray());
            //stopwatch.Stop();
            //Console.WriteLine("over... > " + stopwatch.ElapsedMilliseconds);
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
