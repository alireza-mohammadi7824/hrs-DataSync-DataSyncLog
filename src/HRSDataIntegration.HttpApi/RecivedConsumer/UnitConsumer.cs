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
    public class UnitConsumer : IConsumer<MessageDTO>
    {
        public class UnitConsumerDefinition : ConsumerDefinition<UnitConsumer>
        {
            public UnitConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<UnitConsumer> consumerConfigurator)
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
        private readonly IUnitService _unitService;
        public UnitConsumer(IUnitService unitService)
        {
            _unitService = unitService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_UNIT_Queue")
            {
                _unitService.ConvertSqlUnitTableConvertToOracleTBUNITtableWhenInsert(ID);
            }
            if (TypeName == "Update_UnitParentId_Queue")
            {
                _unitService.ConvertUpdateTBUNIT_PARENT_DETAIL(ID);
            }
            if (TypeName == "Update_UnitName_Queue")
            {
                _unitService.ConvertUpdateTBUNIT_Name(ID);
            }
            if (TypeName == "Update_UnitAddress_Queue")
            {
                _unitService.ConvertUpdateTBUNIT_Address(ID);
            }
            if (TypeName == "Update_UnitTels_Queue")
            {
                _unitService.ConvertUpdateTBUNIT_Tels(ID);
            }
            if(TypeName == "Insert_UnitDestroyEdgham_Queue")
            {
                _unitService.ConvertDestroy_Edgham_TBUNIT(ID);
            }
            if (TypeName == "Insert_UnitDestroyEnhelal_Queue")
            {
                _unitService.ConvertDestroy_Enhelal_TBUNIT(ID);
            }
        }
    }
}
