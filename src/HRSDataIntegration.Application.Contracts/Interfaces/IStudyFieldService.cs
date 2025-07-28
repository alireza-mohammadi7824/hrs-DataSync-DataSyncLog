using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IStudyFieldService
    {
        void ConvertToStudyField_Insert_ToOracleTable(string ID);
        void ConvertToStudyField_Update_ToOracleTable(string ID);
        void ConvertToStudyField_Delete_ToOracleTable(string ID);
    }
}
