using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetaPoco;
using System;
using BGAuto.DAL.DBContext;
using BGAuto.DAL.Helpers;
using BGAuto.DAL.Services;
using Microsoft.Extensions.Logging;

namespace BGAuto
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
            services.AddControllersWithViews();

            // Register PetaPoco database instance as a singleton
            services.AddSingleton<IDatabase>(provider =>
            {
                var connectionString = Configuration.GetConnectionString("DBConnection");
                // Assuming you are using SQL Server
                return new Database(connectionString, "SqlServer");
            });

            // Register your services
            services.AddSingleton<IDataContextHelper, DataContextHelper>();
            services.AddSingleton<IBasicDataServices, BasicDataServices>();
            services.AddSingleton<ICustomerServices, CustomerServices>();
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddSingleton<ICustomerURLHelper, CustomerURLHelper>();

            // Session and HttpContext accessors
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<LoginAuthorization>();

            // Session settings
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
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
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=CustomerHome}/{action=Index}/{id?}");
            });
        }
    }
}
