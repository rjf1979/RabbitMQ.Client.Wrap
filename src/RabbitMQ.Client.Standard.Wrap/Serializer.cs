using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap
{
    public class Serializer: ISerializer
    {
        public TObject Deserialize<TObject>(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TObject>(data);
        }

        public string Serialize(object value)
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
    }
}
