using System.Collections.Generic;
using AspNetCoreRateLimit;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using People.API.Data;
using People.Data.Entities;
using People.Data.Services;
using Swashbuckle.AspNetCore.Swagger;


namespace People.API
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
            services.AddDbContext<PeopleContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("PeopleConnectionString")); // Configuration["ConnectionStrings:PeopleConnectionString"];
                cfg.EnableSensitiveDataLogging(true); // Not recommend to use it in production. 
            });

            services.AddAutoMapper();
            

            services.AddTransient<PeopleSeeder>();
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddMvc(setupAction =>
                    {
                        setupAction.ReturnHttpNotAcceptable = true;
                        setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()); // Support XML too.
                        setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                    })
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });

            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                                         {
                                             Version = "v1",
                                             Title = "ASP.NET Core Web API demo",
                                             Description = "Demonstration of ASP.NET Core 2.0 Web API and some of its middleware.",
                                             TermsOfService = "None",
                                             Contact = new Contact { Name = "Bozhi Qian", Url = "https://twitter.com/bozhiqian" },
                                             License = new License { Name = "MIT", Url = "https://en.wikipedia.org/wiki/MIT_License" }
                                         });

                //Add XML comment document by uncommenting the following
                // var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MyApi.xml");
                // options.IncludeXmlComments(filePath);

            });

            // Shared caches (.NET Core) - ASP.NET Core 2.0 ResponseCaching middleware.
            services.AddResponseCaching();

            // https://github.com/KevinDockx/HttpCache
            services.AddHttpCacheHeaders(
                expirationModelOptions=>
                {
                    expirationModelOptions.MaxAge = 60;
                },
                validationModelOptions=>
                {
                    validationModelOptions.AddMustRevalidate = true;
                    validationModelOptions.VaryByAll = true;
                });

            #region Rate Litmit -- https://github.com/stefanprodan/AspNetCoreRateLimit

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            // Rate Limiting and Throttling
            services.Configure<IpRateLimitOptions>(options =>
            {
                // https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup
                options.GeneralRules = new List<RateLimitRule>
                                       {
                                           new RateLimitRule
                                           {
                                               Endpoint = "*",
                                               Limit = 1000,
                                               Period = "5m"
                                           },
                                           new RateLimitRule
                                           {
                                               Endpoint = "*",
                                               Limit = 200,
                                               Period = "10s"
                                           }
                                       };
            });

            // inject counter and rules stores
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // loggerFactory.AddNLog(); // this is for ASP.NET Core 1.*, for ASP.NET Core 2.0, it is in "Program.cs". 

            // Seed the database
            using(var scope = app.ApplicationServices.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<PeopleSeeder>();
                seeder.Seed();
            }

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if(exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500,
                                exceptionHandlerFeature.Error,
                                exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseStaticFiles();

            #region Use Swagger           
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            // Visit http://localhost:51888/swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            #endregion

            // Rate Limit
            app.UseIpRateLimiting();

            // Shared caches (.NET Core) - ASP.NET Core 2.0 ResponseCaching middleware.
            app.UseResponseCaching(); // this should be before "app.UseHttpCacheHeaders();"

            // https://github.com/KevinDockx/HttpCache
            app.UseHttpCacheHeaders();

            app.UseMvc();

        }
    }
}
