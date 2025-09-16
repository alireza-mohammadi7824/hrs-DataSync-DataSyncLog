using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class OrganizationChartLimitation
    {
        public  Guid Id { get ; set; }
        //public int ProcessType { get; set; }
        public int Quantity { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? TenantId { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string WorkflowInstanceId { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public string LastActionTitle { get; set; }


        //Relations
        public virtual OrganizationChart OrganizationChart { get; set; }
        public Guid OrganizationChartId { get; set; }

        public virtual Post Post { get; set; }
        public Guid? PostId { get; set; }

        public virtual Unit Unit { get; set; }
        public Guid UnitId { get; set; }


    }
}
