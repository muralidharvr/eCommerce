using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Fabric;
using System.Fabric.Query;
using Newtonsoft.Json;
using Microsoft.ServiceFabric.Data.Collections;

namespace eCommerceGatewaySvc.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly StatelessServiceContext serviceContext;

        public ValuesController(HttpClient httpClient, StatelessServiceContext context, FabricClient fabricClient)
        {
            this.fabricClient = fabricClient;
            this.httpClient = httpClient;
            this.serviceContext = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return this.Json(new string[] { "value1", "value2" });


            Uri serviceName = eCommerceGatewaySvc.GetVotingDataServiceName(this.serviceContext);
            //Uri proxyAddress = this.GetProxyAddress(serviceName);

            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);

            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();

            foreach (Partition partition in partitions)
            {
                string proxyUrl = "http://localhost:19081/eCommerce/eCommerceOrderAPI/API/Values?PartitionKey=1&PartitionKind=Int64Range";
                //$"{proxyAddress}/api/values?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }

                    result.AddRange(JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(await response.Content.ReadAsStringAsync()));
                }
            }

            return this.Json(result);


        }


        //// PUT api/VoteData/name
        //[HttpPut("{name}")]
        //public async Task<IActionResult> Put(string name)
        //{
        //    IReliableDictionary<string, int> votesDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, int>>("counts");

        //    using (ITransaction tx = this.stateManager.CreateTransaction())
        //    {
        //        await votesDictionary.AddOrUpdateAsync(tx, name, 1, (key, oldvalue) => oldvalue + 1);
        //        await tx.CommitAsync();
        //    }

        //    return new OkResult();
        //}



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



        /// <summary>
        /// Constructs a reverse proxy URL for a given service.
        /// Example: http://localhost:19081/VotingApplication/VotingData/
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private Uri GetProxyAddress(Uri serviceName)
        {
            return new Uri($"http://localhost:19081/eCommerce/eCommerceOrderAPI");//    ---{serviceName.AbsolutePath}");
        }

        /// <summary>
        /// Creates a partition key from the given name.
        /// Uses the zero-based numeric position in the alphabet of the first letter of the name (0-25).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private long GetPartitionKey(string name)
        {
            return Char.ToUpper(name.First()) - 'A';
        }
    }
}
