using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //Console.WriteLine("🔍 Startup.cs is running...");
        //Console.WriteLine("Auth0 Domain: " + Configuration["Auth0:Domain"]);
        //Console.WriteLine("Auth0 ClientId: " + Configuration["Auth0:ClientId"]);
        //Console.WriteLine("Auth0 ClientSecret: " + (string.IsNullOrEmpty(Configuration["Auth0:ClientSecret"]) ? "Missing" : "Exists")); Section made for debugging

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            options.Authority = Configuration["Auth0:Domain"];
            options.ClientId = Configuration["Auth0:ClientId"];
            options.ClientSecret = Configuration["Auth0:ClientSecret"];

            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            options.CallbackPath = "/callback";
            options.SignedOutCallbackPath = "/signout-callback";

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");

            // roles claim. example: admin, teacher, student.
            options.ClaimActions.MapJsonKey("role","https://gloriachessinstitute.com/claims/roles");
        });

        services.AddAuthorization();
        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
