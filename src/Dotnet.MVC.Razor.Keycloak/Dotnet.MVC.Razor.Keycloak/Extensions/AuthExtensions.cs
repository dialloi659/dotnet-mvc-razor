using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Dotnet.MVC.Razor.Keycloak.Extensions;

public static class AuthExtensions
{
    public static WebApplicationBuilder AddMvcAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
          {
              options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
          })
     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
     {
         options.Cookie.Name = ".DotnetMvcRazorKeycloak.Auth";              // Custom cookie name
         options.Cookie.HttpOnly = true;
         options.Cookie.SecurePolicy = CookieSecurePolicy.None; // HTTPS only
         options.ExpireTimeSpan = TimeSpan.FromMinutes(60);        // Cookie lifetime
         options.SlidingExpiration = true;                         // Extend lifetime on activity
         options.LoginPath = "/Auth/login";                     // Fallback login path
         options.LogoutPath = "/Auth/logout";                   // Logout redirect
         options.AccessDeniedPath = "/AccessDenied";      // 403 path
     })
     .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
     {
         options.Authority = builder.Configuration["Keycloak:Authority"];
         options.ClientId = builder.Configuration["Keycloak:ClientId"];
         options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"];
         options.ResponseType = "code";
         options.RequireHttpsMetadata = false; // Set to true in production

         options.SaveTokens = true;
         options.GetClaimsFromUserInfoEndpoint = true;

         options.Scope.Add("openid");
         options.Scope.Add("profile");
         options.Scope.Add("email");
         options.Scope.Add("offline_access"); // Add offline_access scope for refresh token
         options.Scope.Add("roles"); // Add roles scope if needed

         options.TokenValidationParameters = new TokenValidationParameters
         {
             NameClaimType = "preferred_username",
             RoleClaimType = "roles"
         };
     });

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true; // Check for user consent
            options.MinimumSameSitePolicy = SameSiteMode.Lax; // Set SameSite policy
        });

        return builder;
    }
}
