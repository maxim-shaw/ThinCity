using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using ThinCity.AbstractRepository;
using ThinCity.WebAPI.Builders;

namespace ThinCity.WebAPI
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
            services.AddCors();

            // Connection string stored in the environment variables of the host environment.
            var dbConnString = Environment.GetEnvironmentVariable(Configuration.GetValue<string>("ConnectionStringKey"));
            // Alternativelly store connection string in the settings file: Configuration.GetConnectionString("DatabaseContext");
            if (string.IsNullOrWhiteSpace(dbConnString))
            {
                throw new ArgumentNullException(nameof(dbConnString), "Connection string is required.");
            }

            services.AddDbContext<DatabaseContext>(options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(dbConnString));

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDependencyInjections(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
