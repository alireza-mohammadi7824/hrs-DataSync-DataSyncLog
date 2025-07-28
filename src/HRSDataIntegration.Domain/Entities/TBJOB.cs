using System;
using Volo.Abp.Domain.Entities;

namespace HRSDataIntegration.Entities
{
    
    public class TBJOB: IEntity<Guid>
    {
        public Guid ID { get; set; }

        public Guid Id => throw new NotImplementedException();

        public int CODE { get; set; }
        public string NAME { get; set; }
        public int ACTIVE_TYPE_CODE { get; set; }
        public string RASTEH_ID { get; set; }
        public string? ACTIVE_DATE { get; set; }
        public string? INACTIVE_DATE { get; set; }
        public int? JOB_GROUP_CODE { get; set; }

        public object?[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
