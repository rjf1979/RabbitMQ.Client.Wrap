namespace RabbitMQ.Client.Standard.Wrap.Interface
{
    public interface ISerializer
    {
        TObject Deserialize<TObject>(string data);

        string Serialize(object value);
    }
}
