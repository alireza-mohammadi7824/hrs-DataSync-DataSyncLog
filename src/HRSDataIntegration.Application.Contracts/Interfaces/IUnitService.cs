using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IUnitService
    {
        void ConvertSqlUnitTableConvertToOracleTBUNITtableWhenInsert(string Id);
        void ConvertUpdateTBUNIT_PARENT_DETAIL(string Id);
        void ConvertUpdateTBUNIT_Name(string Id);
        void ConvertUpdateTBUNIT_Address(string Id);
        void ConvertUpdateTBUNIT_Tels(string Id);
        void ConvertDestroy_Edgham_TBUNIT(string Id);
        void ConvertDestroy_Enhelal_TBUNIT(string Id);

    }
}
