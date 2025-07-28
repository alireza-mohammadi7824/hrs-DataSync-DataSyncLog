using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBPERSONNEL_TOTAL
    {
        public string ID { get; set; }
        public int? NO { get; set; }
        public int? CONVENTIONAL_NO { get; set; }
        public string NAME { get; set; }
        public string FAMILY { get; set; }
        public string FATHER_NAME { get; set; }
        public string LATIN_NAME { get; set; }
        public string LATIN_FAMILY { get; set; }
        public string ID_NO { get; set; }
        public string? NATIONAL_ID { get; set; }
        public string BIRTH_DATE { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public int ENTRY_STATUS_CODE { get; set; }
        public long DOMAIN_CODE { get; set; }
        public long RECORD_ACTIVE { get; set; }
        public int STATE_CODE { get; set; }
        public string? ID_CARD_CITY_ID { get; set; }
        public string? BIRTH_CITY_ID { get; set; }
        public int DIPLOMA_CODE { get; set; }
        public int SEX_TYPE_CODE { get; set; }
        public int MARRIAGE_TYPE_CODE { get; set; }
        public string ID_SERIAL1 { get; set; }
        public string ID_SERIAL2 { get; set; }
        public string ID_SERIAL3 { get; set; }
        public string? JOB_START_DATE { get; set; }
    }
}
