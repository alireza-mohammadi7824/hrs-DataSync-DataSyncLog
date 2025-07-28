using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class OrganizationChartNodeDiagram
    {
        public Guid Id { get; set; }
        public float xPos { get; set; }

        public float yPos { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public string FromPointIndex { get; set; }

        public string ToPointIndex { get; set; }

        public bool IsCreatedInDiagram { get; set; }

        public bool IsInGroupBy { get; set; }


        // Interfaces
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? TenantId { get; set; }

        public string ConcurrencyStamp { get; set; }


        // Relations
        public virtual OrganizationChartNodeDetail OrganizationChartNodeDetail { get; set; }
        public Guid OrganizationChartNodeDetailId { get; set; }

        public virtual ICollection<OrganizationChartNodeDiagramPointArray> OrganizationChartNodeDiagramPointArrays { get; set; }
    }
}
