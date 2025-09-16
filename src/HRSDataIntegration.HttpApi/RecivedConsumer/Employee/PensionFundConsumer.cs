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
    public class PensionFundConsumer : IConsumer<MessageDTO>
    {
        public class PensionFundConsumerDefinition : ConsumerDefinition<PensionFundConsumer>
        {
            public PensionFundConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<PensionFundConsumer> consumerConfigurator)
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
        private readonly IPensionFundService _pensionFundService;
        public PensionFundConsumer(IPensionFundService pensionFundService)
        {
            _pensionFundService = pensionFundService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_PensionFund_Queue")
            {
                _pensionFundService.ConvertToPensionFund_Insert_ToOracleTable(ID);
            }
            if (TypeName == "Update_PensionFund_Queue")
            {
                _pensionFundService.ConvertToPensionFund_Update_ToOracleTable(ID);
            }
            if (TypeName == "Delete_PensionFund_Queue")
            {
                _pensionFundService.ConvertToPensionFund_Delete_ToOracleTable(ID);
            }
        }
    }
}
