using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class TBUNIT
    {
        public string ID { get; set; }
        public int CODE { get; set; }
        public string NAME { get; set; }
        public string? ADDRESS { get; set; }
        public string? TELEPHONE { get; set; }
        public string? FAX { get; set; }
        public string CREATE_DATE { get; set; }
        public string? DESTROY_DATE { get; set; }
        public int TYPE_CODE { get; set; }
        public string? PARENT_UNIT_ID { get; set; }
        public string? PROVINCE_ID { get; set; }
        public string CITY_ID { get; set; }
        public string? PART_ID { get; set; }
        public string? BIG_VILLAGE_ID { get; set; }
        public string? VILLAGE_ID { get; set; }
        public int STATE_CODE { get; set; }
        public string? DIRECOR_UNIT_ID { get; set; }
        public string? UNIT_CLASS_ID { get; set; }
        public int? BRANCH_RANK_CODE { get; set; }
        public int? BRANCH_TYPE_CODE { get; set; }
        public string? UNIT_RANKING_GROUP_ID { get; set; }
        public int? MEHR_CODE { get; set; }
        public string TOWNSHIP_ID { get; set; }
        public string POLITICAL_PROVINCE_ID { get; set; }
        public string? MANAGERTEL { get; set; }
        public string? ASSISTTEL { get; set; }
        public string? LOANOFFICERTEL { get; set; }
        public string? EXCHANGETEL { get; set; }
        public string? LATITUDE { get; set; }
        public string? LONGITUDE { get; set; }
        public string? CBI_CODE { get; set; }
    }
}
