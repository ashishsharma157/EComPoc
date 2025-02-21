using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration Configuration, Assembly assembly=null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                if (assembly != null)
                {
                    config.AddConsumers(assembly);
                }

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(Configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(Configuration["MessageBroker:UserName"]!);
                        host.Password(Configuration["MessageBroker:Password"]!);
                    });
                    configurator.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
