using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IPensionFundService
    {
        void ConvertToPensionFund_Insert_ToOracleTable(string ID);
        void ConvertToPensionFund_Update_ToOracleTable(string ID);
        void ConvertToPensionFund_Delete_ToOracleTable(string ID);
    }
}
