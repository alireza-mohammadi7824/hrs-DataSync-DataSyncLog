using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class TBPOST
    {
        public string ID { get; set; }
        public int CODE { get; set; }
        public string NAME { get; set; }
        public int POST_TYPE_CODE { get; set; }
        public int ACTIVE_TYPE_CODE { get; set; }
        public int? STAR_POST_CODE { get; set; }
        public int? SPECIFIC_PARTS_CODE { get; set; }
        public string CREATION_DATE { get; set; }
        public int? POST_RASTEH_CODE { get; set; }
    }
}
