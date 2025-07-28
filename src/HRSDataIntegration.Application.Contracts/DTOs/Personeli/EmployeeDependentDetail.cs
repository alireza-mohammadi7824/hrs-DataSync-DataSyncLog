using System;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class EmployeeDependentDetail
    {
        public Guid Id { get; set; }
        public Guid EmployeeDependentId { get; set; }
        public Guid EmployeeId { get; set; }
        public string? EmployeeDependentNumber { get; set; }
        public Guid PersonId { get; set; }
        public Guid DependentRelationTypeId { get; set; }
        public Guid? EmployeeDependentStatusId { get; set; }
        public string? Description { get; set; }
        public Guid? TenantId { get; set; }
        public string WorkflowInstanceId { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string? StateTitle { get; set; }
        public string LastActionTitle { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual EmployeeDependent EmployeeDependent { get; set; }
        public virtual Person Person { get; set; }
        public virtual EmployeeDependentStatus EmployeeDependentStatus { get; set; }
    }
}