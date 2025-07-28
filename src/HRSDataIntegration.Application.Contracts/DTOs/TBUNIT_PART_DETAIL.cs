using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs
{
    public class TBUNIT_PART_DETAIL
    {
        public Guid ID { get; set; }
        public Guid UNIT_ID { get; set; }
        public Guid PART_ID { get; set; }
        public string EXEC_DATE { get; set; }
    }
}
