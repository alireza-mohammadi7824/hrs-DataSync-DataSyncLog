using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Personeli
{
    public class TBCPENSION_FUND
    {
        [Key]
        public int CODE { get; set; }
        public string NAME { get; set; }
        public string CAPTION { get; set; }
    }
}
