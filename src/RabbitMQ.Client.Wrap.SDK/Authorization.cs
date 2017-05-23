namespace RabbitMQ.Client.Wrap
{
    /// <summary>
    /// 
    /// </summary>
    internal class Authorization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="vhost"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public Authorization(string userName, string password, string vhost, string host,int port=5672)
        {
            UserName = userName;
            Password = password;
            VHost = vhost;
            Host = host;
            Port = port;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string VHost { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; private set; }
    }
}
