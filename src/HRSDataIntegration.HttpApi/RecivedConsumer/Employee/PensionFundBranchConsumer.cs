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
    public class PensionFundBranchConsumer : IConsumer<MessageDTO>
    {
        public class PensionFundBranchConsumerDefinition : ConsumerDefinition<PensionFundBranchConsumer>
        {
            public PensionFundBranchConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<PensionFundBranchConsumer> consumerConfigurator)
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
        private readonly IPensionFundBranchService _pensionFoundBranchService;
        public PensionFundBranchConsumer(IPensionFundBranchService pensionFoundBranchService)
        {
            _pensionFoundBranchService = pensionFoundBranchService;
        }

        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_PensionFundBranch_Queue")
            {
                _pensionFoundBranchService.ConvertToPensionFundBranch_Insert_ToOracleTable(ID);
            }
            if (TypeName == "Update_PensionFundBranch_Queue")
            {
                _pensionFoundBranchService.ConvertToPensionFundBranch_Update_ToOracleTable(ID);
            }
            if (TypeName == "Delete_PensionFundBranch_Queue")
            {
                _pensionFoundBranchService.ConvertToPensionFundBranch_Delete_ToOracleTable(ID);
            }
        }
    }
}
