using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedingPrecision.Service;

namespace SeedingPrecision.Controllers
{
    [Route("api/mobile")]
    public class MobileController : Controller
    {

        // GET api/user/userdata
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [HttpGet("mobileData")]
        public async Task<ActionResult> MobileData()
        {
            var service = new MobileService();
            var result = await service.MobileData();
            return Ok(result);
        }
    }
}
