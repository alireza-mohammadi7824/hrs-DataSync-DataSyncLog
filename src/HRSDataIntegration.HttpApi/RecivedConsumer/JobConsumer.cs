using HRSDataIntegration.DTOs;
using HRSDataIntegration.Interfaces;
using MassTransit;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace HRSDataIntegration.RecivedConsumer
{
    public class JobConsumer : IConsumer<MessageDTO>
    {


        public class JobConsumerDefinition : ConsumerDefinition<JobConsumer>
        {
            public JobConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<JobConsumer> consumerConfigurator)
            {
                endpointConfigurator.ConfigureConsumeTopology = false;
                if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
                {
                    rmq.Bind(nameof(MessageDTO), x =>
                    {
                        x.RoutingKey = "JobRoutingKey";
                        x.ExchangeType = ExchangeType.Direct;
                    });
                }
            }
        }

        private readonly IJobService _jobService;
        public JobConsumer(IJobService jobService)
        {
            _jobService = jobService;
        }



        public async Task Consume(ConsumeContext<MessageDTO> context)
        {

            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_Job_Queue")
            {
                _jobService.ConvertJobOverflow(ID.ToString());
            }
        }
    }
}
