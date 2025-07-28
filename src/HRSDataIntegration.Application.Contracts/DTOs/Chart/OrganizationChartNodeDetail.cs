using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class OrganizationChartNodeDetail
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? TenantId { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
        public string ConcurrencyStamp { get; set; }
        public virtual OrganizationChartNode OrganizationChartNode { get; set; }
        public Guid OrganizationChartNodeId { get; set; }
        public Guid OrganizationChartNodeTypeId { get; set; }
        public OrganizationChart OrganizationChart { get; set; }
        public Guid OrganizationChartId { get; set; }
        public virtual Unit Unit { get; set; }
        public Guid? UnitId { get; set; }
        public Guid? PositionId { get; set; }
        public virtual Post Post { get; set; }
        public Guid? PostId { get; set; }
        public Guid OrganizationChartNodeStatusId { get; set; }
        public Guid OrganizationChartNodeRelationTypeId { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public string? Radif { get; set; }
        public virtual OrganizationChartNodeDetail Parent { get; set; }
        public Guid ParentId { get; set; }
        [NotMapped]
        public SqlHierarchyId ParentPath { get; set; }
        public string LastActionTitle { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public Guid LastActivityUserId { get; set; }
        public int StateCode { get; set; }
        public string StateTitle { get; set; }
        public string WorkflowInstanceId { get; set; }
        public virtual ICollection<OrganizationChartNodeDiagram> OrganizationChartNodeDiagrams { get; set; } = new HashSet<OrganizationChartNodeDiagram>();
    }
}

