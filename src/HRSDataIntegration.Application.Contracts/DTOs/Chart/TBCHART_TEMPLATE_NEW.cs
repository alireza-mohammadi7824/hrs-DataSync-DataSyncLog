using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class TBCHART_TEMPLATE_NEW
    {
        public string ID { get; set; }
        public string? APPROVED_DATE { get; set; }
        public string? ORIGIN_NO { get; set; }
        public Int64 DOMAIN_CODE { get; set; }
        public string UNIT_ID { get; set; }
        public int ORIGIN_CODE { get; set; }
        public int STATE_CODE { get; set; }
        public int LEFT_MARGIN { get; set; }
        public int TOP_MARGIN { get; set; }
        public int BACK_COLOR { get; set; }
        public string? CHART_DESCRIPTION { get; set; }
        public int AUTO_DRAW_CODE { get; set; }
        public int CHART_TYPE_CODE { get; set; }
        public string THEME_ID { get; set; }
        public int CURRENT_PAPER_CODE { get; set; }
        public int FONT_SIZE { get; set; }
        public string FONT_NAME { get; set; }
        public int FONT_STYLE { get; set; }
        public string FOR_COLOR { get; set; }
    }
}
