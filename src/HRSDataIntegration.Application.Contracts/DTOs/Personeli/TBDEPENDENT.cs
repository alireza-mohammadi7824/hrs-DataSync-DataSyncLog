using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBDEPENDENT
    {
        public string ID { get; set; }
        public string FROM_DATE { get; set; }
        public string EXIT_DATE { get; set; }
        public string EXIT_DESCRIPTION { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public long DOMAIN_CODE { get; set; }
        public long RECORD_ACTIVE { get; set; }
        public string FAMILY_ID { get; set; }
        public int STATE_CODE { get; set; }
        public string NON_DEPENDENT_REASON_ID { get; set; }
    }
}
