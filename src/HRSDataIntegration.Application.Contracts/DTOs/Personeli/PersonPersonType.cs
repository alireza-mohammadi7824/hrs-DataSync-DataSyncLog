using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class PersonPersonType
    {
        public Guid Id { get; set; }
        public virtual Person Person { get; set; }
        public Guid PersonId { get; set; }
        public virtual PersonType PersonType { get; set; }
        public Guid PersonTypeId { get; set; }
        public Guid? TenantId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string ConcurrencyStamp { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
    }
}
