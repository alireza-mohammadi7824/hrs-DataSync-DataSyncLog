using System;
using Volo.Abp.Domain.Entities;

namespace HRSDataIntegration.DTOs
{
    public class MappingId : Entity<Guid>
    {
      public Guid Id { get; set; }
        public DateTime ConvertDateTime { get; set; }
        public byte SubsystemCode { get; set; }
        public string OldTableName { get; set; }
        public string OldColumnName { get; set; }
        public string NewTableName { get; set; }
        public string NewColumnName { get; set; }
        public string OldColumnValue { get; set; }
        public string NewColumnValue { get; set; }
    }
}
