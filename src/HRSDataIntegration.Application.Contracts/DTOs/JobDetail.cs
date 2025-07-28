using System;
using Volo.Abp.Domain.Entities;

namespace HRSDataIntegration.DTOs
{
    public class JobDetail:Entity<Guid>
    {
        //public Guid Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public Guid? TenantId { get; set; }

        public Guid JobId { get; set; }
        public Job Job { get; set; }

        public Guid JobRastehId { get; set; }
        public JobRasteh JobRasteh { get; set; }

        public Guid JobGroupId { get; set; }
        public JobGroup JobGroup { get; set; }

        public string ConcurrencyStamp { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string WorkflowInstanceId { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public string LastActionTitle { get; set; }

        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
    }
}
