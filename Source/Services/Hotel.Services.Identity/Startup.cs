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
using System.Reflection;
using System.Threading.Tasks;
using Hotel.Common.Features.Identity.Commands.SignUp;
using Hotel.Common.MongoDb;
using Hotel.Common.RabbitMq;
using Hotel.Services.Identity.Core.Handlers.Commands.SignUp;
using Hotel.Services.Identity.Core.Helpers;
using Hotel.Services.Identity.Core.Options;
using Hotel.Services.Identity.Core.Services;
using Hotel.Services.Identity.Core.Services.Interfaces;
using Hotel.Services.Identity.Database;
using Hotel.Services.Identity.Database.Repositories;
using Hotel.Services.Identity.Database.Repositories.Interfaces;
using Hotel.Services.Identity.RabbitMq;
using MediatR;

namespace Hotel.Services.Identity
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
            services.AddSingleton<IdentityDbContext>();

            services.AddSingleton<IIdentityErrorDescriber, IdentityErrorDescriber>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IUserSessionRepository, UserSessionRepository>();
            services.AddTransient<IJwtService, JwtService>();

            services.Configure<MongoDbOptions>(_ => Configuration.GetSection("Mongo").Bind(_));
            services.Configure<JwtOptions>(_ => Configuration.GetSection("Jwt").Bind(_));

            services.AddRabbitMq<ConsumeIdentityRabbitMqBackgroundService>(_ => Configuration.GetSection("RabbitMQ").Bind(_));

            services.AddMediatR(Assembly.GetExecutingAssembly());

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
