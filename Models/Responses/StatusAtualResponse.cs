using System;
using System.Collections.Generic;
using static SeedingPrecision.Models.Responses.AtributesResponse;

namespace SeedingPrecision.Models.Responses
{
    public class StatusAtualResponse
    {
        public string id { get; set; }
        public string type { get; set; }
        public HumidAmbiente humidAmbiente { get; set; }
        public HumidSolo humidSolo { get; set; }
        public Luminosidade luminosidade { get; set; }
        public TempAmbiente tempAmbiente { get; set; }
        public TempSolo tempSolo { get; set; }
        public PH pH { get; set; }
        public DateTime? Data { get; internal set; }
    }
}
