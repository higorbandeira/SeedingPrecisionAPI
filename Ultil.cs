using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision
{
    static public class Ultil
    {
        public static double AjustaPH(double PH)
        {
            return (PH - 583.9622641509434 / 259.4339622641509) ;
        }
        public static double AjustaUmidadeDoSolo(double HumidSol)
        {
            HumidSol = (HumidSol * 0.0557103064066852) - 128.133704735376;
            HumidSol = HumidSol > 100 ? 100 : HumidSol < 0 ? 0 : HumidSol;

            if (HumidSol > 50)
                return (100 - HumidSol) * -1;
            else
                return (100 - HumidSol);
        }

        public static double AjustaLuminnosidade(double Luminosidade)
        {
            return Luminosidade / 3;
        }
    }
}
