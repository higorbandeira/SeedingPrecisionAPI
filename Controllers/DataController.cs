using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
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
        private readonly IConfiguration _configuration;
        // GET api/user/userdata
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [HttpGet("loadData")]
        public async Task<ActionResult<List<StatusAtualResponse>>> LoadData()
        {
            var service = new StatusAtualService();
            var result = await service.LoadData();
            return Ok(result);
        }
        public class filtros
        {
            public string NumberOfTable { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        [HttpPost("listStatusHistory")]
        public async Task<ActionResult<List<StatusHistoryRsponse>>> ListStatusHistory([FromBody]filtros filter)
        {
            HistoryStatusService hss = new HistoryStatusService(_configuration, filter.NumberOfTable, filter.StartDate, filter.EndDate);
            var result = await hss.AjusteHistorys();
            return Ok(result);
        }
        [HttpGet("listStatusPerSensor")]
        public async Task<ActionResult<List<SensorModel>>> ListStatusPerSensor(string NumberOfTable, string sensor, string StartDate, string EndDate)
        {
            HistoryStatusService hss = new HistoryStatusService(_configuration, NumberOfTable, StartDate, EndDate);
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
            HistoryStatusService hss = new HistoryStatusService(_configuration, NumberOfTable,StartDate,EndDate) ;

            foreach(string sensor in Sensores)
            {
                var aux = await hss.TakeHistorysBySensor(sensor);
                result.Add(aux);
            }
            return Ok(result);
        }
    }
}
