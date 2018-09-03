namespace RabbitMQ.Client.Standard.Wrap
{
    /// <summary>
    /// 
    /// </summary>
    public class Result
    {
        private Result()
        {
            IsSuccess = true;
        }

        private Result(string failMessage)
        {
            FailMessage = failMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get;private set; }
        /// <summary>
        /// 
        /// </summary>
        public string FailMessage { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Result Success()
        {
            return  new Result();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="failMessage"></param>
        /// <returns></returns>
        public static Result Failure(string failMessage)
        {
            return new Result(failMessage);
        }
    }
}
