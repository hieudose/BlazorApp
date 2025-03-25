using BlazorApp.Web;
using BlazorApp.Web.Authentication;
using BlazorApp.Web.Components;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazoredToast();

builder.Services.AddAuthenticationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddOutputCache();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddHttpClient<ApiClient>(client =>
    {
        client.BaseAddress = new(Environment.GetEnvironmentVariable("API_BASE_URL") ?? "https+http://localhost:7304");
    });

// Add localization services
builder.Services.AddLocalization();
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var supportedCultures = new[] { "en-US", "fr-FR" };
var localizeoptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en-US")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizeoptions);

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
