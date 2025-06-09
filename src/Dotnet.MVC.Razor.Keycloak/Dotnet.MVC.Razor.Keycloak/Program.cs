using Dotnet.MVC.Razor.Keycloak.Extensions;
using Dotnet.MVC.Razor.Keycloak.Logics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddMvcAuthentication();
// Add services to the container.
builder.Services
    .AddScoped<IMarkdownRenderer, MarkdownRenderer>()
    .ConfigureDbContextAndRepositories(builder.Configuration)
    .AddControllersWithViews();

var app = builder.Build();
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.SeedDatabase();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
