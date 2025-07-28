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
    public class PersonDetailConsumer : IConsumer<MessageDTO>
    {
        public class PersonDetailConsumerDefinition : ConsumerDefinition<PersonDetailConsumer>
        {
            public PersonDetailConsumerDefinition()
            {
                EndpointName = "message.queue";
            }
            [Obsolete]
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                IConsumerConfigurator<PersonDetailConsumer> consumerConfigurator)
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
        private readonly IPersonDetailService _personDetailService;
        public PersonDetailConsumer(IPersonDetailService personDetailService)
        {
            _personDetailService= personDetailService;
        }
        public async Task Consume(ConsumeContext<MessageDTO> context)
        {
            var TypeName = context.Message.Type;
            var ID = context.Message.Id;
            if (TypeName == "Insert_PersonDetail_Queue")
            {
                _personDetailService.ConvertToPersonDetail_Insert_ToOracleTable(ID);
            }
            if (TypeName == "DivorceRegistrationProcessForPersonel_Queue")
            {
                _personDetailService.ConvertDivorceRegistrationProcessForPersonelToOracleTable(ID);
            }
            if (TypeName == "PersonelMarriageProcess_Queue")
            {
                _personDetailService.ConvertPersonelMarriageProcessToOracleTable(ID);
            }
            if (TypeName == "ProcessOfRegisteringDeathOfPersonel_Queue")
            {
                _personDetailService.ConvertProcessOfRegisteringDeathFamilyOfPersonelToOracleTable(ID);
            }
            if (TypeName == "ProcessOfRegistrationBirthOfEmployeesChildrean_Queue")
            {
                _personDetailService.ConvertProcessOfRegistrationBirthOfEmployeesChildrean(ID);
            }
            if (TypeName == "ProcessOfRegistrationMarriageOfEmployeesChildrean_Queue")
            {
                _personDetailService.ConvertProcessOfRegistrationMarriageOfEmployeesChildrean(ID);
            }
        }
    }
}
