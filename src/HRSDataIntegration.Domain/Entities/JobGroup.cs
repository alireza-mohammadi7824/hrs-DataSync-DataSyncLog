using System;
using System.Collections.Generic;

namespace HRSDataIntegration.Entities
{
    public class JobGroup
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public Guid? TenantId { get; set; }
        public string ConcurrencyStamp { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public bool IsActive { get; set; }
        public virtual ICollection<JobDetail> JobDetails { get; set; }
    }
}
