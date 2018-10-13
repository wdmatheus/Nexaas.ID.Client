using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexaas.ID.Client;

namespace MvcNexaasIDClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly NexaasID _nexaasId;

        public AuthController(NexaasID nexaasId)
        {
            _nexaasId = nexaasId;
        }

        [AllowAnonymous, HttpGet]
        public IActionResult Index() => Redirect(_nexaasId.GetAuthorizeUrl());

        [HttpGet]
        public async Task<IActionResult> Signin(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
                Redirect(_nexaasId.GetAuthorizeUrl());
            
            var authTokenResponse = await _nexaasId.GetAuthorizationToken(code);
            
            var profileResponse = await _nexaasId.GetProfile(authTokenResponse.Data);

            var profile = profileResponse.Data;
            
            var claims = new []
            {
                new Claim(ClaimTypes.Name, profile.FullName),
                new Claim(ClaimTypes.Email, profile.Email),
                new Claim(ClaimTypes.Gender, profile.Gender ?? ""),
                new Claim(ClaimTypes.DateOfBirth, profile.Birth?.ToString() ?? ""),
                new Claim("NexaasIDAccessToken", authTokenResponse.Data.AccessToken), 
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));

            return Redirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }
    }
}