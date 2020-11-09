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
        // GET api/user/userdata
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [HttpGet("loadData")]
        public async Task<ActionResult<List<StatusAtualResponse>>> LoadData()
        {
            var service = new StatusAtualService();
            var result = await service.LoadData();
            foreach(StatusAtualResponse sar in result)
            {
                sar.pH.value = Ultil.AjustaPH(sar.pH.value);
                sar.luminosidade.value = Ultil.AjustaLuminnosidade(sar.luminosidade.value);
                sar.humidSolo.value = Ultil.AjustaUmidadeDoSolo(sar.humidSolo.value);
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
            }
            else
            {
                result = new StatusHistoryRsponse();
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
    }
}
