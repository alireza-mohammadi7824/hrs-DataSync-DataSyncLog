using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class OrganizationChartNodeDiagramPointArray
    {
        public Guid Id { get; set; }
        public float X { get; set; }

        public float Y { get; set; }


        // Interfaces
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? TenantId { get; set; }

        public string ConcurrencyStamp { get; set; }


        // Relations
        public virtual OrganizationChartNodeDiagram OrganizationChartNodeDiagram { get; set; }
        public Guid OrganizationChartNodeDiagramId { get; set; }
    }
}
