using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBH_PRSNL_GENEROUS
    {
        public string ID { get; set; }
        public string? LETTER_NO { get; set; }
        public int? YEAR { get; set; }
        public int? MONTH { get; set; }
        public int? DAY { get; set; }
        public int? PERCENT { get; set; }
        public int? CURE_YEAR { get; set; }
        public int? CURE_MONTH { get; set; }
        public int? CURE_DAY { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? REGISTER_DATE { get; set; }
        public string? EXEC_DATE { get; set; }
        public int? RECORD_TYPE_CODE { get; set; }
        public int? DOMAIN_CODE { get; set; }
        public int? RECORD_ACTIVE { get; set; }
        public int GENEROUS_KIND_CODE { get; set; }
        public int DIPLOMA_CODE { get; set; }
        public int STATE_CODE { get; set; }
        public string PERSONNEL_ID { get; set; }
        public string? FROM_DATE { get; set; }
        public string? TO_DATE { get; set; }
    }
}
