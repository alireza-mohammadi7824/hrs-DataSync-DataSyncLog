using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class Person
    {
        public Guid ID { get; set; }
        public Guid? TenantId { get; set; }
        public string WorkflowInstanceId  { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public string LastActionTitle { get; set; }
        public virtual ICollection<PersonDetail> PersonDetails { get; set; }
        public virtual ICollection<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual ICollection<PersonEducation> PersonEducations { get; set; }
        public virtual ICollection<PersonMilitaryStatus> PersonMilitaryStatuses { get; set; }
        public virtual ICollection<EmployeeDependentDetail> EmployeeDependentDetails { get; set; }
    }
}
