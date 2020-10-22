using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SeedingPrecision.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision.Service
{
    public class StatusAtualService : BaseService
    {
        public async Task<List<StatusAtualResponse>> LoadData()
        {
            var client = new RestClient( IPHelix + "/v2/entities");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            request.AddHeader("Authorization", "Basic YWRtaW46QGFkbWluOQ==");
            IRestResponse response = await client.ExecuteAsync(request);
            List<StatusAtualResponse> myDeserializedClass = JsonConvert.DeserializeObject<List<StatusAtualResponse>>(response.Content);

            return myDeserializedClass;
        }
    }
}
