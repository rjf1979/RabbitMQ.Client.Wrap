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
            
            
