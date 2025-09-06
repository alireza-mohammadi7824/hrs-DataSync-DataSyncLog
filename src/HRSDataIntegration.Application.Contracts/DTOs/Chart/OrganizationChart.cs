using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class OrganizationChart
    {
        public Guid Id { get; set; }
        public int EffectiveDate { get; set; }
        public string? Description { get; set; }
        public int ApproveDate { get; set; }
        public string ApproveNumber { get; set; }
        public Guid OrganizationChartApproverId { get; set; }
        public Guid? TenantId { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public string LastActionTitle { get; set; }
        public DateTime LastActivityTime { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
        public int StateCode { get; set; }
        public string StateTitle { get; set; }
        public string WorkflowInstanceId { get; set; }
        public virtual ICollection<OrganizationChartNodeDetail> OrganizationChartNodeDetails { get; set; }
        public virtual ICollection<OrganizationChartLimitation> OrganizationChartLimitations { get; set; }
    }
}
