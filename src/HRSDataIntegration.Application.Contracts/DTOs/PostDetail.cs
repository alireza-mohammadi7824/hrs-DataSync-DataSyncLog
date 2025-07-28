using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class PostDetail
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public virtual PostType PostType { get; set; }
        public Guid? PostTypeId { get; set; }

        //PostLevel Relation
        public virtual PostLevel PostLevel { get; set; }
        public Guid PostLevelId { get; set; }

        //PostManagingLevel Relation
        public virtual PostManagingLevel PostManagingLevel { get; set; }
        public Guid? PostManagingLevelId { get; set; }

        //Post Relation
        public virtual Post Post { get; set; }
        public Guid PostId { get; set; }


        public string ConcurrencyStamp { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }

        public Guid? TenantId { get; set; }

        public string WorkflowInstanceId { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public string LastActionTitle { get; set; }

        public DateTime CreationTime { get; set; }

    }
}
