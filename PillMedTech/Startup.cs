using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PillMedTech.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

namespace PillMedTech
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    //Konstruktor
    public Startup(IConfiguration config) => Configuration = config;

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddDbContext<LoggingDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("LoggerConnection")));
            //1.2. Kryptera k�nslig data

            services.AddDataProtection();

      services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
           

      services.AddTransient<IPillMedTechRepository, EFPillMedTechRepository>();


      services.AddControllersWithViews();

            // implement password policy (4.6. Implementera l�senordspolicy)
            services.Configure<IdentityOptions>(options =>
            {
                // 4.6.Implementera l�senordspolicy
                // N�r L�senord sparas i databasen �r dom hashade (4.5.)
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
                // Lockout (4.4. F�rdr�jning och/eller captcha vid flera f�rs�k, samt lockout vid f�r m�nga felaktiga f�rs�k)
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });
        }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseStatusCodePages();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}
