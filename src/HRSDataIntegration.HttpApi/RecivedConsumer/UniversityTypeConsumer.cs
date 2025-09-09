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

    public class UniversityTypeConsumer : IConsumer<MessageDTO>
    {
        public class UniversityTypeConsumerDefinition : ConsumerDefinition<UniversityTypeConsumer>
        {
            public UniversityTypeConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<UniversityTypeConsumer> consumerConfigurator)
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
        private readonly IUniversityTypeService _universityTypeService;
        public UniversityTypeConsumer(IUniversityTypeService universityTypeService)
        {
            _universityTypeService = universityTypeService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_UniversityType_Queue")
            {
                _universityTypeService.ConvertToUniversityType_Insert_ToOracleTable(ID);
            }
            if (TypeName == "Update_UniversityType_Queue")
            {
                _universityTypeService.ConvertToUniversityType_Update_ToOracleTable(ID);
            }
            if (TypeName == "Delete_UniversityType_Queue")
            {
                _universityTypeService.ConvertToUniversityType_Delete_ToOracleTable(ID);
            }
        }
    }
}
