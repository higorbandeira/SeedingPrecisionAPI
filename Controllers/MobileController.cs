using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SeedingPrecision.Models.Responses;

namespace SeedingPrecision.Controllers
{
    [Route("api/mobile")]
    public class MobileController : Controller
    {

        // GET api/user/userdata
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [HttpGet("mobileData")]
        public async Task<ActionResult> MobileData()
        {
            var client = new RestClient("http://104.215.116.161:1026/v2/entities");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            request.AddHeader("Authorization", "Basic YWRtaW46QGFkbWluOQ==");
            IRestResponse response = client.Execute(request);
            List<MyArray> myDeserializedClass = JsonConvert.DeserializeObject<List<MyArray>>(response.Content);
            return Ok(myDeserializedClass);
        }
    }
}
