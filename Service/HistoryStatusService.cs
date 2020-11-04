using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SeedingPrecision.Controllers;
using SeedingPrecision.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision.Service
{
    public class HistoryStatusService: BaseService
    {



        private const string dbName = "sth_helixiot";
        private readonly IMongoCollection<HistoryStatus> historyStatusService;

        public HistoryStatusService(IConfiguration configuration, string NumberOfTable)
        {
            string COLLECTION_NAME = "sth_/_urn:ngsi-ld:entity:" + NumberOfTable + "_iot";
            var ConectionString = "mongodb://helix:H3l1xNG@143.107.145.24:27000/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false";//configuration.GetConnectionString("ConnectionStrings:MongoDbDatabase");
            var MongoClient = new MongoClient(ConectionString);
            var dataBase = MongoClient.GetDatabase(dbName);

            historyStatusService = dataBase.GetCollection<HistoryStatus>(COLLECTION_NAME);
        }

        [HttpGet("GetHistoryStatus")]
        public List<HistoryStatus> GetHistoryStatus()
        {
            var Historys = historyStatusService.Find(e => true).ToList();
            return Historys;
        }

        public async Task<List<StatusAtualResponse>> AjusteHistorys()
        {

            List<HistoryStatus> his = GetHistoryStatus().OrderBy(a => a.recvTime).ToList();
            List<StatusAtualResponse> STR = new List<StatusAtualResponse>();
            StatusAtualResponse statusAtualResponse = new StatusAtualResponse();
            DateTime data = his.First().recvTime;
            foreach (HistoryStatus hs in his)
            {
                if (hs.recvTime != data)
                {
                    statusAtualResponse.Data = data;
                    STR.Add(statusAtualResponse);
                    statusAtualResponse = new StatusAtualResponse();
                    data = hs.recvTime;
                }
                switch (hs.attrName)
                {
                    case "pH":
                        statusAtualResponse.pH = new AtributesResponse.PH();
                        statusAtualResponse.pH.type = hs.attrType;
                        statusAtualResponse.pH.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "luminosidade":
                        statusAtualResponse.luminosidade = new AtributesResponse.Luminosidade();
                        statusAtualResponse.luminosidade.type = hs.attrType;
                        statusAtualResponse.luminosidade.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "tempSolo":
                        statusAtualResponse.tempSolo = new AtributesResponse.TempSolo();
                        statusAtualResponse.tempSolo.type = hs.attrType;
                        statusAtualResponse.tempSolo.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "tempAmbiente":
                        statusAtualResponse.tempAmbiente = new AtributesResponse.TempAmbiente();
                        statusAtualResponse.tempAmbiente.type = hs.attrType;
                        statusAtualResponse.tempAmbiente.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "humidSolo":
                        statusAtualResponse.humidSolo = new AtributesResponse.HumidSolo();
                        statusAtualResponse.humidSolo.type = hs.attrType;
                        statusAtualResponse.humidSolo.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "humidAmbiente":
                        statusAtualResponse.humidAmbiente = new AtributesResponse.HumidAmbiente();
                        statusAtualResponse.humidAmbiente.type = hs.attrType;
                        statusAtualResponse.humidAmbiente.value = Convert.ToDouble(hs.attrValue);
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

            }
            return STR;
        }
        public async Task<IEnumerable<SensorModel>> TakeHistorysBySensor(string Sensor)
        {
            List<HistoryStatus> his = GetHistoryStatus().OrderBy(a => a.recvTime).Where(a=>a.attrName == Sensor).ToList();

            var result = from a in his
                         select new SensorModel
                         {
                             Sensor = a.attrName,
                             Valor = Convert.ToDouble(a.attrValue),
                             Data = a.recvTime
                         };
            return result;

        }
    }
}
