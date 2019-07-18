using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Standard.Wrap;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IFactory _factory;
        private readonly IPublisher _publisher;
        private readonly ISubscriber _subscriber;
        public ValuesController(IFactory factory)
        {
            _factory = factory;
            _publisher = _factory.GetPublisher("StatusFlow");
            _subscriber = _factory.GetSubscriber("StatusFlow");
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //_publisher.Publish("test","123456789");
            _subscriber.Subscribe("test", val =>
            {
                Console.WriteLine(val);
            });
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
