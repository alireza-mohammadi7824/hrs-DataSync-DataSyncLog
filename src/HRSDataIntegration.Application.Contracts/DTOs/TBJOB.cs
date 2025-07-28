using System;

namespace HRSDataIntegration.DTOs
{
    
    public class TBJOB
    {
        public string ID { get; set; }
        public int CODE { get; set; }
        public string NAME { get; set; }
        public int ACTIVE_TYPE_CODE { get; set; }
        public string RASTEH_ID { get; set; }
        public string? ACTIVE_DATE { get; set; }
        public string? INACTIVE_DATE { get; set; }
        public int? JOB_GROUP_CODE { get; set; }
    }
}
