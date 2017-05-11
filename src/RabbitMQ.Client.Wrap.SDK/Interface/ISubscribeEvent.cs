namespace RabbitMQ.Client.Wrap.Interface
{
    /// <summary>
    /// 订阅事件
    /// </summary>
    public interface ISubscribeEvent
    {
        /// <summary>
        /// 回调方法
        /// </summary>
        /// <param name="receiveMessage"></param>
        /// <returns></returns>
        bool Call(string receiveMessage);
    }
}
