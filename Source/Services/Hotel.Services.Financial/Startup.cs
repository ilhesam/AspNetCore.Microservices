using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Common.MongoDb;
using Hotel.Common.RabbitMq;
using Hotel.Services.Financial.Database;
using Hotel.Services.Financial.Database.Repositories;
using Hotel.Services.Financial.Database.Repositories.Interfaces;
using Hotel.Services.Financial.RabbitMq;
using MediatR;

namespace Hotel.Services.Financial
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
            services.AddSingleton<FinancialDbContext>();

            services.AddTransient<IFinancialTransactionRepository, FinancialTransactionRepository>();

            services.Configure<MongoDbOptions>(_ => Configuration.GetSection("Mongo").Bind(_));

            services.AddRabbitMq<ConsumeFinancialRabbitMqBackgroundService>(_ => Configuration.GetSection("RabbitMQ").Bind(_));

            services.AddMediatR(typeof(Startup));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
