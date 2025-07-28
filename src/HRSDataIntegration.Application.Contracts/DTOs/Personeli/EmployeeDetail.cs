using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class EmployeeDetail
    {
        public Guid Id { get; set; }
        public virtual Employee Employee { get; set; }
        public Guid? EmployeeId { get; set; }
        public virtual Person Person { get; set; }
        public Guid? PersonId { get; set; }
        public int? EmployeeNumber { get; set; }
        public string? GovernorNumber { get; set; }
        public Guid? EntranceTypeId { get; set; }
        public string? Description { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
        public string WorkflowInstanceId { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public string LastActionTitle { get; set; }
    }
}
