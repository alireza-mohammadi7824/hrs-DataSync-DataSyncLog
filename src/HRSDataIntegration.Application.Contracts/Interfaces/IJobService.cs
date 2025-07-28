using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace HRSDataIntegration.Interfaces
{
    public interface IJobService
    {
        void ConvertJobOverflow(string Id);
    }
}
