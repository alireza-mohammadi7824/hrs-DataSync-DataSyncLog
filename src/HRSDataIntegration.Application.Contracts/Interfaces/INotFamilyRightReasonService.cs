using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface INotFamilyRightReasonService
    {
        void ConvertNotFamilyRightReasonInsertToOracleTable(string Id);
        void ConvertNotFamilyRightReasonUpdateToOracleTable(string Id);
        void ConvertNotFamilyRightReasonDeleteToOracleTable(string Id);
    }
}
