using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class EmployeeMaritalDetail
    {
        public Guid Id { get; set; }
        public Guid EmployeeDependentId { get; set; }
        public bool IsApprovedByService { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Description { get; set; }
        public bool IsMariage { get; set; }
        public bool IsActive { get; set; }
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

        public virtual EmployeeDependent EmployeeDependent { get; set; }
    }
}
