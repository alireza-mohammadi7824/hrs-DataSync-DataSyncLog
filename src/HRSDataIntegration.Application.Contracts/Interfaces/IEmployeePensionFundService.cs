using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IEmployeePensionFundService
    {
        void ConvertToEmployeePensionFund_Insert_ToOracleTable(string ID);
        void ConvertToEmployeePensionFund_Update_ToOracleTable(string ID);
        void ConvertToEmployeePensionFund_Delete_ToOracleTable(string ID);
    }
}
