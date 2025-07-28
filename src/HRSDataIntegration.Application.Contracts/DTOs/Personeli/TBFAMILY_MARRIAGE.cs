using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBFAMILY_MARRIAGE
    {
        public string ID { get; set; }
        public string MARRIAGE_DATE { get; set; }
        public string? MARRIAGE_DESCRIPTION { get; set; }
        public string? DIVORCE_DATE { get; set; }
        public string? DIVORCE_DESCRIPTION { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public long DOMAIN_CODE { get; set; }
        public long RECORD_ACTIVE { get; set; }
        public string FAMILY_ID { get; set; }
        public int STATE_CODE { get; set; }
    }
}
