# Nexaas.Id.Client

This is .NET client for the [Nexaas ID](https://id.nexaas.com) API.

Nexaas.Id.Client doesn't store any data. The application must authenticate users using Nexaas.Id.Client methods. 

# Installation

This package is available via Nuget Packages: https://www.nuget.org/packages/Nexaas.ID.Client

**Package Manager**
```nuget
Install-Package Nexaas.ID.Client
```

**.NET CLI**
```nuget
dotnet add package Nexaas.ID.Client
```

**.Paket Cli**
```nuget
paket add Nexaas.ID.Client
```

# Dependencies

.NET Standard >=2.0, Newtonsoft.Json >= 11.0.2

You can check supported frameworks here:

https://docs.microsoft.com/pt-br/dotnet/standard/net-standard

https://www.newtonsoft.com/json



# Configuration
## Create a new application
Go to https://id.nexaas.com/applications and create a new application in your Nexaas ID account.

#### Setup your environment

```c#

var nexaasId = NexaasID.Production(
  "your client id", 
  "your client secret", 
  "your application callback uri");

```

#### Get Nexaas ID login url

```c#

var url = nexaasId.GetAuthorizeUrl();

```

#### Get Nexaas ID authorization token

```c#

BaseResponse<AuthTokenResponse> response = await nexaasId.GetAuthorizationToken("code");

```

#### Get Profile info

```c#

BaseResponse<Profile> response = await nexaasId.GetProfile("access_token");

```

#### Get Profile professional info

```c#

BaseResponse<ProfessionalInfo> response = await nexaasId.GetProfessionalInfo("access_token");

```

#### Get Profile contacts

```c#

BaseResponse<Contacts> response = await nexaasId.GetContacts("access_token");

```

#### Get Profile emails

```c#

BaseResponse<Emails> response = await nexaasId.GetEmails("access_token");

```

#### Invite to application

```C#

BaseResponse<ApplicationInvitiationResponse> response = 
  await nexaasId.InviteToApplication(new ApplicationInvitiationRequest("invited_email", "access_token"));

```

## Asp.Net Core Mvc usage example

[Source code example](https://github.com/wdmatheus/Nexaas.ID.Client/tree/master/examples/MvcNexaasIDClient)

#### Setup Startup.cs

```c#
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
```

#### Add controller to handle authentication

```c#

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

```

