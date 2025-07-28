using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class Unit
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public Guid? TenantId { get; set; }
        public virtual ICollection<UnitTel> UnitTels { get; set; }
        public virtual ICollection<UnitDetail> UnitDetails { get; set; }
    }
}
