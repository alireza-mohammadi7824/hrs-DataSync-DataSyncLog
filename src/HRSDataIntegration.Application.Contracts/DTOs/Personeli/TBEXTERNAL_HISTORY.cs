using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBEXTERNAL_HISTORY
    {
        public string ID { get; set; }
        public string REQUEST_DATE { get; set; }
        public string? COMPANY_NAME { get; set; }
        public string? INQUIRY_LETTER_NO { get; set; }
        public string? INQUIRY_LETTER_DATE { get; set; }
        public string? COMPANY_BEGIN_DATE { get; set; }
        public string? COMPANY_END_DATE { get; set; }
        public int? LAST_SALARY { get; set; }
        public int? RECEIVED_AMOUNT { get; set; }
        public string? RETIREMENT_INQUIRY_LETTER_NO { get; set; }
        public string? RETIREMENT_INQUIRY_LETTER_DATE { get; set; }
        public int? TRANSFERED_AMOUNT { get; set; }
        public int? CONFIRMED_YEAR { get; set; }
        public int? CONFIRMED_MONTH { get; set; }
        public int? CONFIRMED_DAY { get; set; }
        public string? INSURANCE_NAME { get; set; }
        public string? COMMAND_NO { get; set; }
        public string? EXEC_DATE { get; set; }
        public string? COMMAND_DESCRIPTION { get; set; }
        public int? EMPLOYEE_SHARE_AMOUNT { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public int DOMAIN_CODE { get; set; }
        public int RECORD_ACTIVE { get; set; }
        public string PERSONNEL_ID { get; set; }
        public int? PREV_COMPANY_TYPE_CODE { get; set; }
        public int? CURRENT_COMPANY_TYPE_CODE { get; set; }
        public int? END_JOB_REASON_CODE { get; set; }
        public int? INSURANCE_TYPE_CODE { get; set; }
        public int STATE_CODE { get; set; }
        public int EXTERNAL_HISTORY_COMMAND_CODE { get; set; }
        public int? PAY_TO_RETIRMENT_BOX_CODE { get; set; }
        public int? CALC_TRANSFARED_AMOUNT { get; set; }
        public string? ISSUE_DATE { get; set; }
    }
}
