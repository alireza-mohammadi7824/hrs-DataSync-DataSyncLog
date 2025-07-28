using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class TBUNIT_BIG_VILLAGE_DETAIL
    {
        public Guid ID { get; set; }
        public Guid UNIT_ID { get; set; }
        public Guid BIG_VILLAGE_ID { get; set; }
        public string EXEC_DATE { get; set; }
    }
}
