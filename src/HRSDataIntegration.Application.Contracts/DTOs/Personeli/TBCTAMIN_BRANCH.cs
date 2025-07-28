using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBCTAMIN_BRANCH
    {
        [Key]
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string CAPTION { get; set; }
        public string POLITICAL_PROVINCE_CAPTION { get; set; }
    }
}
