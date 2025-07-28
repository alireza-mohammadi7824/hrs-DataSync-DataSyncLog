using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class TBCHART_LINK
    {
        public string ID { get; set; }
        public string CHART_ID { get; set; }
        public int START_CON_CODE { get; set; }
        public int END_CON_CODE { get; set; }
        public string PARENT_NODE_ID { get; set; }
        public string CHILD_NODE_ID { get; set; }
        public string COLOR { get; set; }
        public int WIDTH { get; set; }
        public int DASH_STYLE_CODE { get; set; }
        public int DRAW_GRADIANT { get; set; }
        public int CONNECTOR_TYPE_CODE { get; set; }
        public int OPACITY { get; set; }
        public string? TEXT { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? POINTS_DATA { get; set; }
        public string FONT_COLOR { get; set; }
        public string FONT_NAME { get; set; }
        public int FONT_SIZE { get; set; }
        public int FONT_STYLE { get; set; }
        public int AUTODRAW { get; set; }
        public Int64 DOMAIN_CODE { get; set; }
    }
}
