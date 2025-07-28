using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBPERSONNEL_GRADUATION
    {
        public string ID { get; set; }
        public string RECEIVE_DATE { get; set; }
        public long AVERAGE { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public int DOMAIN_CODE { get; set; }
        public int RECORD_ACTIVE { get; set; }
        public string PERSONNEL_ID { get; set; }
        public int DIPLOMA_CODE { get; set; }
        public string? EDUCATION_STUDY_ID { get; set; }
        public int STATE_CODE { get; set; }
        public int? EQUAL_CODE { get; set; }
        public int? NON_CORRESPONDED_TYPE_CODE { get; set; }
        public int? HAS_THESIS_CODE { get; set; }
        public string? UNIVERSITY_ID { get; set; }
        public string? EDUCATION_BRANCH_ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public int CORRESPOND_EXPERT_CODE { get; set; }
        public string EXEC_DATE { get; set; }
        public int DIPLOMA_TYPE_CODE { get; set; }
        public string RELATED_DATE { get; set; }
        public string RELATED_UNIVERSITY_ID { get; set; }
        public string START_DATE { get; set; }
    }
}
