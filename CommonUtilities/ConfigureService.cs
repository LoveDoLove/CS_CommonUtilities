// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using CommonUtilities.Helpers.CfCaptcha;
using CommonUtilities.Helpers.GoogleMfa;
using CommonUtilities.Helpers.IpInfo;
using CommonUtilities.Helpers.Mailer;
using CommonUtilities.Helpers.Media;
using CommonUtilities.Helpers.Scheduler;
using CommonUtilities.Helpers.Stripe;
using CommonUtilities.Models.Database;
using CommonUtilities.Services.Sync;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonUtilities;

public class ConfigureService
{
    /// <summary>
    ///     Configures and injects all CommonUtilities services and middleware for a new project.
    ///     Usage: var provider = ConfigureService.ConfigureServices();
    /// </summary>
    /// <param name="appSettingsFile">The appsettings file to use (default: "appsettings.json").</param>
    /// <param name="connectionStringName">The connection string name (default: "DBConnection").</param>
    /// <param name="configureExtras">Optional: Action to register additional services or override defaults.</param>
    /// <returns>IServiceProvider with all CommonUtilities services registered.</returns>
    public static IServiceProvider ConfigureServices(
        string appSettingsFile = "appsettings.json",
        string connectionStringName = "DBConnection",
        Action<IServiceCollection>? configureExtras = null)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        var services = builder.Services;

        // Step 1: Configure appsettings.json
        builder.Configuration.AddJsonFile(appSettingsFile, false, true);

        // Step 2: Configure database context
        var connectionString = builder.Configuration.GetConnectionString(connectionStringName) ??
                               throw new InvalidOperationException(
                                   $"Connection string '{connectionStringName}' not found.");
        services.AddDbContext<DB>(options => options.UseSqlServer(connectionString));

        // Step 3: Configure authentication and authorization
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.Cookie.Name = "logged_in";
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

        // Step 4: Configure session
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Step 5: Configure CORS
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        // Step 6: Bind configuration sections
        ConfigurationManager configuration = builder.Configuration;
        ConfigureAndRegister<SmtpConfig>(services, configuration, "Smtp");
        ConfigureAndRegister<CfCaptchaConfig>(services, configuration, "CfCaptcha");
        ConfigureAndRegister<IpInfoConfig>(services, configuration, "IpInfo");
        ConfigureAndRegister<StripeConfig>(services, configuration, "Stripe");

        // Step 7: Add application services
        services.AddScoped<IStripeHelper, StripeHelper>();
        services.AddScoped<IImageHelper, ImageHelper>();
        services.AddTransient<IMailerHelper, MailerHelper>();
        services.AddTransient<ICfCaptchaHelper, CfCaptchaHelper>();
        services.AddTransient<IIpInfoHelper, IpInfoHelper>();
        services.AddTransient<IGoogleMfaHelper, GoogleMfaHelper>();

        // Step 8: Add essential services and middleware
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddRazorTemplating();
        services.AddCronJob<SyncService>(config =>
        {
            config.TimeZoneInfo = TimeZoneInfo.Local;
            config.CronExpression = @"*/5 * * * *";
        });

        // Step 9: Allow consumer to register additional services
        configureExtras?.Invoke(services);

        return services.BuildServiceProvider();
    }

    private static T ConfigureAndRegister<T>(IServiceCollection services, IConfiguration configuration,
        string sectionName) where T : class, new()
    {
        T? configValue = configuration.GetSection(sectionName).Get<T>();
        services.AddSingleton(configValue);
        return configValue;
    }
}