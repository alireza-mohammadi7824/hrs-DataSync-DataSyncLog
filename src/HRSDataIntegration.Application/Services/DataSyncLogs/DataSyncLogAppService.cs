using HRSDataIntegration.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<DataSyncLog>> LoadLog(DardDTO input)
        {
            List<DataSyncLog> list = new List<DataSyncLog>();
            if (input.DateFrom != null && input.DateTo != null)
            {
                list = await _dataSyncLogRepository.GetQueryable().Where(x => x.CreationTime < input.DateTo && x.CreationTime > input.DateFrom).ToListAsync();
            }
            else
            {
                list = await _dataSyncLogRepository.GetQueryable().ToListAsync();
            }
            return list;
        }


        public class DardDTO
        {
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
        }
    }
}
