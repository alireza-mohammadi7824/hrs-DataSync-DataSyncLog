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
    public class EmployeeDependentStatusConsumer : IConsumer<MessageDTO>
    {
        public class EmployeeDependentStatusConsumerDefinition : ConsumerDefinition<EmployeeDependentStatusConsumer>
        {
            public EmployeeDependentStatusConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<EmployeeDependentStatusConsumer> consumerConfigurator)
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

        private readonly IEmployeeDependentStatusService _employeeDependentStatusService;
        public EmployeeDependentStatusConsumer(IEmployeeDependentStatusService employeeDependentStatusService)
        {
            _employeeDependentStatusService = employeeDependentStatusService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_EmployeeDependentStatus_Queue")
            {
                _employeeDependentStatusService.InsertEmployeeDependentStatusServiceFromSqlToOracle(ID);
            }
            else if (TypeName == "Update_EmployeeDependentStatus_Queue")
            {
                _employeeDependentStatusService.UpdateEmployeeDependentStatusServiceFromSqlToOracle(ID);
            }
            else if (TypeName == "Delete_EmployeeDependentStatus_Queue")
            {
                _employeeDependentStatusService.DeleteEmployeeDependentStatusServiceFromSqlToOracle(ID);
            }
        }
    }
}
