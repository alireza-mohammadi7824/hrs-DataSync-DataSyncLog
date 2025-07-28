using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IUniversityService
    {
        void ConvertToUniversity_Update_ToOracleTable(string ID);
        void ConvertToUniversity_Delete_ToOracleTable(string ID);
        void ConvertToUniversity_Insert_ToOracleTable(string ID);
    }
}
