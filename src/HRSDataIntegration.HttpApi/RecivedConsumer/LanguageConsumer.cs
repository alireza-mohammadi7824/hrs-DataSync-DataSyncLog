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
    public class LanguageConsumer : IConsumer<MessageDTO>
    {
        public class LanguageConsumerDefinition : ConsumerDefinition<LanguageConsumer>
        {
            public LanguageConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<LanguageConsumer> consumerConfigurator)
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
        private readonly ILanguageService _languageService;
        public LanguageConsumer(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_Language_Queue")
            {
                _languageService.ConvertToLanguage_Insert_ToOracleTable(ID);
            }
            if (TypeName == "Update_Language_Queue")
            {
                _languageService.ConvertToLanguage_Update_ToOracleTable(ID);
            }
            if (TypeName == "Delete_Language_Queue")
            {
                _languageService.ConvertToLanguage_Delete_ToOracleTable(ID);
            }
        }
    }
}
