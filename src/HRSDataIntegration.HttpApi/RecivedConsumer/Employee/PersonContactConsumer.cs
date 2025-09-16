using HRSDataIntegration.DTOs;
using HRSDataIntegration.Interfaces;
using MassTransit;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.RecivedConsumer
{
    public class PersonContactConsumer : IConsumer<MessageDTO>
    {
        public class PersonContactConsumerDefinition : ConsumerDefinition<PersonContactConsumer>
        {
            public PersonContactConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<PersonContactConsumer> consumerConfigurator)
            {
                endpointConfigurator.ConfigureConsumeTopology = false;
                if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
                {

                    rmq.Bind(nameof(MessageDTO), x =>
                    {
                        x.RoutingKey = "TODORoutingKey";
                        x.ExchangeType = ExchangeType.Direct;
                    });
                }
            }
        }
        private readonly IPersonContactService _personContactService;
        public PersonContactConsumer(IPersonContactService personContactService)
        {
            _personContactService = personContactService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "PersonContact_Queue")
            {
                _personContactService.ConvertSqlInsertIntoPersoncontactToOracletable(ID);
            }
        }
    }
}
