using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexaas.ID.Client;
using Nexaas.ID.Client.Entities;

namespace MvcNexaasIDClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly NexaasID _nexaasId;

        /// Inject NexaasID
        public AuthController(NexaasID nexaasId)
        {
            _nexaasId = nexaasId;
        }

        /// <summary>
        /// Get and redirect to Nexaas ID authorize url
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous, HttpGet]
        public IActionResult Index() => Redirect(_nexaasId.GetAuthorizeUrl());

        /// <summary>
        /// Action callback, invoked by Nexaas ID after user authentication
        /// </summary>
        /// <param name="code"></param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Signin(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
                Redirect(_nexaasId.GetAuthorizeUrl());
            
            ///Retrive user access token
            BaseResponse<OauthTokenResponse> authTokenResponse = await _nexaasId.GetAuthorizationToken(code);
            
            ///Retrive user data
            BaseResponse<Profile> profileResponse = await _nexaasId.GetProfile(authTokenResponse.Data);

            Profile profile = profileResponse.Data;
            
            ///Define user claims
            var claims = new []
            {
                new Claim(ClaimTypes.Name, profile.FullName),
                new Claim(ClaimTypes.Email, profile.Email),
                new Claim("NexaasIDAccessToken", authTokenResponse.Data.AccessToken), 
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            ///Authenticates user
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