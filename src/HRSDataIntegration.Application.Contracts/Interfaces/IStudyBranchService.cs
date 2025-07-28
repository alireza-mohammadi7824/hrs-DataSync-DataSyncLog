using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IStudyBranchService
    {
        void ConvertToStudyBranch_Update_ToOracleTable(string ID);
        void ConvertToStudyBranch_Delete_ToOracleTable(string ID);
        void ConvertToStudyBranch_Insert_ToOracleTable(string ID);
    }
}
