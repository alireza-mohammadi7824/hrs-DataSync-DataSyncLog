using AutoMapper;
using HRSDataIntegration.DTOs;
//using DataSyncLogEntity = HRSDataIntegration.DataSyncLogs.DataSyncLog;

namespace HRSDataIntegration;

public class HRSDataIntegrationApplicationAutoMapperProfile : Profile
{
    public HRSDataIntegrationApplicationAutoMapperProfile()
    {
        //CreateMap<DataSyncLogEntity, DataSyncLogDto>();
        //CreateMap<CreateUpdateDataSyncLogDto, DataSyncLogEntity>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
