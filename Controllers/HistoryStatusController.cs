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
    public class HistoryStatusController
    {

        private const string dbName = "sth_helixiot";
        private const string COLLECTION_NAME = "sth_/_urn:ngsi-ld:entity:001_iot";
        private readonly IMongoCollection<HistoryStatus> historyStatusService;
        
        public HistoryStatusController(IConfiguration configuration)
        {
            var ConectionString = configuration.GetConnectionString("ConnectionStrings:MongoDbDatabase");
            var MongoClient = new MongoClient(ConectionString);
            var dataBase = MongoClient.GetDatabase(dbName);

            historyStatusService = dataBase.GetCollection<HistoryStatus>(COLLECTION_NAME);
        }


        [HttpGet]
        public List<HistoryStatus> GetHistoryStatus()
        {          
            var Historys = historyStatusService.Find(e => true).ToList();
            return Historys;            
        }
    }
}
