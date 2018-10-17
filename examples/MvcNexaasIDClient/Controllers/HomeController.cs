using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcNexaasIDClient.Models;
using Nexaas.ID.Client;

namespace MvcNexaasIDClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }
        
        [Authorize]
        public async Task<IActionResult> ProfileInfo([FromServices]NexaasID nexaasId)
        {
            var accessToken = User.FindFirstValue("NexaasIDAccessToken");

            if (string.IsNullOrWhiteSpace(accessToken))
                return await Task.FromResult<IActionResult>(Redirect("~/auth"));

            var profileResponse = await nexaasId.GetProfile(accessToken);
            
            if(profileResponse.StatusCode != HttpStatusCode.OK)
                return View();

            profileResponse.Data.Emails = (await nexaasId.GetEmails(accessToken))?.Data;
            
            profileResponse.Data.ProfessionalInfo = (await nexaasId.GetProfessionalInfo(accessToken))?.Data;
            
            profileResponse.Data.Contacts = (await nexaasId.GetContacts(accessToken))?.Data;

            return View(profileResponse.Data);
        }
        
        
        public async Task<IActionResult> ClientCredentials([FromServices]NexaasID nexaasId)
        {

            var tokenResponse = await nexaasId.GetClientAuthorizationToken("profile invite");

            return View(tokenResponse?.Data.AccessToken);
        }
    }
}