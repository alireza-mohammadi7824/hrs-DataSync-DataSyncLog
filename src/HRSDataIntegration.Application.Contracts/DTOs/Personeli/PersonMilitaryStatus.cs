using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class PersonMilitaryStatus
    {
        public Guid Id { get; set; }
        public virtual Person Person { get; set; }
        public Guid PersonId { get; set; }
        public virtual MilitaryStatus MilitaryStatus { get; set; }
        public Guid MilitaryStatusId { get; set; }

        //public virtual MilitaryExemption MilitaryExemption { get; set; }
        //public Guid? MilitaryExemptionId { get; set; }

        //public virtual MilitaryRasteh MilitaryRasteh { get; set; }
        //public Guid? MilitaryRastehId { get; set; }

        //public virtual MilitaryHozeh MilitaryHozeh { get; set; }
        //public Guid? MilitaryHozehId { get; set; }

        public bool IsMilitaryStatusApprovedbyService { get; set; }
        public int IssueDate { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public int? MilitaryYear { get; set; }
        public int? MilitaryMonth { get; set; }
        public int? MilitaryDay { get; set; }
        public string? ApproveLetterNumber { get; set; }
        public int? ApproveLetterDate { get; set; }
        public string? ReplyApproveLetterNumber { get; set; }
        public int? ReplyApproveLetterDate { get; set; }
        public bool IsApproved { get; set; }
        public string? Description { get; set; }
        public Guid? TenantId { get; set; }
        public string WorkflowInstanceId { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public string LastActionTitle { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int EffectiveDate { get; set; }
    }
}
