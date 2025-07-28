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
    public class ChartConsumer : IConsumer<MessageDTO>
    {
        public class ChartConsumerDefinition : ConsumerDefinition<ChartConsumer>
        {
            public ChartConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<ChartConsumer> consumerConfigurator)
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
        private readonly IChartService _chartService;

        public ChartConsumer(IChartService chartService)
        {
            _chartService = chartService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_CHART_Queue")
            {
                _chartService.ConvertSqlChartTable_Insert_ToOracletable(ID);
            }
            if (TypeName == "Update_CHART_Queue")
            {
                _chartService.ConvertSqlChartTable_Update_ToOracletable(ID);
            }
        }
    }
}
