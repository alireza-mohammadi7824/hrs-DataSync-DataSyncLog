using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IEmployeeDependentStatusService
    {
        void InsertEmployeeDependentStatusServiceFromSqlToOracle(string ID);
        void UpdateEmployeeDependentStatusServiceFromSqlToOracle(string ID);
        void DeleteEmployeeDependentStatusServiceFromSqlToOracle(string ID);
    }
}
