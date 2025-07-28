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
    public class StudyFieldConsumer : IConsumer<MessageDTO>
    {
        public class StudyFieldConsumerDefinition : ConsumerDefinition<StudyFieldConsumer>
        {
            public StudyFieldConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<StudyFieldConsumer> consumerConfigurator)
            {
                endpointConfigurator.ConfigureConsumeTopology = false;
                if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
                {

                    rmq.Bind(nameof(MessageDTO), x =>
                    {
                        x.ExchangeType = ExchangeType.Direct;
                    });
                }
            }
        }

        private readonly IStudyFieldService _educationService;
        public StudyFieldConsumer(IStudyFieldService educationService)
        {
            _educationService = educationService;
        }

        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_StudyField_Queue")
            {
                _educationService.ConvertToStudyField_Insert_ToOracleTable(ID);
            }
            if (TypeName == "Update_StudyField_Queue")
            {
                _educationService.ConvertToStudyField_Update_ToOracleTable(ID);
            }
            if (TypeName == "Delete_StudyField_Queue")
            {
                _educationService.ConvertToStudyField_Delete_ToOracleTable(ID);
            }
        }
    }
}
