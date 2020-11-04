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

        [HttpGet("listStatusHistory/{NumberOfTable}")]
        public async Task<ActionResult<List<StatusAtualResponse>>> ListStatusHistory(string NumberOfTable)
        {
            HistoryStatusService hss = new HistoryStatusService(_configuration, NumberOfTable);
            var result = await hss.AjusteHistorys();
            return Ok(result);
        }
        [HttpGet("listStatusPerSensor")]
        public async Task<ActionResult<List<SensorModel>>> ListStatusPerSensor(string NumberOfTable, string sensor)
        {
            HistoryStatusService hss = new HistoryStatusService(_configuration, NumberOfTable);
            var result = await hss.TakeHistorysBySensor(sensor);
            return Ok(result);
        }

        [HttpGet("listAllStatusPerSensor")]
        public async Task<ActionResult<List<List<SensorModel>>>> ListAllStatusPerSensor(string NumberOfTable)
        {
            List<IEnumerable<SensorModel>> result = new List<IEnumerable<SensorModel>> { };
            string[] Sensores = { "pH", "luminosidade", "tempSolo", "tempAmbiente", "humidSolo", "humidAmbiente" };
            HistoryStatusService hss = new HistoryStatusService(_configuration, NumberOfTable);

            foreach(string sensor in Sensores)
            {
                var aux = await hss.TakeHistorysBySensor(sensor);
                result.Add(aux);
            }
            return Ok(result);
        }
    }
}
