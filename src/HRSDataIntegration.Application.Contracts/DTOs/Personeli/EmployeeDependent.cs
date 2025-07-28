using System.Collections.Generic;
using System;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class EmployeeDependent
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public virtual ICollection<EmployeeDependentDetail> EmployeeDependentDetails { get; set; }
        public virtual ICollection<EmployeeMaritalDetail> EmployeeMaritalDetails { get; set; }
    }
}