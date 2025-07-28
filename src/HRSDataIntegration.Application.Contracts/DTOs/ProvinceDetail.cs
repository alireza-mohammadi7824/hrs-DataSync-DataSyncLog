using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class ProvinceDetail
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public Guid CountryDivisionId { get; set; }
        public Guid TenantId { get; set; }
        public Guid ConcurrencyStamp { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatorId { get; set; }
        public Guid LastModifierId { get; set; }
        public DateTime LastModificationTime { get; set; }
        public int EffectiveDateFrom { get; set; }
        public int EffectiveDateTo { get; set; }
    }
}
