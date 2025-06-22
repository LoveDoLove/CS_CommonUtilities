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
using CommonUtilities.Helpers.IpInfo;
using CommonUtilities.Helpers.Mailer;
using CommonUtilities.Helpers.Media;
using CommonUtilities.Helpers.Stripe;
using CommonUtilities.Models.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonUtilities;

public static class CommonUtilitiesServiceRegistrar
{
    /// <summary>
    ///     Registers all CommonUtilities services and middleware for a new project.
    ///     <b>Core Steps:</b>
    ///     1. Loads configuration from appsettings.json.
    ///     2. Registers the database context.
    ///     3. Configures authentication and session.
    ///     4. Binds configuration sections for SMTP, Captcha, etc.
    ///     5. Registers core utility services (mailer, captcha, etc.).
    ///     6. Adds essential ASP.NET Core services (controllers, views, etc.).
    ///     <b>Extension Point:</b>
    ///     Use the <paramref name="configureExtras" /> parameter to append custom logic, such as:
    ///     - Registering your own services
    ///     - Overriding default implementations
    ///     - Adding project-specific configuration
    ///     <b>Example:</b>
    ///     <code>
    /// var provider = CommonUtilitiesServiceRegistrar.RegisterAllServices(
    ///     configureExtras: services =>
    ///     {
    ///         // Register your custom services here
    ///         services.AddScoped&lt;IMyService, MyService&gt;();
    ///         // Add custom configuration, middleware, etc.
    ///     });
    /// </code>
    ///     <b>Best Practice:</b>
    ///     For complex or grouped registrations, consider using extension methods:
    ///     <code>
    /// services.AddMyCustomServices(configuration);
    /// </code>
    ///     See official ASP.NET Core documentation for more patterns.
    /// </summary>
    /// <param name="appSettingsFile">The appsettings file to use (default: "appsettings.json").</param>
    /// <param name="connectionStringName">The connection string name (default: "DBConnection").</param>
    /// <param name="configureExtras">Optional: Action to register additional services or override defaults.</param>
    /// <returns>IServiceProvider with all CommonUtilities services registered.</returns>
    public static IServiceProvider RegisterAllServices(
        string appSettingsFile = "appsettings.json",
        string connectionStringName = "DBConnection",
        Action<IServiceCollection>? configureExtras = null)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        IServiceCollection services = builder.Services;

        // Step 1: Configure appsettings.json
        builder.Configuration.AddJsonFile(appSettingsFile, false, true);

        // Step 2: Configure database context
        string connectionString = builder.Configuration.GetConnectionString(connectionStringName) ??
                                  throw new InvalidOperationException(
                                      $"Connection string '{connectionStringName}' not found.");
        services.AddDbContext<DB>(options => options.UseSqlServer(connectionString));

        // Step 3: Configure authentication (minimal, project-specific options should be set via configureExtras)
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
        // Example for custom authentication options (place in configureExtras):
        // services.ConfigureApplicationCookie(options =>
        // {
        //     options.LoginPath = "/Login";
        //     options.LogoutPath = "/Account/Logout";
        //     options.AccessDeniedPath = "/Home/AccessDenied";
        //     options.Cookie.Name = "logged_in";
        //     options.SlidingExpiration = true;
        //     options.Cookie.HttpOnly = true;
        //     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // });

        // Step 4: Configure session (minimal, project-specific options should be set via configureExtras)
        services.AddSession();
        // Example for custom session options (place in configureExtras):
        // services.Configure<SessionOptions>(options =>
        // {
        //     options.IdleTimeout = TimeSpan.FromMinutes(30);
        //     options.Cookie.HttpOnly = true;
        //     options.Cookie.IsEssential = true;
        // });

        // Step 5: Configure forwarded headers (minimal, project-specific options should be set via configureExtras)
        services.Configure<ForwardedHeadersOptions>(options => { });
        // Example for custom forwarded headers options (place in configureExtras):
        // services.Configure<ForwardedHeadersOptions>(options =>
        // {
        //     options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        //     options.KnownNetworks.Clear();
        //     options.KnownProxies.Clear();
        // });

        // Step 6: Bind configuration sections
        ConfigurationManager configuration = builder.Configuration;
        RegisterConfigSection<SmtpConfig>(services, configuration, "Smtp");
        RegisterConfigSection<CfCaptchaConfig>(services, configuration, "CfCaptcha");
        RegisterConfigSection<IpInfoConfig>(services, configuration, "IpInfo");
        RegisterConfigSection<StripeConfig>(services, configuration, "Stripe");

        // Step 7: Add application services
        services.AddScoped<IStripeHelper, StripeHelper>();
        services.AddScoped<IImageHelper, ImageHelper>();
        services.AddTransient<IMailerHelper, MailerHelper>();
        services.AddTransient<ICfCaptchaHelper, CfCaptchaHelper>();
        services.AddTransient<IIpInfoHelper, IpInfoHelper>();

        // Step 8: Add essential services and middleware
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddRazorTemplating();
        // Example for registering a cron job (place in configureExtras):
        // services.AddCronJob<SyncService>(config =>
        // {
        //     config.TimeZoneInfo = TimeZoneInfo.Local;
        //     config.CronExpression = @"*/5 * * * *";
        // });

        // Step 9: Allow consumer to register additional services
        configureExtras?.Invoke(services);

        return services.BuildServiceProvider();
    }

    public static T RegisterConfigSection<T>(IServiceCollection services, IConfiguration configuration,
        string sectionName) where T : class, new()
    {
        T? configValue = configuration.GetSection(sectionName).Get<T>();
        if (configValue == null)
            throw new InvalidOperationException(
                $"Configuration section '{sectionName}' is missing or invalid for type {typeof(T).Name}.");
        services.AddSingleton(configValue);
        return configValue;
    }
}