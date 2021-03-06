﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision
{
    static public class Ultil
    {
        public static double AjustaPH(double PH)
        {

            PH = AjustaDouble((PH - 583.9622641509434) / 259.4339622641509);
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
                return AjustaDouble((100 - HumidSol) * -1);
            else
                return AjustaDouble(100 - HumidSol);
        }

        public static double AjustaLuminnosidade(double Luminosidade, double maxima)
        {
            return AjustaDouble(Luminosidade / (maxima*0.01));
        }

        public static double AchaLuminosidadeMaxima(double a, double b)
        {
            return a > b ? a:b;
        }

        public static double AjustaDouble(string num)
        {
            string[] sep = num.Split('.');
            double a = 0;
            if (!String.IsNullOrEmpty(num) && sep != null)
            {
                a = Convert.ToDouble(sep[0]);
                if (sep.Length > 1&&sep[1]!="0")
                {
                    int b =0;
                    if (sep[1].Length > 2)
                    {
                        b = Convert.ToInt32(Convert.ToInt32(sep[1]) / Math.Pow(10, (sep[2].Length - 2)));
                    }
                    if (sep[1].Length == 1)
                    {
                        b = Convert.ToInt32(Convert.ToInt32(sep[1])*10);
                    }
                    double c = Convert.ToDouble(b);
                    a += (c/100);
                }                
            }
            return a;
        
        }
        public static double AjustaDouble(double num)
        {
            int a = Convert.ToInt32(num * 100);
            double b = Convert.ToDouble(a);
            return b/100;
        }
    }
}
