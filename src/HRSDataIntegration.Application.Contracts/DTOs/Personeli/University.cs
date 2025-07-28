using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class University
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsActive { get; set; }
        public virtual UniversityType UniversityType { get; set; }
        public Guid UniversityTypeId { get; set; }
        public virtual CountryDivision CountryDivision { get; set; }
        public Guid CountryDivisionId { get; set; }
        public virtual ICollection<PersonEducation> PersonEducations { get; set; }
    }
}
