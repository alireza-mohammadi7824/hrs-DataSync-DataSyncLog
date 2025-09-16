using HRSDataIntegration.DTOs;
using HRSDataIntegration.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;


namespace HRSDataIntegration.Services
{
    public class JobService : IJobService
    {
        private readonly IOracleRepository<TBJOB> _jobRepository;
        private readonly ISqlRepository<JobDetail> _sqlRepositoryJobDetail;
        private readonly ISqlRepository<MappingId> _sqlRepositoryMappingId;
        private readonly IOracleCommon _oracleCommon;



        public JobService(
            IOracleRepository<TBJOB> jobRepository,
            ISqlRepository<JobDetail> sqlRepositoryJobDetail,
            ISqlRepository<MappingId> sqlRepositoryMappingId,
            IOracleCommon oracleCommon)
        {
            _oracleCommon = oracleCommon;
            _jobRepository = jobRepository;
            _sqlRepositoryJobDetail = sqlRepositoryJobDetail;
            _sqlRepositoryMappingId = sqlRepositoryMappingId;
        }
        public void ConvertJobOverflow(string Id)
        {
            try
            {
                // Guid targetId = new Guid("7A429CEE-F46E-EFD8-7508-3A16C10AE95C");

                var sqlJobDetail = _sqlRepositoryJobDetail.GetQueryable()
                                    .Where(x => x.JobId.ToString() == Id)
                                    .Select(x => new
                                    {
                                        ID = x.Job.Id,
                                        CODE = x.Code,
                                        NAME = x.Title,
                                        ACTIVE_TYPE_CODE = x.Job.IsActive ? 1 : 0,
                                        RASTEH_ID = "",
                                        JOB_GROUP_CODE = x.JobGroup.Code,
                                        ACTIVE_DATE = x.EffectiveDateFrom,
                                        INACTIVE_DATE = x.EffectiveDateTo,
                                        RASTEHID = x.JobRastehId
                                    })
                                    .FirstOrDefault();

                var OldColumnValueOfMappingId = _oracleCommon.OldColumnValue("HRS.TBRASTEH", "ID", sqlJobDetail.RASTEHID.ToString());

                //var OldColumnValueOfMappingId = _sqlRepositoryMappingId.GetQueryable()
                //                .Where(x => x.NewColumnValue == sqlJobDetail.RASTEHID.ToString())
                //                .Select(x => x.OldColumnValue)                            
                //                .FirstOrDefault();

                var TBJobData = new TBJOB()
                {
                    ID = sqlJobDetail.ID.ToString(),
                    CODE = sqlJobDetail.CODE,
                    NAME = sqlJobDetail.NAME,
                    ACTIVE_TYPE_CODE = sqlJobDetail.ACTIVE_TYPE_CODE,
                    RASTEH_ID = OldColumnValueOfMappingId,
                    ACTIVE_DATE = _oracleCommon.ToStringDateTime(sqlJobDetail.ACTIVE_DATE),
                    INACTIVE_DATE = sqlJobDetail.INACTIVE_DATE == 0 ? null : _oracleCommon.ToStringDateTime(sqlJobDetail.INACTIVE_DATE),
                    JOB_GROUP_CODE = sqlJobDetail.JOB_GROUP_CODE
                };



                // start Insert To JOB Table
                #region Debug
                //var TBJOBCountBeforeInsert = _jobRepository.GetQueryable().Count();
                // var TBJOBCountBeforeInsert = _oracleCommon.GetTableCount("TBJOB");
                #endregion Debug
                _jobRepository.Create(TBJobData);
                _jobRepository.SaveChanges();
                #region Debug
                // var TBJOBCountAfterInsert = _oracleCommon.GetTableCount("TBJOB");
                #endregion Debug
                // end Insert To JOB Table



                // start Insert To Log Table TBACTIVITY_LOG_CHARTDESIGN
                #region Debug
                // var chartDesignCountBeforeInsert = _oracleCommon.GetTableCount("TBACTIVITY_LOG_CHARTDESIGN");
                #endregion Debug
                _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 1009, 8589934592);
                #region Debug
                //  var chartDesignCountAfterInsert = _oracleCommon.GetTableCount("TBACTIVITY_LOG_CHARTDESIGN");
                #endregion Debug
                // end Insert To Log Table TBACTIVITY_LOG_CHARTDESIGN




                // start Insert To Log Table DataConverter_MappingId
                #region Debug
                // var DataConverter_MappingIdCountBeforeInsert = _oracleCommon.GetTableCount("MappingId");
                #endregion Debug
                _oracleCommon.InsertInto_DataConverter_MappingId(sqlJobDetail.ID.ToString(), Id, "HRS.TBJOB", "ID", "OrganChart.Job", "ID");
                #region Debug
                // var DataConverter_MappingIdCountAfterInsert = _oracleCommon.GetTableCount("MappingId");
                #endregion Debug
                // end Insert To Log Table DataConverter_MappingId

                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id), true);
            }
            catch (Exception ex)
            {

                string message = ex.Message?.Length > 500
                                ? ex.Message.Substring(0, 500)
                                : ex.Message;
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id), false, message);
                throw ex;
            }

        }
    }
}
