using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class UnitTel
    {
        public  Guid Id { get; set; }
        public virtual Unit Unit { get; set; }
        public Guid UnitId { get; set; }

        public string? PreCode { get; set; }

        public string? Number { get; set; }

        public virtual UnitTellType UnitTelType { get; set; }
        public Guid UnitTelTypeId { get; set; }

        public virtual PostLevel PostLevel { get; set; }
        public Guid PostLevelId { get; set; }

        public string? Description { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? TenantId { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
