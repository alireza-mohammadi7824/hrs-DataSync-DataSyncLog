using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class Employee
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        public Guid BranchId { get; set; }
        public virtual ICollection<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual ICollection<EmployeeDependentDetail> EmployeeDependentDetails { get; set; }
        public virtual ICollection<EmployeeAppointmentUnit> EmployeeAppointmentUnits { get; set; }
    }
}
