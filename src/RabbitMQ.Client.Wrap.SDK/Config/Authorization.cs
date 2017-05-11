namespace RabbitMQ.Client.Wrap.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class Authorization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="vhost"></param>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        public Authorization(string userName, string password, string vhost, string hostName,int port=5672)
        {
            UserName = userName;
            Password = password;
            VHost = vhost;
            HostName = hostName;
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
        public string HostName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; private set; }
    }
}
