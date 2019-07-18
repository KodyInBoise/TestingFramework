using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingFramework.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestingFramework.Extensions;

namespace TestingFramework
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.Configure<CookiePolicyOptions>(options =>
         {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         });

#if DEBUG
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("PostgreSQL-Dev")));
#else
         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseNpgsql(
                 Configuration.GetConnectionString("PostgreSQL")));
#endif

         services.AddDefaultIdentity<IdentityUser>()
             .AddDefaultUI(UIFramework.Bootstrap4)
             .AddEntityFrameworkStores<ApplicationDbContext>();

         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseStaticFiles();

         app.UseAuthentication();

         app.UseMvc(routes =>
         {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
         });


         LoggingUtil.Initialize();

         try
         {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
               var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

               context.Database.Migrate();

               RoleHelper.SeedAppRoles(context);
            }

            LoggingUtil.AddEntry("APP", "Application started up and applied database migrations.");
         }
         catch (Exception ex)
         {
            LoggingUtil.AddEntry("APP", $"Exception occurred applying database migrations: {ex.Message}");
         }
      }
   }
}
