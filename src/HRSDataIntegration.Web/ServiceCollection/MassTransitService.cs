using HRSDataIntegration.DTOs;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Linq;

namespace HRSDataIntegration.Web.ServiceCollection
{
    public static class MassTransitService
    {
        public static IServiceCollection AddServiceMassTransit(this IServiceCollection collection, IConfiguration configuration)
        {
            var configurations = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false)
               .Build();


            var HostName = configuration["EventBus:HostName"];
            var Port = configuration["EventBus:Port"];
            var UserName = configuration["EventBus:UserName"];
            var Password = configuration["EventBus:Password"];

            collection.AddMassTransit(config =>
            {
                var entryAssembly = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IConsumer).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(x => x.Assembly).ToArray();

                //var typeOfCunsumerModel = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                //.Where(x => x.GetCustomAttributes(typeof(EntityNameAttribute), true).Length > 0).ToList();


                config.AddConsumers(entryAssembly);

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("rabbitmq://" + HostName + ":" + Port + ""), h =>
                    {
                        h.Username(UserName);
                        h.Password(Password);
                    });

                    cfg.ConfigureMessageProjectTopologyodules();
                    cfg.ConfigureEndpoints(context);
                });
            });
            return collection;
        }

    }


    public static class ConfigureMessageTopologyodules
    {
        public static void ConfigureMessageProjectTopologyodules(this IRabbitMqBusFactoryConfigurator configurator)
        {

            configurator.Message<MessageDTO>(x => x.SetEntityName(nameof(MessageDTO)));
            configurator.Publish<MessageDTO>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;

            });
        }
    }
}
