using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision.Models.Responses
{
    public class AtributesResponse
    {
        public class Metadata
        {
        }

        public class HumidAmbiente
        {
            public string type { get; set; }
            public double value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class HumidSolo
        {
            public string type { get; set; }
            public double value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Luminosidade
        {
            public string type { get; set; }
            public double value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class TempAmbiente
        {
            public string type { get; set; }
            public double value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class TempSolo
        {
            public string type { get; set; }
            public double value { get; set; }
            public Metadata metadata { get; set; }
        }

        public class PH
        {
            public string type { get; set; }
            public double value { get; set; }
            public Metadata metadata { get; set; }
        }
    }
}
