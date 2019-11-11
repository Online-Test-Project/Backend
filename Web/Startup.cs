using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Web.AppStart;
using Web.Common;
using Web.Models;
using Web.Services;

namespace Web
{
    public class Startup
    {
        private IConfiguration configuration { get; }
        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // CP
            services.AddSingleton<IJWTHandler, JWTHandler>();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                .Build());
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var sp = services.BuildServiceProvider();
            var JWTHandler = sp.GetRequiredService<IJWTHandler>();
            services.AddMvc(options =>
            {
                options.Filters.Add(new ExceptionResponseAttribute()); // an instance
                options.Filters.Add(new AuthenticationFilter(configuration, JWTHandler));
            });

            // END CP

            services.AddDbContext<OnlineTestContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("OnlineTestContext")));
            services.AddMvc();

            services.Scan(scan => scan
                .FromAssemblyOf<ITransientService>()
                    .AddClasses(classes => classes.AssignableTo<ITransientService>())
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
                    .AddClasses(classes => classes.AssignableTo<IScopedService>())
                        .As<IScopedService>()
                        .WithScopedLifetime());
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseCors("CorsPolicy");
            app.UseMvcWithDefaultRoute();
            app.UseAuthentication();


        } 

    }
}
