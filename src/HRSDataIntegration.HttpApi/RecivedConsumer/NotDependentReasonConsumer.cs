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
    public class NotDependentReasonConsumer : IConsumer<MessageDTO>
    {
        public class NotDependentReasonConsumerDefinition : ConsumerDefinition<NotDependentReasonConsumer>
        {
            public NotDependentReasonConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<NotDependentReasonConsumer> consumerConfigurator)
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
        private readonly INotDependentReasonService _notDependentReasonService;
        public NotDependentReasonConsumer(INotDependentReasonService notDependentReasonService)
        {
            _notDependentReasonService = notDependentReasonService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_NotDependentReason_Queue")
            {
                _notDependentReasonService.ConvertToNotDependentReasonService_Insert_ToOracleTable(ID);
            }
            if (TypeName == "Update_NotDependentReason_Queue")
            {
                _notDependentReasonService.ConvertToNotDependentReasonService_Update_ToOracleTable(ID);
            }
            if (TypeName == "Delete_NotDependentReason_Queue")
            {
                _notDependentReasonService.ConvertToNotDependentReasonService_Delete_ToOracleTable(ID);
            }
        }
    }
}
