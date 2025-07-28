using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class PostLevel
    {
        public Guid ID { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }

        public Guid? TenantId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public bool IsActive { get; set; }
        public virtual ICollection<PostDetail> PostDetails { get; set; }
    }
}
