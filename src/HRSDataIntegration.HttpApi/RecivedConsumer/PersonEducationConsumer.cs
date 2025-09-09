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
    public class PersonEducationConsumer : IConsumer<MessageDTO>
    {
        public class PersonEducationConsumerDefinition : ConsumerDefinition<PersonEducationConsumer>
        {
            public PersonEducationConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<PersonEducationConsumer> consumerConfigurator)
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
        private readonly IPersonEducationService _personEducationService;
        public PersonEducationConsumer(IPersonEducationService personEducationService)
        {
            _personEducationService = personEducationService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_PersonEducation_Queue")
            {
                _personEducationService.ConvertToPersonEducationService_Insert_ToOracleTable(ID);
            }
           
        }
    }
}
