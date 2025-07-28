using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.DTOs.Chart
{
    public class TBCHART_POST_TEMPLATE
    {
        public string ID { get; set; }
        public int APPROVED_COUNT { get; set; }
        public string? BACK_COLOR { get; set; }
        public string FONT_NAME { get; set; }
        public int FONT_SIZE { get; set; }
        public int FONT_STYLE { get; set; }
        public string FONT_COLOR { get; set; }
        public int? CHILD_LAYOUT_TYPE_CODE { get; set; }
        public string CHART_TEMPLATE_ID { get; set; }
        public string? PARENT_POST_ID { get; set; }
        public string? POST_ID { get; set; }
        public string? DESCRIPTION { get; set; }
        public Int64 DOMAIN_CODE { get; set; }
        public int? CONNECTOR_TYPE_CODE { get; set; }
        public int? F_P_CONNECTOR_DASH_STYLE_CODE { get; set; }
        public string? F_P_CONNECTOR_COLOR { get; set; }
        public int F_P_CONNECTOR_WIDTH { get; set; }
        public int? WIDTH { get; set; }
        public int? HEIGHT { get; set; }
        public int? TEXT_ALIGNMENT_TYPE_CODE { get; set; }
        public int? CHILD_INDEX { get; set; }
        public int T_C_CONNECTOR_WIDTH { get; set; }
        public int T_C_CONNECTOR_DASH_STYLE_CODE { get; set; }
        public int T_C_CONNECTOR_COLOR { get; set; }
        public int X_CORDINATE { get; set; }
        public int Y_CORDINATE { get; set; }
        public int DRAW_GRADIANT { get; set; }
        public int BORDER_COLOR { get; set; }
        public int BORDER_DASH_STYLE_CODE { get; set; }
        public int BORDER_WIDTH { get; set; }
        public int SHAPE_TYPE_CODE { get; set; }
        public int SHOW_CHILDS_CODE { get; set; }
        public int POST_POSITION_TYPE_CODE { get; set; }
        public int? ASSISTANT_TYPE_CODE { get; set; }
        public int CHART_BOX_TYPE_CODE { get; set; }
        public string BACK_COLOR2 { get; set; }
        public int OPACITY { get; set; }
        public string? RADIF { get; set; }
        public int? COUNT_DISPLAY_CODE { get; set; }
        public string? VIRTUAL_PARENT_POST_ID { get; set; }
    }
}
