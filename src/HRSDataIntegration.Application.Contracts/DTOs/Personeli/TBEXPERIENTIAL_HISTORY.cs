using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBEXPERIENTIAL_HISTORY
    {
        public string ID { get; set; }
        public string POST_NAME { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public int YEAR { get; set; }
        public int MONTH { get; set; }
        public int DAY { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public int DOMAIN_CODE { get; set; }
        public int RECORD_ACTIVE { get; set; }
        public string EXTERNAL_HISTORY_ID { get; set; }
        public int EMPLOYEE_ACTIVITY_TYPE_CODE { get; set; }
        public int JOB_CATEGORY_CODE { get; set; }
        public int DIPLOMA_CODE { get; set; }
        public int BANK_EXPERIENCE_APPROVED_CODE { get; set; }
        public string? PERMISSION_FOR_ASSIGN_ID { get; set; }
    }
}
