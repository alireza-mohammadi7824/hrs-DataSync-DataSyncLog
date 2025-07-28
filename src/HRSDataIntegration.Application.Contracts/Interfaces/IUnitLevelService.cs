using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IUnitLevelService
    {
        void ConvertSqlUnitLevelTable_Insert_ToOracletable(string Id);
        void ConvertSqlUnitLevelTable_Update_ToOracletable(string Id);
        void ConvertSqlUnitLevelTable_Delete_ToOracletable(string Id);
    }
}
