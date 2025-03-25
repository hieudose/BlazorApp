using BlazorApp.BL.Repositories;
using BlazorApp.BL.Services;
using BlazorApp.Database.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Text;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings()
                                                .GetCurrentClassLogger();
logger.Debug("init main");
try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add service defaults & Aspire components.
    builder.AddServiceDefaults();
    builder.Services.AddLocalization();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
            },
            Array.Empty<string>()
        }
        });
    });

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    var secret = builder.Configuration.GetValue<string>("Jwt:Secret");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "doseHieu",
            ValidAudience = "doseHieu",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });
    builder.Services.AddAuthorization();
    builder.AddRedisDistributedCache("cache");


    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();


    // Add services to the container.
    builder.Services.AddProblemDetails();

    var app = builder.Build();


    // Configure the HTTP request pipeline.
    app.UseExceptionHandler();
    app.UseAuthentication();
    app.UseAuthorization();

    var supportedCultures = new[] { "en-US", "fr-FR" };
    var localizeoptions = new RequestLocalizationOptions()
        .SetDefaultCulture("en-US")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    app.UseRequestLocalization(localizeoptions);

    app.MapControllers();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapDefaultEndpoints();

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}