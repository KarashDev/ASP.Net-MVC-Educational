using ASP.Net_MVC_Educational.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_MVC_Educational
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // � ������������ ��������� �������� �� LocalDB �� SQL Server Express
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CarStoreDbContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            // ���� UseEndPoints(����� ������), ���� UseMvc(������ ������)
            // ���� ��� ������������� ���������� Convention-Based Routing
            // ���� ������������ ������������� �� ������ ���������, ��� ����� ������ �� ���������� endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                // == ���� ������������ ���������������� ������� (Areas), ����� ���������� ��������� ���� � ��� (������� MVC ��� ���)
                //endpoints.MapControllerRoute(
                //   name: "MyArea",
                // pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
