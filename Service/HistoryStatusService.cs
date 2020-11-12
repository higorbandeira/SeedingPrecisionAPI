using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json.Schema;
using SeedingPrecision.Controllers;
using SeedingPrecision.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SeedingPrecision.Service
{
    public class HistoryStatusService : BaseService
    {



        private const string dbName = "sth_helixiot";
        private readonly IMongoCollection<HistoryStatus> historyStatusService;

        public List<HistoryStatus> his;

        public HistoryStatusService(string NumberOfTable, string StartDate, string EndDate)
        {
            // VARIABLES
            string dbName = "sth_helixiot";
            IMongoCollection<HistoryStatus> historyStatusService;

            //CONNECTION
            string COLLECTION_NAME = "sth_/_" + NumberOfTable + "_iot";
            var ConectionString = "mongodb://helix:H3l1xNG@143.107.145.24:27000/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false";//configuration.GetConnectionString("ConnectionStrings:MongoDbDatabase");
            var MongoClient = new MongoClient(ConectionString);
            var dataBase = MongoClient.GetDatabase(dbName);

            //GET DOCUMENT
            historyStatusService = dataBase.GetCollection<HistoryStatus>(COLLECTION_NAME);
            try 
            {
                his = historyStatusService.Find(e => true).ToList().OrderBy(a => a.recvTime).ToList();
            }
            catch
            {
                his = new List<HistoryStatus>();
            }
            
            if (his.Count>0)
            {
                StartDate = StartDate != "null" ? StartDate : null;
                EndDate = EndDate != "null" ? EndDate : null;
                if (!String.IsNullOrEmpty(StartDate))
                {
                    DateTime data = DateTime.Parse(StartDate);
                    his = his.Where(a => a.recvTime >= data).ToList();
                }
                if (!String.IsNullOrEmpty(EndDate))
                {
                    DateTime data = DateTime.Parse(EndDate);
                    his = his.Where(a => a.recvTime <= data).ToList();
                }
            }
        }
        private void zeraVetores(double[] doubles, int[] ints, bool[] bools)
        {
            for (int i= 0; i < doubles.Length; i++)
            {
                doubles[i] = 0;
            }
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = 0;
            }
            for (int i = 0; i < bools.Length; i++)
            {
                bools[i] = true;
            }

        }
        public async Task<StatusHistoryRsponse> AjusteHistorys(string agrupamento)
        {
            StatusHistoryRsponse STR = new StatusHistoryRsponse();
            StatusAtualResponse statusAtualResponse = new StatusAtualResponse();
            DateTime data = his.First().recvTime;
            double[] dadosAgroup = new double[6];
            int[] contadores = new int[6];
            bool[] NotNull = new bool[6];
            double luminosidadeMaxima=0;

            string dataFormat = agrupamento == "Dia" ? "{0:dd/MM/yy}" : agrupamento == "Mes" ? "{0:MMM/yy}" : agrupamento == "Ano" ? "{0:yyyy}" : "{0:d/M HH}";
            if (data != null)
            {
                STR.Data = new List<string>();                
                STR.Data.Add(String.Format(dataFormat, data));
                STR.pH = new List<double?>();
                STR.humidAmbiente = new List<double?>();
                STR.humidSolo = new List<double?>();
                STR.luminosidade = new List<double?>();
                STR.tempAmbiente = new List<double?>();
                STR.tempSolo = new List<double?>();
                zeraVetores(dadosAgroup, contadores, NotNull);
            }
            foreach (HistoryStatus hs in his) 
            {
                if (hs.attrName== "luminosidade") 
                {                    
                    luminosidadeMaxima = Ultil.AchaLuminosidadeMaxima(luminosidadeMaxima, Ultil.AjustaDouble(hs.attrValue));
                }
            }
            foreach (HistoryStatus hs in his)
            {
                if(agrupamento == "Dia")
                {
                    if (hs.recvTime.Day != data.Day || hs.recvTime.Month != data.Month || hs.recvTime.Year != data.Year)
                    {
                        agrupaPorPeriodo(hs, STR, ref data, dadosAgroup, contadores, NotNull, dataFormat);
                    }
                    else
                    {
                        switch (hs.attrName)
                        {
                            case "pH":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[0]++;
                                    dadosAgroup[0] += Ultil.AjustaPH(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[0] = false;
                                }
                                break;
                            case "luminosidade":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[1]++;
                                    
                                    dadosAgroup[1] += Ultil.AjustaLuminnosidade(Ultil.AjustaDouble(hs.attrValue), luminosidadeMaxima);
                                    NotNull[1] = false;
                                }
                                break;
                            case "tempSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[2]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[2] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[2] = false;
                                }
                                break;
                            case "tempAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[3]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[3] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[3] = false;
                                }
                                break;
                            case "humidSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[4]++;
                                    
                                    dadosAgroup[4] += Ultil.AjustaUmidadeDoSolo(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[4] = false;
                                }
                                break;
                            case "humidAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[5]++;
                                    
                                    dadosAgroup[5] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[5] = false;
                                }
                                break;
                        }
                    }
                }
                if(agrupamento == "Mes") 
                {
                    if (hs.recvTime.Month != data.Month || hs.recvTime.Year != data.Year)
                    {
                        agrupaPorPeriodo(hs, STR, ref data, dadosAgroup, contadores, NotNull, dataFormat);
                    }
                    else
                    {
                        switch (hs.attrName)
                        {
                            case "pH":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[0]++;
                                    
                                    dadosAgroup[0] += Ultil.AjustaPH(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[0] = false;
                                }
                                break;
                            case "luminosidade":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[1]++;
                                    
                                    dadosAgroup[1] += Ultil.AjustaLuminnosidade(Ultil.AjustaDouble(hs.attrValue), luminosidadeMaxima);
                                    NotNull[1] = false;
                                }
                                break;
                            case "tempSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[2]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[2] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[2] = false;
                                }
                                break;
                            case "tempAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[3]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[3] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[3] = false;
                                }
                                break;
                            case "humidSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[4]++;
                                    
                                    dadosAgroup[4] += Ultil.AjustaUmidadeDoSolo(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[4] = false;
                                }
                                break;
                            case "humidAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[5]++;
                                    
                                    dadosAgroup[5] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[5] = false;
                                }
                                break;
                        }
                    }
                }
                if (agrupamento == "Ano") 
                {
                    if (hs.recvTime.Year != data.Year)
                    {
                        agrupaPorPeriodo(hs, STR, ref data, dadosAgroup, contadores, NotNull, dataFormat);
                    }
                    else
                    {
                        switch (hs.attrName)
                        {
                            case "pH":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[0]++;
                                    
                                    dadosAgroup[0] += Ultil.AjustaPH(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[0] = false; 
                                }
                                break;
                            case "luminosidade":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[1]++;
                                    
                                    dadosAgroup[1] += Ultil.AjustaLuminnosidade(Ultil.AjustaDouble(hs.attrValue), luminosidadeMaxima);
                                    NotNull[1] = false;
                                }
                                break;
                            case "tempSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[2]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[2] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[2] = false;
                                }
                                break;
                            case "tempAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[3]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[3] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[3] = false;
                                }
                                break;
                            case "humidSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[4]++;
                                    
                                    dadosAgroup[4] += Ultil.AjustaUmidadeDoSolo(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[4] = false;
                                }
                                break;
                            case "humidAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[5]++;
                                    
                                    dadosAgroup[5] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[5] = false;
                                }
                                break;
                        }
                    }
                }
                if (agrupamento == "Hora")
                {
                    if (hs.recvTime.Hour != data.Hour || hs.recvTime.Day != data.Day || hs.recvTime.Month != data.Month || hs.recvTime.Year != data.Year)
                    {
                        agrupaPorPeriodo(hs, STR, ref data, dadosAgroup, contadores, NotNull, dataFormat);
                    }
                    else
                    {
                        switch (hs.attrName)
                        {
                            case "pH":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[0]++;
                                    
                                    dadosAgroup[0] += Ultil.AjustaPH(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[0] = false;
                                }
                                break;
                            case "luminosidade":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[1]++;
                                    
                                    dadosAgroup[1] += Ultil.AjustaLuminnosidade(Ultil.AjustaDouble(hs.attrValue),luminosidadeMaxima);
                                    NotNull[1] = false;
                                }
                                break;
                            case "tempSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[2]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[2] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[2] = false;
                                }
                                break;
                            case "tempAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[3]++;
                                    
                                    var a = Ultil.AjustaDouble(hs.attrValue);
                                    dadosAgroup[3] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[3] = false;
                                }
                                break;
                            case "humidSolo":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[4]++;
                                    
                                    dadosAgroup[4] += Ultil.AjustaUmidadeDoSolo(Ultil.AjustaDouble(hs.attrValue));
                                    NotNull[4] = false;
                                }
                                break;
                            case "humidAmbiente":
                                if (!String.IsNullOrEmpty(hs.attrValue))
                                {
                                    contadores[5]++;
                                    
                                    dadosAgroup[5] += Ultil.AjustaDouble(hs.attrValue);
                                    NotNull[5] = false;
                                }
                                break;
                        }
                    }
                }

            }
            return STR;
        }
        private void agrupaPorPeriodo(HistoryStatus hs, StatusHistoryRsponse STR,ref DateTime data, double[] dadosAgroup, int[] contadores, bool[] NotNull, string dataFormat)
        {
            STR.Data.Add(String.Format(dataFormat, hs.recvTime));
            data = hs.recvTime;
            if (NotNull[0])
            {
                STR.pH.Add(null);
            }
            else
                STR.pH.Add(dadosAgroup[0] / contadores[0]);
            if (NotNull[1])
            {
                STR.luminosidade.Add(null);
            }
            else
                STR.luminosidade.Add(dadosAgroup[1] / contadores[1]);
            if (NotNull[2])
            {
                STR.tempSolo.Add(null);
            }
            else
                STR.tempSolo.Add(dadosAgroup[2] / contadores[2]);
            if (NotNull[3])
            {
                STR.tempAmbiente.Add(null);
            }
            else
                STR.tempAmbiente.Add(dadosAgroup[3] / contadores[3]);
            if (NotNull[4])
            {
                STR.humidSolo.Add(null);
            }
            else
                STR.humidSolo.Add(dadosAgroup[4] / contadores[4]);
            if (NotNull[5])
            {
                STR.humidAmbiente.Add(null);
            }
            else
                STR.humidAmbiente.Add(dadosAgroup[5] / contadores[5]);
            zeraVetores(dadosAgroup, contadores, NotNull);
        }
    
        
        public async Task<IEnumerable<SensorModel>> TakeHistorysBySensor(string Sensor)
        {
            var hisfiltrado = his.OrderBy(a => a.recvTime).Where(a => a.attrName == Sensor).ToList();

            var result = from a in hisfiltrado
                         select new SensorModel
                         {
                             Sensor = a.attrName,
                             Valor = Ultil.AjustaDouble(a.attrValue),
                             Data = a.recvTime
                         };
            return result;

        }
    }
}
