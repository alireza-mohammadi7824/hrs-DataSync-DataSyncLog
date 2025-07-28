using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class Province
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public Guid TenantId { get; set; }
    }
}
