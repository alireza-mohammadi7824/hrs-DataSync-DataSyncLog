using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IPersonDetailService
    {
        void ConvertToPersonDetail_Insert_ToOracleTable(string ID);
        void ConvertDivorceRegistrationProcessForPersonelToOracleTable(string ID);
        void ConvertPersonelMarriageProcessToOracleTable(string ID);
        void ConvertProcessOfRegisteringDeathFamilyOfPersonelToOracleTable(string ID);
        void ConvertProcessOfRegistrationBirthOfEmployeesChildrean(string ID);
        void ConvertProcessOfRegistrationMarriageOfEmployeesChildrean(string ID);
    }
}
