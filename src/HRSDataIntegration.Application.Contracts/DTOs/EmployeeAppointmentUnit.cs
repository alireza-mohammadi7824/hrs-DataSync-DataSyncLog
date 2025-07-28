using HRSDataIntegration.DTOs.Personeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class EmployeeAppointmentUnit
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public string ConcurrencyStamp { get; set; }

        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }

        public int StateCode { get; set; }

        public Guid EmployeeAppointmentId { get; set; }
      //  public virtual EmployeeAppointment EmployeeAppointment { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
