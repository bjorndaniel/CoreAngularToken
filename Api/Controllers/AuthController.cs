using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthTokens.Helpers;
using AuthTokens.Model;
using AuthTokens.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AuthTokens.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel model)
        {
            var identity = await GetClaimsIdentity(model.UserName, model.Password);
            if(identity == null){
                return BadRequest("");
            }
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, model.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify == null)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}