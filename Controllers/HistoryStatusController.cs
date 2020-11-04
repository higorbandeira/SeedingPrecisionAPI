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
    [Route("apiHistory/")]
    public class HistoryStatusController : Controller
    {

        static IConfiguration configuration;
        [HttpGet("listStatusHistory")]
        public async Task<ActionResult<List<StatusAtualResponse>>> ListStatusHistory(string NumberOfTable)
        {           
            HistoryStatusService hss = new HistoryStatusService(configuration, NumberOfTable);
            var result = await hss.AjusteHistorys();
            return Ok(result);
        }

        [HttpGet("takeHistorysBySensor")]
        public async Task<ActionResult<List<SensorModel>>> TakeHistorysBySensor(string NumberOfTable, string sensor)
        {
            HistoryStatusService hss = new HistoryStatusService(configuration, NumberOfTable);
            var result = await hss.TakeHistorysBySensor(sensor);
            return Ok(result);
        }

        [HttpGet("takeHistorysAllSensors")]
        public async Task<ActionResult<List<List<SensorModel>>>> TakeHistorysAllSensors(string NumberOfTable)
        {
            string[] listaSensores = { "pH", "luminosidade", "tempSolo", "tempAmbiente", "humidSolo", "humidAmbiente" };
            HistoryStatusService hss = new HistoryStatusService(configuration, NumberOfTable);
            List<List<SensorModel>> result = new List<List<SensorModel>> { };

            foreach (string sensor in listaSensores)
            {
                var aux = await hss.TakeHistorysBySensor(sensor);
                result.Add(aux.ToList());
            }
            return Ok(result);
        }


    }
}
