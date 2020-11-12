using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision.Models.Responses
{
    public class StatusHistoryRsponse
    {
        public List<double?> humidAmbiente { get; set; }
        public List<double?> humidSolo { get; set; }
        public List<double?> luminosidade { get; set; }
        public List<double?> tempAmbiente { get; set; }
        public List<double?> tempSolo { get; set; }
        public List<double?> pH { get; set; }
        public List<string> Data { get;  set; }
        public bool? isEmpty { get; set; }
    }
}
