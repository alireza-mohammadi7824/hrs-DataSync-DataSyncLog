using HRSDataIntegration.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace HRSDataIntegration.Services.DataSyncLogs
{
    public class DataSyncLogAppService : ApplicationService, IDataSyncLogAppService
    {
        private readonly ISqlRepository<DataSyncLog> _dataSyncLogRepository;

        public DataSyncLogAppService(ISqlRepository<DataSyncLog> dataSyncLogRepository)
        {
            _dataSyncLogRepository = dataSyncLogRepository;
        }

        public async Task<List<DataSyncLog>> LoadLog()
        {
            var list = await _dataSyncLogRepository.GetQueryable().ToListAsync();
            return list;
        }



    }
}
