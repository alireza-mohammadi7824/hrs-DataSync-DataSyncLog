using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBPERSONNEL_PENSION_FUND
    {
        public string ID { get; set; }
        public string PERSONNEL_ID { get; set; }
        public long DOMAIN_CODE { get; set; }
        public long RECORD_ACTIVE { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public int PENSION_FUND_CODE { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string MEMBER_CODE { get; set; }
        public int IS_MEMBER_REFAH_FUND { get; set; }
        public int IS_MEMBER_HEALTH { get; set; }
        public int IS_MEMBER_SAVING_CASH { get; set; }
        public int STATE_CODE { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
