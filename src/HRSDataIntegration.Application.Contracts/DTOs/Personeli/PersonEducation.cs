using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class PersonEducation
    {
        public Guid Id { get; set; }
        public virtual Person Person { get; set; }
        public Guid PersonId { get; set; }

        public virtual DegreeLevel DegreeLevel { get; set; }

        public Guid DegreeLevelID { get; set; }

        public virtual DegreeType? DegreeType { get; set; }
        public Guid? DegreeTypeID { get; set; }
        public virtual University? University { get; set; }
        public Guid? UniversityID { get; set; }
        public virtual StudyField? StudyField { get; set; }
        public Guid? StudyFieldID { get; set; }
        public virtual StudyBranch? StudyBranch { get; set; }
        public Guid? StudyBranchlID { get; set; }
        public virtual NotRelatedType NotRelatedType { get; set; } 
        public Guid? NotRelatedTypeID { get; set; }
        public bool HasThesis { get; set; }
        public int? GraduationDate { get; set; }

        public decimal? DegreeScore { get; set; }

        public bool? IsApproved { get; set; }

        public string? Description { get; set; }

        public bool? IsApproveNotNeeded { get; set; }
        public string? ApproveLetterNumber { get; set; }

        public int? ApproveLetterDate { get; set; }

        public string? ReplyApproveLetterNumber { get; set; }

        public int? ReplyApproveLetterDate { get; set; }



        //From Interfaces
        public Guid? TenantId { get; set; }

        public string WorkflowInstanceId { get; set; }
        public int StateCode { get; set; }
        public Guid LastActionUniqueId { get; set; }
        public Guid LastActivityUserId { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string StateTitle { get; set; }
        public string LastActionTitle { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string ConcurrencyStamp { get; set; }
        public int EffectiveDate { get; set; }
    }
}
