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

            PH = Convert.ToDouble(((PH - 583.9622641509434) / 259.4339622641509).ToString("F").Replace(".", ","));
            if (PH >= 0)
                return PH;
            else
                return PH * -1;
        }
        public static double AjustaUmidadeDoSolo(double HumidSol)
        {
            HumidSol = (HumidSol * 0.0557103064066852) - 128.133704735376;
            HumidSol = HumidSol > 100 ? 100 : HumidSol < 0 ? 0 : HumidSol;

            if (HumidSol > 50)
                return Convert.ToDouble(((100 - HumidSol) * -1).ToString("F").Replace(".", ","));
            else
                return Convert.ToDouble(((100 - HumidSol)).ToString("F").Replace(".", ","));
        }

        public static double AjustaLuminnosidade(double Luminosidade, double maxima)
        {
            return Convert.ToDouble((Luminosidade / (maxima*0.01)).ToString("F").Replace(".", ","));
        }

        public static double AchaLuminosidadeMaxima(double a, double b)
        {
            return a > b ? a:b;
        }
    }
}
