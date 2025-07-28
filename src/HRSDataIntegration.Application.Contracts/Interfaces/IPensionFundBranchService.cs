using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IPensionFundBranchService
    {
        void ConvertToPensionFundBranch_Insert_ToOracleTable(string ID);
        void ConvertToPensionFundBranch_Update_ToOracleTable(string ID);
        void ConvertToPensionFundBranch_Delete_ToOracleTable(string ID);
    }
}
