using System;
using System.Numerics;

namespace HRSDataIntegration.Entities
{
    public class TBACTIVITY_LOG_CHARTDESIGN
    {
        public TBACTIVITY_LOG_CHARTDESIGN(
              Guid ID
            , Guid dOC_ID
            , DateTime dATE_TIME
            , string dESCRIPTION
            , string uSER_NAME
            , Int64 dOMAIN_CODE
            , string aCTIVITY_ID
            , string dOC_VALUE_ID
            , int dOCUMENT_CODE
            )
        {
            ID = ID;
            DOC_ID = dOC_ID;
            DATE_TIME = dATE_TIME;
            DESCRIPTION = dESCRIPTION;
            USER_NAME = uSER_NAME;
            DOMAIN_CODE = dOMAIN_CODE;
            ACTIVITY_ID = aCTIVITY_ID;
            DOC_VALUE_ID = dOC_VALUE_ID;
            DOCUMENT_CODE = dOCUMENT_CODE;
        }

        public Guid ID { get; set; }
        public Guid DOC_ID { get; set; }
        public DateTime DATE_TIME { get; set; }
        public string? DESCRIPTION { get; set; }
        public string USER_NAME { get; set; }
        public Int64 DOMAIN_CODE { get; set; }
        public string ACTIVITY_ID { get; set; }
        public string? DOC_VALUE_ID { get; set; }
        public int DOCUMENT_CODE { get; set; }
    }
}
