using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IPersonEducationService
    {
        void ConvertToPersonEducationService_Insert_ToOracleTable(string ID);
        void ConvertToPersonEducationPromotionToOracleTable(string ID);
    }
}
