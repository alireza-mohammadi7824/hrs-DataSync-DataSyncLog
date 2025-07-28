using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface INotDependentReasonService
    {
        void ConvertToNotDependentReasonService_Update_ToOracleTable(string ID);
        void ConvertToNotDependentReasonService_Delete_ToOracleTable(string ID);
        void ConvertToNotDependentReasonService_Insert_ToOracleTable(string ID);
    }
}
