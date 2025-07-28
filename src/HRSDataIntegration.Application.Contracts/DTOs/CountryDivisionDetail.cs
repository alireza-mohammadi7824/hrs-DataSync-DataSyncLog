using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class CountryDivisionDetail
    {
        public Guid ID { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string? CountryDivisionRoute { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
        public Guid? TenantId { get; set; }
        public Guid CountryDivisionTypeId { get; set; }
        public virtual CountryDivisionType CountryDivisionType { get; set; }
        public Guid CountryDivisionId { get; set; }
        public virtual CountryDivision CountryDivision { get; set; }
        public Guid? CountryDivisionDetailParentId { get; set; }
        public Guid? ParentId { get; set; }
        //public virtual ICollection<CountryDivisionDetail> ChildrenCountryDivisionDetail { get; set; }
        //public virtual CountryDivisionDetail ParentCountryDivisionDetail { get; set; }


        //public virtual HRSUser HRSUser { get; set; }

        public DateTime CreationTime { get; set; }

        [ForeignKey("HRSUser")]
        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
