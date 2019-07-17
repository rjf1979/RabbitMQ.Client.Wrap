using System;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    public class Publisher : Queue, IPublisher
    {
        private bool _isQueueDeclare = false;
        public Publisher(ILogger logger,RabbitMQConfigOption option) : base(logger, option)
        {
            
        }

        public void Publish(string queue, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            //EnterLogEvent("当前Channel：" + Channel.GetHashCode());
            //lock (_locker)
            {
                var body = Encoding.UTF8.GetBytes(message);
                try
                {
                    if (Channel.IsOpen)
                    {
                        if (!_isQueueDeclare)
                        {
                            QueueDeclare(queue);
                            _isQueueDeclare = true;
                        }

                        //if (Option.ExchangeType == ExchangeType.Fanout)
                        //{
                        //    Channel.BasicPublish(Option.Exchange, string.Empty, BasicProperties, body);
                        //}
                        //else if (Option.ExchangeType == ExchangeType.Direct)
                        //{
                        //    Channel.BasicPublish(Option.Exchange, Option.RouteKey, BasicProperties, body);
                        //}
                        //else
                        //{
                            Channel.BasicPublish(string.Empty, queue, BasicProperties, body);
                        //}
                    }
                    else
                    {
                        var msg = "Channel is Not Opened";
                        ApplicationException applicationException = new ApplicationException(msg);
                        EnterLogEvent(msg, applicationException);
                    }
                }
                catch (Exception exception)
                {
                    var msg = $"queue：{queue} > BasicPublish Failed,Time:{DateTime.Now}";
                    EnterLogEvent(msg, exception);
                }
            }
        }

        public void Dispose()
        {
            Channel.Abort();
            Connection.Abort();
            Channel.Dispose();
            Connection.Dispose();
        }
    }
}
