using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBFAMILY_GRADUATION
    {
        public string ID { get; set; }
        public string BEGIN_DATE { get; set; }
        public string END_DATE { get; set; }
        public string UNSUCCESSFUL_DESCRIPTION { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public int DOMAIN_CODE { get; set; }
        public int RECORD_ACTIVE { get; set; }
        public string FAMILY_ID { get; set; }
        public int STATE_CODE { get; set; }
        public int DIPLOMA_CODE { get; set; }
        public int IS_SUCCESSFUL_CODE { get; set; }
        public int EQUAL_CODE { get; set; }
        public string EDUCATION_STUDY_ID { get; set; }
        public string EDUCATION_BRANCH_ID { get; set; }
    }
}
