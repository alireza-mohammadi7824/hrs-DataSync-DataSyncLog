using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class DataSyncLog
    {
            public Guid Id { get; set; }
            public Guid MessageUniqueId { get; set; }
            public Guid RecordId { get; set; }
            public string Type { get; set; }
            public bool? IsDone { get; set; }
            public string? ExceptionMessage { get; set; }
            public DateTime CreationTime { get; set; }
            public Guid? CreatorId { get; set; }
            public Guid? LastModifierId { get; set; }
            public string? LastModificationTime { get; set; }
            public Guid? DeleterId { get; set; }
            public DateTime? DeletionTime { get; set; }
            public bool IsDeleted { get; set; }        
    }
}
