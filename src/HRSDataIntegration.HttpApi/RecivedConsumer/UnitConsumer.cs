using HRSDataIntegration.DTOs;
using HRSDataIntegration.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
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
                        x.RoutingKey = "UnitRoutingKey";
                        x.ExchangeType = ExchangeType.Direct;
                    });
                }
            }
        }
        private readonly IUnitService _unitService;
        private readonly ILogger<UnitConsumer> _logger;
        public UnitConsumer(IUnitService unitService, ILogger<UnitConsumer> logger)
        {
            _unitService = unitService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            try
            {
                var TypeName = context.Message.Type;
                var ID = context.Message.Id;
                if (TypeName == "Insert_UNIT_Queue")
                {
                    _unitService.ConvertSqlUnitTableConvertToOracleTBUNITtableWhenInsert(ID);
                }
                else if (TypeName == "Update_UnitParentId_Queue")
                {
                    _unitService.ConvertUpdateTBUNIT_PARENT_DETAIL(ID);
                }
                else if (TypeName == "Update_UnitName_Queue")
                {
                    _unitService.ConvertUpdateTBUNIT_Name(ID);
                }
                else if (TypeName == "Update_UnitAddress_Queue")
                {
                    _unitService.ConvertUpdateTBUNIT_Address(ID);
                }
                else if (TypeName == "Update_UnitTels_Queue")
                {
                    _unitService.ConvertUpdateTBUNIT_Tels(ID);
                }
                else if (TypeName == "Insert_UnitDestroyEdgham_Queue")
                {
                    _unitService.ConvertDestroy_Edgham_TBUNIT(ID);
                }
                else if (TypeName == "Insert_UnitDestroyEnhelal_Queue")
                {
                    _unitService.ConvertDestroy_Enhelal_TBUNIT(ID);
                }
                else
                {
                    _logger.LogCritical($"HRSLogger: Unit Consume Error --> TypeName: {context.Message.Type} , ID: {context.Message.Id} ");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"HRSLogger: Unit Consume Error --> Exception message : {ex.Message}" +
                      $"\n Exception: {ex}");
            }
        }
    }
}
