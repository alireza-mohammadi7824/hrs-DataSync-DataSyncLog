using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IProvinceService
    {
        void InsertProvinceToOracle(string Id);
        void UpdateProvinceToOracleBy(string Id);
        void RemoveProvinceToOracle(string Id);
    }
}
