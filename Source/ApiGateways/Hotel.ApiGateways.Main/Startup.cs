using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignIn;
using Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignUp;
using Hotel.ApiGateways.Main.Extensions;
using Hotel.Common.Features.Identity.Commands.ValidateToken;
using Hotel.Common.Models;
using Hotel.Common.RabbitMq;
using Hotel.Common.RabbitMq.Manager;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.ApiGateways.Main
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
            services.AddRabbitMq(_ => Configuration.GetSection("RabbitMQ").Bind(_));

            services.AddMediatR(typeof(Startup));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // -> next three lines: avoid to check de signature key (just the auth app, the issuer, has the key)
                    ValidateIssuerSigningKey = false,
                    RequireSignedTokens = false,
                    SignatureValidator = (token, _) => new JwtSecurityToken(token),

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateActor = false,
                    ValidateLifetime = true  // -> even avoiding the signature check, the token expiration has to be checked
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        await Task.Run(() =>
                        {
                            var accessToken = context.HttpContext.GetBearerToken();
                            var rabbitMq = context.HttpContext.RequestServices.GetRequiredService<IRabbitMqManager>();

                            var validate = rabbitMq.Publish<ValidateTokenCommand, Result<ValidateTokenResult>>(new ValidateTokenCommand(accessToken));

                            if (!validate.IsSucceeded)
                            {
                                context.Fail("Please log in to your account");
                                return;
                            }

                            context.Success();
                        });
                    }
                };
            });

            services.AddControllers()
                .AddFluentValidation(_ => _.RegisterValidatorsFromAssemblyContaining<SignUpInputValidator>());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel.ApiGateways.Main", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel.ApiGateways.Main v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
