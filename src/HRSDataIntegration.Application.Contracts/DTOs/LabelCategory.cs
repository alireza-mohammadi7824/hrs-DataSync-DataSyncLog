using System;

namespace HRSDataIntegration.DTOs
{
    public class LabelCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
