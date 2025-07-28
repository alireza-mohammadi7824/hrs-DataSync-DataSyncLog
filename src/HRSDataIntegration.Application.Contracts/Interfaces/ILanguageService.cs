using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface ILanguageService
    {
        void ConvertToLanguage_Insert_ToOracleTable(string ID);
        void ConvertToLanguage_Update_ToOracleTable(string ID);
        void ConvertToLanguage_Delete_ToOracleTable(string ID);
    }
}
