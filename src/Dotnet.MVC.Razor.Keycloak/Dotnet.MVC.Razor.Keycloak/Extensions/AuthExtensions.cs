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
             // Custom cookie name
             options.Cookie.Name = ".DodiTech.Auth";
             options.Cookie.HttpOnly = true;
             // HTTPS only
             options.Cookie.SecurePolicy = CookieSecurePolicy.None;
             // Cookie lifetime
             options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
             // Extend lifetime on activity. This renews the cookie's expiration with activity, reducing idle logout errors.
             options.SlidingExpiration = true;

             // Fallback login path
             options.LoginPath = "/Auth/login";

             // Logout redirect
             options.LogoutPath = "/Auth/logout";

             // 403 path
             options.AccessDeniedPath = "/Auth/AccessDenied";
         })
         .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
         {
             options.Authority = builder.Configuration["Keycloak:Authority"];
             options.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"];
             options.ClientId = builder.Configuration["Keycloak:ClientId"];
             options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"];
             options.ResponseType = "code";
             options.RequireHttpsMetadata = bool.TryParse(builder.Configuration["Keycloak:RequireHttpsMetadata"], out var requireHttps) && requireHttps; // Set to true for production
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 NameClaimType = "preferred_username",
                 RoleClaimType = "roles",
                 ValidIssuer = builder.Configuration["Keycloak:ValidIssuer"],
                 ValidateAudience = true,
                 ValidateIssuer = true
             };
             options.Events = new OpenIdConnectEvents
             {
                 OnRemoteFailure = context =>
                 {
                     var error = context.Failure?.Message;
                     var exception = context.Failure;
                     //logger.Error("OIDC Authentication Failed: " + error);

                     // Optional redirect
                     context.Response.Redirect("/Auth/Login");
                     context.HandleResponse(); // Suppress the exception
                     return Task.CompletedTask;
                 }
             };

             options.SaveTokens = true;
             options.GetClaimsFromUserInfoEndpoint = true;
             //This prevents ASP.NET from using the token expiration as the session expiration.
             //This ensures the session cookie lives according to the ExpireTimeSpan in Cookie configuration above, not the token lifetime.
             options.UseTokenLifetime = false;

             options.Scope.Add("openid");
             options.Scope.Add("profile");
             options.Scope.Add("email");
             options.Scope.Add("offline_access"); // Add offline_access scope for refresh token
             options.Scope.Add("roles"); // Add roles scope if needed
         });

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true; // Check for user consent
            options.MinimumSameSitePolicy = SameSiteMode.Lax; // Set SameSite policy
        });

        return builder;
    }
}
