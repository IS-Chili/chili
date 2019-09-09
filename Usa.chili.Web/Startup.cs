using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Usa.chili.Data;
using Usa.chili.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Logging;
using Usa.chili.Web;
using Microsoft.AspNetCore.Mvc;

namespace Usa.chili
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
            services.AddDbContext<ChiliDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddOptions();

            services.AddScoped<IStationService, StationService>();
            services.Configure<ChiliConfig>(Configuration.GetSection("ChiliConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var policyCollection = new HeaderPolicyCollection()
                .AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyNoReferrer()
                .RemoveServerHeader()
                .AddCustomHeader("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; base-uri 'none'; object-src 'self'; form-action 'self'; style-src 'self' 'unsafe-inline'; font-src 'self'; img-src 'self' data: http://weather.southalabama.edu; sandbox allow-forms allow-scripts allow-same-origin allow-popups; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://polyfill.io; connect-src 'self'; manifest-src 'self'");

            app.UseSecurityHeaders(policyCollection);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            loggerFactory.AddLog4Net();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseMvcWithDefaultRoute();
        }
    }
}
