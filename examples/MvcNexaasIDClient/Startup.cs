using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexaas.ID.Client;

namespace MvcNexaasIDClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get;  }
        
        public void ConfigureServices(IServiceCollection services)
        {
            //Retrive Nexaas ID application configuration
            var clientId = Configuration.GetSection("NexaasIDConfig:ClientId").Value;
            var clientSecret = Configuration.GetSection("NexaasIDConfig:clientSecret").Value;
            var redirectUri = Configuration.GetSection("NexaasIDConfig:RedirectUri").Value;

            //Setup Nexaas.ID.Client
            var nexaasId = Env.IsProduction()
                ? NexaasID.Production(clientId, clientSecret, redirectUri)
                : NexaasID.Sandbox(clientId, clientSecret, redirectUri);

            //Add Nexaas.ID.Client in DI container
            services.AddSingleton(nexaasId);
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            //Configure cookie authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "NexaasIdCookie";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(7200);
                    options.LoginPath = "/auth";
                });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
      
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}