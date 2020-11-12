using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Driver;
using RestSharp;
using SeedingPrecision.Models.Responses;
using SeedingPrecision.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision.Controllers
{
    [Route("api/")]
    public class DataController : Controller
    {        
        // GET api/user/userdata
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [HttpGet("loadData")]
        public async Task<ActionResult<List<StatusAtualResponse>>> LoadData()
        {
            double maxima=0;
            var service = new StatusAtualService();
            var result = await service.LoadData();
            foreach(StatusAtualResponse sar in result)
            {
                maxima = Ultil.AchaLuminosidadeMaxima(maxima, sar.luminosidade.value);
                sar.pH.value = Ultil.AjustaPH(sar.pH.value);                
                sar.humidSolo.value = Ultil.AjustaUmidadeDoSolo(sar.humidSolo.value);
            }
            foreach (StatusAtualResponse sar in result)
            {
                sar.luminosidade.value = Ultil.AjustaLuminnosidade(sar.luminosidade.value, maxima);
            }
            return Ok(result);
        }
        public class filtros
        {
            public string NumberOfTable { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Agrupamento { get; set; }
        }

        [HttpPost("listStatusHistory")]
        public async Task<ActionResult<StatusHistoryRsponse>> ListStatusHistory([FromBody]filtros filter)
        {
            HistoryStatusService hss = new HistoryStatusService(filter.NumberOfTable, filter.StartDate, filter.EndDate);
            StatusHistoryRsponse result;
            if (hss.his.Count > 0)
            {
                result = await hss.AjusteHistorys(filter.Agrupamento);
                result.isEmpty = false;
            }
            else
            {
                result = new StatusHistoryRsponse();
                result.isEmpty = true;
            }
            return Ok(result);
        }
        [HttpGet("listStatusPerSensor")]
        public async Task<ActionResult<List<SensorModel>>> ListStatusPerSensor(string NumberOfTable, string sensor, string StartDate, string EndDate)
        {
            HistoryStatusService hss = new HistoryStatusService(NumberOfTable, StartDate, EndDate);
            var result = await hss.TakeHistorysBySensor(sensor);
            return Ok(result);
        }

        public class filtro
        {
            public string NumberOfTable { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

        }

        [HttpGet("listAllStatusPerSensor")]
        public async Task<ActionResult<List<List<SensorModel>>>> ListAllStatusPerSensor(string NumberOfTable, string StartDate, string EndDate)
        {
            List<IEnumerable<SensorModel>> result = new List<IEnumerable<SensorModel>> { };
            string[] Sensores = { "pH", "luminosidade", "tempSolo", "tempAmbiente", "humidSolo", "humidAmbiente" };
            HistoryStatusService hss = new HistoryStatusService(NumberOfTable,StartDate,EndDate) ;

            foreach(string sensor in Sensores)
            {
                var aux = await hss.TakeHistorysBySensor(sensor);
                result.Add(aux);
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("getClima")]
        public async Task<string> GetClima()
        {
            var client = new RestClient("http://pt-br.wttr.in/Itaquera?m?2?T");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string html = response.Content;

            var inicial = html.IndexOf("<pre>");
            var final = html.IndexOf("</pre>");
            var length = final - inicial;

            string result = html.Substring(inicial, length -27);
            result = result.Replace("Localização: ", "")+ "</pre>";


            return result;
        }
    }
}
