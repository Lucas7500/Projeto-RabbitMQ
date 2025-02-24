﻿using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoRabbitMQ.Infrastructure;
using Serilog;
using System.Text;

namespace ProjetoRabbitMQ.Extensions
{
    public static class AppExtensions
    {
        public static void AddRabbitMQService(this IHostApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri(builder.Configuration["RABBITMQ_URI"]!), host =>
                    {
                        host.Username(builder.Configuration["RABBITMQ_USERNAME"]!);
                        host.Password(builder.Configuration["RABBITMQ_PASSWORD"]!);
                    });

                    config.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddDatabaseContext(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MySqlContext>(opt =>
            {
                var connectionString = builder.Configuration["MY_SQL_CONNECTION_STRING"];
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }

        public static void AddJwtConfiguration(this IHostApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT_KEY"]!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
        }

        public static void AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
        }

        public static void AddSerilogConfiguration(this ConfigureHostBuilder config)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            config.UseSerilog();
        }
        
        public static void EnsureDatabaseCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MySqlContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}
