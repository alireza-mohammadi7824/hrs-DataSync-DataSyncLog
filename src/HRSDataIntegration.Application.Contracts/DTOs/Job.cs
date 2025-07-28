using System;
using System.Collections.Generic;

namespace HRSDataIntegration.DTOs
{
    public class Job
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<JobDetail> JobDetails { get; set; }
        //public virtual ICollection<JobHRBaseRule> JobHRBaseRules { get; set; }
        //public virtual ICollection<JobDuty> JobDuties { get; set; }
        //public virtual ICollection<PostJob> PostJobs { get; set; }
    }
}
