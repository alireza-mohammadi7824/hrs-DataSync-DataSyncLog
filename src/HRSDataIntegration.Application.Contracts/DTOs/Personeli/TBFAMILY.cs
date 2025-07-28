using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBFAMILY
    {
        public string ID { get; set; }
        public string NO { get; set; }
        public string NAME { get; set; }
        public string FAMILY { get; set; }
        public string FATHER_NAME { get; set; }
        public string ID_NO { get; set; }
        public string? NATIONAL_ID { get; set; }
        public string BIRTH_DATE { get; set; }
        public string? DEATH_DATE { get; set; }
        public int RECORD_TYPE_CODE { get; set; }
        public long DOMAIN_CODE { get; set; }
        public long RECORD_ACTIVE { get; set; }
        public string PERSONNEL_ID { get; set; }
        public int STATE_CODE { get; set; }
        public int TYPE_CODE { get; set; }
        public string? CITY_ID { get; set; }
        public string ID_SERIAL1 { get; set; }
        public string ID_SERIAL2 { get; set; }
        public string ID_SERIAL3 { get; set; }
    }
}
