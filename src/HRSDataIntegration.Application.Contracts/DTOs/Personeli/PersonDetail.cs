using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class PersonDetail
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public bool? IsApprovedByService { get; set; }
        public string NamePersian { get; set; }
        public string FamilyPersian { get; set; }
        public string? NameLatin { get; set; }
        public string? FamilyLatin { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? MotherFamily { get; set; }
        public string? IdentityNo { get; set; }
        public string? IdentitySer { get; set; }
        public string? IdentitySerial { get; set; }
        public string? IdentityDescription { get; set; }
        public string NationalCode { get; set; }
        public int? BirthDate { get; set; }
        public int? IdentityDate { get; set; }
        public Guid? BirthCountryDivisionId { get; set; }
        public Guid? IdentityCountryDivisionId { get; set; }
        public Guid? GenderId { get; set; }
        public Guid? ReligionId { get; set; }
        public Guid? ReligionBranchId { get; set; }
        public Guid? NationalityTypeId { get; set; }
        public bool IsAlive { get; set; }
        public string? Description { get; set; }
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
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
        public virtual Person Person { get; set; }
    }
}
