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
    public class PostConsumer : IConsumer<MessageDTO>
    {
        public class PostConsumerDefinition : ConsumerDefinition<PostConsumer>
        {
            public PostConsumerDefinition()
            {
                EndpointName = "message.queue";
            }

            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<PostConsumer> consumerConfigurator)
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
        private readonly IPostService _postService;
        public PostConsumer(IPostService postService)
        {
            _postService = postService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_POST_Queue")
            {
                _postService.ConvertJobPostConvertToOracle(ID);
            }
            if (TypeName == "Update_POST_JOB_Queue")
            {
                _postService.UpdatePostJob(ID);
            }
        }
    }
}
