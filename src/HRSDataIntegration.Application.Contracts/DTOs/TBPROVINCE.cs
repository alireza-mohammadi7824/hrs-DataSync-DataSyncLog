using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class TBPROVINCE
    {
        public string ID { get; set; }
        public int CODE { get; set; }
        public string NAME { get; set; }
        public Int64 DOMAIN_CODE { get; set; }
        public string? POLITICAL_PROVINCE_ID { get; set; }
        public int EVALUATION_CODE { get; set; }
        public string EXEC_DATE { get; set; }
        public int STATE_CODE { get; set; }

    }
}
