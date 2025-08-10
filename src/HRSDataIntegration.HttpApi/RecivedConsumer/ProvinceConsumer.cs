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
    public class ProvinceConsumer  : IConsumer<MessageDTO>
    {
        public class ProvinceConsumerDefinition : ConsumerDefinition<ProvinceConsumer>
    {
        public ProvinceConsumerDefinition()
        {
            EndpointName = "message.queue";
        }

        [Obsolete]
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<ProvinceConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;
            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
            {

                rmq.Bind(nameof(MessageDTO), x =>
                {
                    x.RoutingKey = "ProvinceRoutingKey";
                    x.ExchangeType = ExchangeType.Direct;
                });
            }
        }
        }
        private readonly IProvinceService _provinceService;
        public ProvinceConsumer(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_PROVINCE_Queue")
            {
                _provinceService.InsertProvinceToOracle(ID);
            }
            if (TypeName == "Update_PROVINCE_Queue")
            {
                _provinceService.UpdateProvinceToOracleBy(ID);
            }
            if (TypeName == "Delete_PROVINCE_Queue")
            {
                _provinceService.RemoveProvinceToOracle(ID);
            }
        }
    }
}
