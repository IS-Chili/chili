// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Usa.chili.Data;
using Usa.chili.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;
using Usa.chili.Web.Converters;

namespace Usa.chili.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure DB Context with the connection string
            services.AddDbContextPool<ChiliDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(5, 5, 64), ServerType.MariaDb);
                    }
            ));

            // Configure Antiforgery
            services.AddAntiforgery(opts => opts.Cookie.Name = "X-CSRF-TOKEN-COOKIE");

            // Configure Cookie Policy
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.SameAsRequest; 
            });

            // Enable if site will use HTTPS
            /*
            services.AddHsts(options =>
            {
                options.Preload = false;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
                options.HttpsPort = 443;
            });
            */

            services
                // Add MVC support
                .AddControllersWithViews()
                // Add View hot reload support
                .AddRazorRuntimeCompilation()
                // Add JSON converters
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new DoubleConverter());
                });

            // Add the scoped services for dependency injection support
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IStationDataService, StationDataService>();
            services.AddScoped<IPublicService, PublicService>();
            services.AddScoped<IVariableService, VariableService>();

            // Add ChiliConfig support for future options added to the appsettings.json files
            services.AddOptions();
            services.Configure<ChiliConfig>(Configuration.GetSection("ChiliConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILogger<Startup> logger)
        {
            // Configure forwarding headers to Apache or IIS
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            // Configure security headers
            var policyCollection = new HeaderPolicyCollection()
                .AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyNoReferrer()
                .RemoveServerHeader()
                .AddCustomHeader("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; base-uri 'none'; object-src 'self'; form-action 'self' https://export.highcharts.com; style-src 'self' 'unsafe-inline'; font-src 'self'; img-src 'self' data: blob: http://weather.southalabama.edu https://api.tiles.mapbox.com; sandbox allow-forms allow-scripts allow-same-origin allow-popups; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://polyfill.io; connect-src 'self'; manifest-src 'self'");

            // Add security headers
            app.UseSecurityHeaders(policyCollection);

            // Configure options for local development and production
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Handle exceptions and return HTTP 500
                app.UseGlobalExceptionHandler(logger);

                // Enable if site will use HTTPS
                // app.UseHsts();
            }

            // Enable if site will use HTTPS
            // app.UseHttpsRedirection();

           // Configure static file support with cache-control
           app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Configure max-age for 1 year
                    const int durationInSeconds = 31557600;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            // Use the Cookie Policy
            app.UseCookiePolicy();

            // Handle HTTP Errors
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            // Use routing
            app.UseRouting();

            // Set default endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
