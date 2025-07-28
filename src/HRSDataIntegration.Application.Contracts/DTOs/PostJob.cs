using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class PostJob
    {
        public Guid Id { get; set; }
        public virtual Post Post { get; set; }
        public Guid PostId { get; set; }

        public virtual Job Job { get; set; }
        public Guid JobId { get; set; }

        public Guid? TenantId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
    }
}
