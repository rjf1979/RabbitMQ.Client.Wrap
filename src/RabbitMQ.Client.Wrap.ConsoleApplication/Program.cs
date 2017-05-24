using System;
using System.Net;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Wrap.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string queue1 = "queue-demo";
            var mqClient = Client.Build("admin", "123456", "LogHost", "192.168.117.158");
            mqClient.Publisher.QueueDeclare(queue1);
                var tag = mqClient.Subscriber.Subscribe(queue1, message =>
                {
                    Console.WriteLine($"Recevice Data > {message}，Time > {DateTime.Now}");
                    return true;
                });
                Console.WriteLine($"Subscribe Tag > {tag} ， Time > {DateTime.Now}");
  
            Console.ReadKey();
        }

        static async void Publish()
        {
            //添加一个日志记录操作对象类
            Logger.AddLogger(new MyLogger());
            //然后就可以直接进行


            string queueName = "queue-demo";
            var mqClient = Client.Build("admin", "123456", "LogHost", "192.168.117.158");


            mqClient.Subscriber.RegisterExceptionHandler((message, exception) =>
            {
                //处理消息


                //处理异常


            });




            mqClient.Publisher.QueueDeclare(queueName);
            string messageData = "test-" + DateTime.Now;
            await mqClient.Publisher.Publish(queueName, messageData);

            var tag = mqClient.Subscriber.Subscribe(queueName, message =>
            {
                Console.WriteLine($"Recevice Data > {message}，Time > {DateTime.Now}");
                return true;
            });
            Console.WriteLine($"Subscriber Tag > {tag} ， Time > {DateTime.Now}");
        }
    }
}
