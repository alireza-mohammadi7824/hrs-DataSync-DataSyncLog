using MassTransit;

namespace HRSDataIntegration.DTOs
{
    [EntityName(nameof(MessageDTO))]
    public class MessageDTO
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
}
