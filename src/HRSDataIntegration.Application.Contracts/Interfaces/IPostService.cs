using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IPostService
    {
        void ConvertJobPostConvertToOracle(string Id);
        void UpdatePostJob(string Id);
    }
}
