using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class CountryDivision
    {
        public Guid ID { get; set; }
        public Guid? TenantId { get; set; }

        public virtual ICollection<CountryDivisionDetail> CountryDivisionDetails { get; set; }
        public virtual ICollection<UnitDetail> UnitDetails { get; set; }
    }
}
