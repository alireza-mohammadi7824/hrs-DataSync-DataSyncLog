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
    public class NotFamilyRightReasonConsumer : IConsumer<MessageDTO>
    {
        public class NotFamilyRightReasonConsumerDefinition : ConsumerDefinition<NotFamilyRightReasonConsumer>
        {
            public NotFamilyRightReasonConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<NotFamilyRightReasonConsumer> consumerConfigurator)
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
        private readonly INotFamilyRightReasonService _notFamilyRightReasonService;
        public NotFamilyRightReasonConsumer(INotFamilyRightReasonService notFamilyRightReasonService)
        {
            _notFamilyRightReasonService = notFamilyRightReasonService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_NotFamilyRightReason_Queue")
            {
                _notFamilyRightReasonService.ConvertNotFamilyRightReasonInsertToOracleTable(ID);
            }
            if (TypeName == "Update_NotFamilyRightReason_Queue")
            {
                _notFamilyRightReasonService.ConvertNotFamilyRightReasonUpdateToOracleTable(ID);
            }
            if (TypeName == "Delete_NotFamilyRightReason_Queue")
            {
                _notFamilyRightReasonService.ConvertNotFamilyRightReasonDeleteToOracleTable(ID);
            }
        }
    }
}
