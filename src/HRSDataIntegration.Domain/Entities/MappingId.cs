using System;

namespace HRSDataIntegration.Entities
{
    public class MappingId
    {
      public Guid Id { get; set; }
        public DateTime ConvertDateTime { get; set; }
        public int SubsystemCode { get; set; }
        public string OldTableName { get; set; }
        public string OldColumnName { get; set; }
        public string NewTableName { get; set; }
        public string NewColumnName { get; set; }
        public Guid OldColumnValue { get; set; }
        public Guid NewColumnValue { get; set; }
    }
}
