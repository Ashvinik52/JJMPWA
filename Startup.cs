using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJMApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static class ShareConnectionString
        {
            public static string JJM { get; set; }
            public static string Z1_JJM { get; set; }
            public static string Z2_JJM { get; set; }
            public static string Z3_JJM { get; set; }
            public static string Z4_JJM { get; set; }
        }
        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProgressiveWebApp();
            services.AddServiceWorker();
            services.AddControllersWithViews();
            services.AddDNTCaptcha(options =>

        options.UseCookieStorageProvider()
        .ShowThousandsSeparators(false));
            var JJM = Configuration.GetConnectionString("ConnStrJJM");
            var Z1_JJM = Configuration.GetConnectionString("ConnStrZ1");
            var Z2_JJM = Configuration.GetConnectionString("ConnStrZ2");
            var Z3_JJM = Configuration.GetConnectionString("ConnStrZ3");
            var Z4_JJM = Configuration.GetConnectionString("ConnStrZ4");
            ShareConnectionString.JJM = JJM;
            ShareConnectionString.Z1_JJM = Z1_JJM;
            ShareConnectionString.Z2_JJM = Z2_JJM;
            ShareConnectionString.Z3_JJM = Z3_JJM;
            ShareConnectionString.Z4_JJM = Z4_JJM;
            services.AddRazorPages();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);

                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
                
                options.Cookie.HttpOnly = true;
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}
