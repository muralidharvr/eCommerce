using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;

namespace eCommerceOrderAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        //private readonly IReliableStateManager stateManager;


        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return new string[] { "key1": "dsdsfdsvalue1", "valuedsfds2" };

            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();

            result.Add(new KeyValuePair<string, int>("key1", 1));
            result.Add(new KeyValuePair<string, int>("key2", 2));
            result.Add(new KeyValuePair<string, int>("key3", 3));

            return this.Json(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
