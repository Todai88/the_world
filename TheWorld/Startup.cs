﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TheWorld
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                              .SetBasePath(_env.ContentRootPath)
                              .AddJsonFile("config.json")
                              .AddEnvironmentVariables();

            _config = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development")) {
                services.AddScoped<IMailService, DebugMailService>();
            } else
            {
                // Add real implementation
            }

            services.AddDbContext<WorldContext>();

            services.AddScoped<IWorldRepository, WorldRepository>();

            services.AddTransient<GeoCoordsService>();
            services.AddTransient<WorldContextSeedData>();

            services.AddLogging();

            services.AddMvc(config =>
            {
                if(_env.IsProduction()) { 
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            }).AddJsonOptions(config =>
                   config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
                );

            services.AddIdentity<WorldUser, IdentityRole>(config => { 
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            })
            .AddEntityFrameworkStores<WorldContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory factory)
        {
            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(config => {

                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();

            });

            // Check if client is a development-machine to show stacktraces etc.
            if (_env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                factory.AddDebug(LogLevel.Information);
            } else
            {
                factory.AddDebug(LogLevel.Error);
            }

           
            app.UseMvc(config =>
                {
                    config.MapRoute(
                                    name: "Default",
                                    template: "{controller}/{action}/{id?}",
                                    defaults: new { controller = "App", action = "Index"}
                                    );
                });
            seeder.EnsureSeedData().Wait();
        }
    }
}
