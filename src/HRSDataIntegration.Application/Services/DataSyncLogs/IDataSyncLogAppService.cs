using HRSDataIntegration.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using static HRSDataIntegration.Services.DataSyncLogs.DataSyncLogAppService;


namespace HRSDataIntegration.Services.DataSyncLogs
{
    public interface IDataSyncLogAppService : ITransientDependency, IApplicationService
    {
        Task<List<DataSyncLog>> LoadLog();
    }
}
