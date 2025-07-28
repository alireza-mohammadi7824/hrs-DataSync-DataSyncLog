using System;
using System.Numerics;

namespace HRSDataIntegration.DTOs
{
    public class TBACTIVITY_LOG_CHARTDESIGN
    {
        public TBACTIVITY_LOG_CHARTDESIGN()
        {
                
        }
        public TBACTIVITY_LOG_CHARTDESIGN(
              Guid ID
            , string dOC_ID
            , DateTime dATE_TIME
            , string dESCRIPTION
            , string uSER_NAME
            , Int64 dOMAIN_CODE
            , string aCTIVITY_ID
            , string dOC_VALUE_ID
            , int dOCUMENT_CODE
            )
        {
            this.ID = ID;
            this.DOC_ID = dOC_ID;
            this.DATE_TIME = dATE_TIME;
            this.DESCRIPTION = dESCRIPTION;
            this.USER_NAME = uSER_NAME;
            this.DOMAIN_CODE = dOMAIN_CODE;
            this.ACTIVITY_ID = aCTIVITY_ID;
            this.DOC_VALUE_ID = dOC_VALUE_ID;
            this.DOCUMENT_CODE = dOCUMENT_CODE;
        }

        public Guid ID { get; set; }
        public string DOC_ID { get; set; }
        public DateTime DATE_TIME { get; set; }
        public string? DESCRIPTION { get; set; }
        public string USER_NAME { get; set; }
        public Int64 DOMAIN_CODE { get; set; }
        public string ACTIVITY_ID { get; set; }
        public string? DOC_VALUE_ID { get; set; }
        public int DOCUMENT_CODE { get; set; }
    }
}
