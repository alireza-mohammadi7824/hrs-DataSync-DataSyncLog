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
    public class UnitLevelConsumer : IConsumer<MessageDTO>
    {
        public class UnitLevelConsumerDefinition : ConsumerDefinition<UnitLevelConsumer>
        {
            public UnitLevelConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<UnitLevelConsumer> consumerConfigurator)
            {
                endpointConfigurator.ConfigureConsumeTopology = false;
                if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
                {

                    rmq.Bind(nameof(MessageDTO), x =>
                    {
                        x.RoutingKey = "UnitLevelRoutingKey";
                        x.ExchangeType = ExchangeType.Direct;
                    });
                }
            }
        }
        private readonly IUnitLevelService _unitLevelService;
        public UnitLevelConsumer(IUnitLevelService unitLevelService)
        {
          _unitLevelService = unitLevelService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_UnitLevel_Queue")
            {
                _unitLevelService.ConvertSqlUnitLevelTable_Insert_ToOracletable(ID);
            }
            if (TypeName == "Update_UnitLevel_Queue")
            {
                _unitLevelService.ConvertSqlUnitLevelTable_Update_ToOracletable(ID);
            }
            if (TypeName == "Delete_UnitLevel_Queue")
            {
                 _unitLevelService.ConvertSqlUnitLevelTable_Delete_ToOracletable(ID);
            }
        }
    }
}
