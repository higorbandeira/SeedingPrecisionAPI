using Newtonsoft.Json;
using RestSharp;
using SeedingPrecision.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeedingPrecision.Service
{
    public class MobileService : BaseService
    {
        public async Task<List<MyArray>> MobileData()
        {
            var client = new RestClient( IPHelix + "/v2/entities");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            request.AddHeader("Authorization", "Basic YWRtaW46QGFkbWluOQ==");
            IRestResponse response = await client.ExecuteAsync(request);
            List<MyArray> myDeserializedClass = JsonConvert.DeserializeObject<List<MyArray>>(response.Content);
            return myDeserializedClass;
        }
    }
}
