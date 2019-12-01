﻿// ********************************************************************************************************************************************
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
            services.AddDbContextPool<ChiliDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(5, 5, 64), ServerType.MariaDb);
                    }
            ));

            // Configure Antiforgery
            services.AddAntiforgery(opts => opts.Cookie.Name = "X-CSRF-TOKEN-COOKIE");

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
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new DoubleConverter());
                });
                
            services.AddRazorPages();

            services.AddOptions();

            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IStationDataService, StationDataService>();
            services.AddScoped<IPublicService, PublicService>();
            services.AddScoped<IVariableService, VariableService>();
            services.Configure<ChiliConfig>(Configuration.GetSection("ChiliConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            var policyCollection = new HeaderPolicyCollection()
                .AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyNoReferrer()
                .RemoveServerHeader()
                .AddCustomHeader("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; base-uri 'none'; object-src 'self'; form-action 'self' https://export.highcharts.com; style-src 'self' 'unsafe-inline'; font-src 'self'; img-src 'self' data: blob: http://weather.southalabama.edu https://api.tiles.mapbox.com; sandbox allow-forms allow-scripts allow-same-origin allow-popups; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://polyfill.io; connect-src 'self'; manifest-src 'self'");

            app.UseSecurityHeaders(policyCollection);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // Enable if site will use HTTPS
                // app.UseHsts();
            }

            // Enable if site will use HTTPS
            // app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
