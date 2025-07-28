using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class EmployeePensionFund
    {
        public Guid Id { get; set; }
        public virtual Employee Employee { get; set; }
        public Guid EmployeeId { get; set; }

        public virtual PensionFundBranch PensionFundBranch { get; set; }
        public Guid? PensionFundBranchId { get; set; }

      //  public virtual PensionFund.PensionFund PensionFund { get; set; }
        public Guid PensionFundId { get; set; }

        public string? MembershipCode { get; set; }

        public string? Description { get; set; }


        //From Interfaces
        public Guid? TenantId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
