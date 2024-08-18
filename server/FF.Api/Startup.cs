using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FF.Backend.Extensions;
using FF.Backend.Caching.Extensions;

namespace FF.Api
{
    public class Startup
    {
        private readonly string CorsPolicyName = "MyCorePolicy";

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // load app settings
            services.Configure<Backend.Settings>(Configuration.GetSection(nameof(Backend.Settings)));
            var settings = Configuration.GetSection(nameof(FF.Backend.Settings)).Get<Backend.Settings>();

            // caching
            services.AddCaching();

            // backend
            services.AddBackend(Configuration);

            // database
            services.AddDbContext<FF.Backend.Contexts.FFContext>();
            
            // configure Cors
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicyName,
                                  builder =>
                                  {
                                      builder.WithOrigins(settings.Cors.AllowedOrigins);
                                      builder.AllowAnyHeader();
                                      builder.WithMethods(settings.Cors.AllowedMethods);
                                      builder.WithExposedHeaders("Content-Disposition");
                                  });
            });

            services.AddControllers(config =>
            {
                //config.Filters.Add(typeof(Auth.ClaimRequirementFilter));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FF.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AKUVO.Api.Web v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(CorsPolicyName);
            //app.UseAuthentication();
            //app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
