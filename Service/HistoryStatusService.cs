using Microsoft.Extensions.Configuration;
using SeedingPrecision.Controllers;
using SeedingPrecision.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision.Service
{
    public class HistoryStatusService
    {
        static IConfiguration configuration ;

        HistoryStatusController hsc = new HistoryStatusController(configuration);
            
        public List<StatusAtualResponse> AjusteHistorys()
        {
           
            List<HistoryStatus> his = hsc.GetHistoryStatus().OrderBy(a => a.recvTime).ToList();
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
                switch (hs.attrType)
                {
                    case "pH":
                        statusAtualResponse.pH.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "luminosidade":
                        statusAtualResponse.luminosidade.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "tempSolo":
                        statusAtualResponse.tempSolo.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "tempAmbiente":
                        statusAtualResponse.tempAmbiente.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "humidSolo":
                        statusAtualResponse.humidSolo.value = Convert.ToDouble(hs.attrValue);
                        break;
                    case "humidAmbiente":
                        statusAtualResponse.humidAmbiente.value = Convert.ToDouble(hs.attrValue);
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

            }
            return STR;
        }
    }
}
