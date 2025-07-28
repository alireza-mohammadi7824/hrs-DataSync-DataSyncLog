using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class TBUNIT_DESTROY_DETAIL
    {
        public string ID { get; set; }
        public string UNIT_ID { get; set; }
        public string DESTROY_START_DATE { get; set; }
        public string DESTROY_END_DATE { get; set; }
        public string CANCEL_DESTROY_DESCRIPTION { get; set; }
        public string DESTROY_DESCRIPTION { get; set; }
    }
}
