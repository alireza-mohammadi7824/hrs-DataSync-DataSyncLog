using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IChartService
    {
        void ConvertSqlChartTable_Insert_ToOracletable(string Id);
        void ConvertSqlChartTable_Update_ToOracletable(string Id);
    }
}
