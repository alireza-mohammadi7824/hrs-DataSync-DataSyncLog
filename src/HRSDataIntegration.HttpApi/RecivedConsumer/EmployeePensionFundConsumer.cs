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
    public class EmployeePensionFundConsumer : IConsumer<MessageDTO>
    {
        public class EmployeePensionFundConsumerDefinition : ConsumerDefinition<EmployeePensionFundConsumer>
        {
            public EmployeePensionFundConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<EmployeePensionFundConsumer> consumerConfigurator)
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
        private readonly IEmployeePensionFundService _employeePensionFundService;
        public EmployeePensionFundConsumer(IEmployeePensionFundService employeePensionFundService)
        {
            _employeePensionFundService = employeePensionFundService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_EmployeePensionFund_Queue")
            {
                _employeePensionFundService.ConvertToEmployeePensionFund_Insert_ToOracleTable(ID);
            }
            else if (TypeName == "Update_EmployeePensionFund_Queue")
            {
                _employeePensionFundService.ConvertToEmployeePensionFund_Update_ToOracleTable(ID);
            }
            else if(TypeName == "Delete_EmployeePensionFund_Queue")
            {
                _employeePensionFundService.ConvertToEmployeePensionFund_Delete_ToOracleTable(ID);
            }
        }
    }
}
