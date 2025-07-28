using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBFAMILY_MOBILE
    {
        public string ID { get; set; }
        public string FAMILY_ID { get; set; }
        public int MOBILE_NUMBER { get; set; }
        public string FROM_DATE { get; set; }
        public int STATE_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public long DOMAIN_CODE { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public long RECORD_ACTIVE { get; set; }
    }
}
