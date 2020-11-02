using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class DataController : Controller
    {
        // GET api/user/userdata
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [HttpGet("loadData")]
        public async Task<ActionResult<List<StatusAtualResponse>>> LoadData()
        {
            var service = new StatusAtualService();
            var result = await service.LoadData();
            return Ok(result);
        }

        [HttpGet("listStatusHistory/{NumberOfTable}")]
        public async Task<ActionResult<List<StatusAtualResponse>>> ListStatusHistory(string NumberOfTable)
        {
            HistoryStatusService hss = new HistoryStatusService();
            var result = await hss.GetHistoryStatus(NumberOfTable);
            return Ok(result);
        }
    }
}
