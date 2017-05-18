using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Wrap.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine("start... > " + DateTime.Now);
            IList<Task> tasks = new List<Task>();
            string queue1 = "test-1";
            var mqClient = Client.Build("admin", "123456", "LogHost", "192.168.117.158");
            mqClient.Publisher.QueueDeclare(queue1);
            for (int i = 0; i < 10000; i++)
            {
                var task = mqClient.Publisher.Publish(queue1, "test_" + i);
                task.Wait();
                //tasks.Add(task);
                //task.Start();
            }
            //Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            Console.WriteLine("over... > "+ stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
