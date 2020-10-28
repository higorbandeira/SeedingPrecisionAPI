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
    public class HistoryStatusController
    {

        static IConfiguration configuration;
        [HttpGet("listStatusHistory")]
        public List<StatusAtualResponse> ListStatusHistory(string NumberOfTable)
        {           
            HistoryStatusService hss = new HistoryStatusService(configuration, NumberOfTable);
            return hss.AjusteHistorys();
        }
    }
}
