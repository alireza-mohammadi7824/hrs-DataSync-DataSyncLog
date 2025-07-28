using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class OrganizationChartNode
    {
        public Guid Id { get; set; }
        public int StateCode { get; set; }
        public string WorkflowInstanceId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public Guid? TenantId { get; set; }
        public bool IsActive { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public string LastActionTitle { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public virtual ICollection<OrganizationChartNodeDetail> OrganizationChartNodeDetails { get; set; } = new HashSet<OrganizationChartNodeDetail>();
    }
}
