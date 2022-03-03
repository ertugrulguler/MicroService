using Autofac;
using Autofac.Extensions.DependencyInjection;
using Catalog.Api.Attribute;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Container;
using Catalog.Container.Modules;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Authorization;
using Framework.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

namespace Catalog.Api
{
    //[ExcludeFromCodeCoverage] code covarage'dan çıkartmank için kullanılır
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var environmentNamePath = string.IsNullOrEmpty(environmentName) ? "" : environmentName + ".";
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentNamePath}json", optional: false)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RepositoryModule.AddDbContext(services, Configuration);
            LoggingModule.AddLogging(Configuration);
            services.AddControllers(options => options.Filters.Add(new ValidateModelAttribute(Bootstrapper.Container.Resolve<IAppLogger>())));
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["Token:Endpoint"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //add service for httpContext
            services.AddScoped<ICategoryDomainService, CategoryDomainService>();
            services.AddScoped<IProductDomainService, ProductDomainService>();
            services.AddScoped<IBrandDomainService, BrandDomainService>();
            services.AddScoped<IAttributeDomainService, AttributeDomainService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ApplicationService.Handler.Services.v2.IProductService, ApplicationService.Handler.Services.v2.ProductService>();
            services.AddScoped<IProductVariantService, ProductVariantService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAttributeService, AttributeService>();
            services.AddScoped<IAttributeValueService, AttributeValueService>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });
            services.AddHttpClient("user").SetHandlerLifetime(TimeSpan.FromSeconds(20));
            //services.AddHttpClient("order").SetHandlerLifetime(TimeSpan.FromSeconds(20));
            //services.AddHttpClient("payment").SetHandlerLifetime(TimeSpan.FromSeconds(20));
            services.AddStackExchangeRedisCache(c => c.Configuration = Configuration["RedisConnString"]);
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var container = app.ApplicationServices.GetAutofacRoot();
            Bootstrapper.SetContainer(container);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler("/error");
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API"); });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Bootstrapper.RegisterModules(builder);
        }
    }
}