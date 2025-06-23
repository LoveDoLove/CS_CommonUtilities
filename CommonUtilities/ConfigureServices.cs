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

public static class ConfigureServices
{
    /// <summary>
    ///     Registers all CommonUtilities services and middleware for a new project in a clear, user-friendly sequence.
    ///     <b>Core Steps (in order):</b>
    ///     1. <b>Load configuration</b> from appsettings.json (or custom file).
    ///     2. <b>Register the database context</b> (using the provided connection string name).
    ///     3. <b>Register authentication and session</b> (minimal, project-specific options via
    ///     <paramref name="configureExtras" />).
    ///     4. <b>Bind configuration sections</b> for SMTP, Captcha, etc. (strongly-typed config).
    ///     5. <b>Register core utility services</b> (mailer, captcha, image, etc.).
    ///     6. <b>Add essential ASP.NET Core services</b> (controllers, views, HTTP context, etc.).
    ///     7. <b>Extension point:</b> Use <paramref name="configureExtras" /> to append custom logic, such as:
    ///     - Registering your own services
    ///     - Overriding default implementations
    ///     - Adding project-specific configuration, middleware, or jobs
    ///     <b>Usage Example:</b>
    ///     <code>
    /// var provider = CommonUtilitiesServiceRegistrar.RegisterAllServices(
    ///     appSettingsFile: "appsettings.Development.json",
    ///     connectionStringName: "MyDbConnection",
    ///     configureExtras: services =>
    ///     {
    ///         // Register your custom services here
    ///         services.AddScoped&lt;IMyService, MyService&gt;();
    ///         // Add custom configuration, middleware, jobs, etc.
    ///         // services.AddCronJob&lt;MyJob&gt;(config => { ... });
    ///     });
    /// </code>
    ///     <b>Best Practice:</b> For complex or grouped registrations, use extension methods:
    ///     <code>
    /// services.AddMyCustomServices(configuration);
    /// </code>
    ///     <b>Note:</b> Project-specific options for authentication, session, forwarded headers, CORS, and jobs should be set
    ///     in <paramref name="configureExtras" /> for maximum flexibility and maintainability.
    ///     See official ASP.NET Core documentation for more patterns and guidance.
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
        // Example for registering a custom sync cron job (place in configureExtras):
        // services.AddCronJob<MyCustomSyncService>(config =>
        // {
        //     config.CronExpression = "*/10 * * * *"; // every 10 minutes
        //     config.TimeZoneInfo = TimeZoneInfo.Local;
        // });
        // To implement: public class MyCustomSyncService : SyncServiceBase<MyCustomSyncService> { ... override ExecuteSyncAsync ... }
        // See SyncService.cs for a template.

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