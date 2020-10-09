using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SeedingPrecision.Helpers;
using SeedingPrecision.Models.Identity;
using SeedingPrecision.Models.Requests;
using SeedingPrecision.Models.Responses;

namespace SeedingPrecision.Controllers
{
    [EnableCors("AllowAllHeaders")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // GET api/user/userdata
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getAll")]
        public async Task<ActionResult> UserData()
        {
            var user = await _userManager.GetUserAsync(User);
            var userData = new UserDataResponse
            {
                Name = user.Name,
                Farm = user.Farm,
                UserName = user.UserName
            };
            return Ok(userData);
        }

        // POST api/user/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterEntity model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Name = model.Name, Farm = model.Farm, UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    var token = AuthenticationHelper.GenerateJwtToken(model.UserName, user, _configuration);

                    var rootData = new SignUpResponse(token, user.UserName);
                    return Created("api/authentication/register", rootData);
                }
                return Ok(string.Join(",", result.Errors?.Select(error => error.Description)));
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }


        // POST api/user/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginEntity model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.UserName);
                    var token = AuthenticationHelper.GenerateJwtToken(model.UserName, appUser, _configuration);

                    var rootData = new LoginResponse(token, appUser.UserName);
                    return Ok(rootData);
                }
                return StatusCode((int)HttpStatusCode.Unauthorized, "Bad Credentials ");
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }
    }
}
