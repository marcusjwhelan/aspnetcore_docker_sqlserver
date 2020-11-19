using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Commander.Models;
using Commander.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Commander
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
              
            // Can do this instead of configuration as shown below
              
            var server = Configuration["DBServer"] ?? "localhost"; // or the container service name "db"
            var port = Configuration["DBPort"] ?? "1433";
            var user = Configuration["DBUser"] ?? "SA";
            var password = Configuration["DBPassword"] ?? "Pa55w0rd!";
            var database = Configuration["Database"] ?? "CommanderDB";
            services.AddDbContext<CommanderContext>(
                options => options.UseSqlServer($"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}")
                );
                
            
            // configure sql here - connection string comes from appsettings.json
            /*services.AddDbContext<CommanderContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("CommanderConnection"))
                );*/
            
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // auto mapper dependency injection service
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                
            // can swap out the MockCommanderRepo for any other repo reprisenation
            // services.AddScoped<ICommanderRepo, MockCommanderRepo>();
            services.AddScoped<ICommanderRepo, SqlCommanderRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // pred database in development environment
                PrepDB.PrepPopulation(app);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
