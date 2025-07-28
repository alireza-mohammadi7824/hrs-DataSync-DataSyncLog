using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class UnitDetail
    {
        public Guid ID { get; set; }
        public  Unit Unit { get; set; }
        public Guid UnitId { get; set; }

        public int Code { get; set; }

        public string Title { get; set; }

        public int? MehrGostarCode { get; set; }

        public int? CentralBankCode { get; set; }

        public Guid ParentId { get; set; }
        //public virtual Unit ParentUnit { get; set; }
        //public virtual Unit ParentUnit { get; set; }

        public virtual UnitLevel UnitLevel { get; set; }
        public Guid UnitLevelId { get; set; }

       // public virtual Province Province { get; set; }
        public Guid ProvinceId { get; set; }

       public virtual CountryDivision CountryDivision { get; set; }
        public Guid CountryDivisionId { get; set; }

        public string? Address { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }


        //inherit
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string ConcurrencyStamp { get; set; }

        public Guid? TenantId { get; set; }

        public int EffectiveDateFrom { get; set; }

        public int EffectiveDateTo { get; set; }

        public string WorkflowInstanceId { get; set; }

        public int StateCode { get; set; }

        public Guid LastActionUniqueId { get; set; }

        public Guid LastActivityUserId { get; set; }

        public DateTime LastActivityTime { get; set; }

        public string StateTitle { get; set; }

        public string LastActionTitle { get; set; }
    }
}
