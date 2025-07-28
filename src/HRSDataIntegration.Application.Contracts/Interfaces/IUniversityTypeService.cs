using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IUniversityTypeService
    {
        void ConvertToUniversityType_Update_ToOracleTable(string ID);
        void ConvertToUniversityType_Delete_ToOracleTable(string ID);
        void ConvertToUniversityType_Insert_ToOracleTable(string ID);
    }
}
