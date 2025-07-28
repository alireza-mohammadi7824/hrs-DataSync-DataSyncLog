using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class UnitType
    {
        public Guid Id { get; set; }
        public int Code { get; set; }

        public string Title { get; set; }


        //inherit
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? TenantId { get; set; }

        public bool IsActive { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
