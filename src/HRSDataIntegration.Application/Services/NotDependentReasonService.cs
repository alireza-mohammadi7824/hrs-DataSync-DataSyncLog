using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class NotDependentReasonService : INotDependentReasonService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBNON_DEPENDENT_REASON> _TBNON_DEPENDENT_REASONRepository;
        private readonly ISqlRepository<NotDependentReason> _sqlRepositoryNotDependentReason;
        public NotDependentReasonService(IOracleCommon oracleCommon,
               IOracleRepository<TBNON_DEPENDENT_REASON> TBNON_DEPENDENT_REASONRepository,
               ISqlRepository<NotDependentReason> sqlRepositoryNotDependentReason)
        {
            _oracleCommon = oracleCommon;
            _TBNON_DEPENDENT_REASONRepository = TBNON_DEPENDENT_REASONRepository;
            _sqlRepositoryNotDependentReason = sqlRepositoryNotDependentReason;
        }
        public void ConvertToNotDependentReasonService_Insert_ToOracleTable(string ID)
        {
            var notDependentReasonQueryable = _sqlRepositoryNotDependentReason.GetQueryable();
            var notDependentReason = notDependentReasonQueryable
                .Where(x=>x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    CODE = x.Code,
                    NAME = x.Title
                })
                .FirstOrDefault();

            var TBNON_DEPENDENT_REASON = new TBNON_DEPENDENT_REASON()
            {
                ID = notDependentReason.ID.ToString(),
                CODE = notDependentReason.CODE,
                NAME = notDependentReason.NAME,
            };
            _TBNON_DEPENDENT_REASONRepository.Create(TBNON_DEPENDENT_REASON);
            _TBNON_DEPENDENT_REASONRepository.SaveChanges();

            _oracleCommon.InsertInto_DataConverter_MappingId(notDependentReason.ID.ToString(), ID, "HRS.TBNON_DEPENDENT_REASON", "ID", "Employee.NotDependentReason", "ID");
        }

        public void ConvertToNotDependentReasonService_Update_ToOracleTable(string ID)
        {
            var notDependentReasonQueryable = _sqlRepositoryNotDependentReason.GetQueryable();
            var notDependentReason = notDependentReasonQueryable
                .Where(x => x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    CODE = x.Code,
                    NAME = x.Title
                })
                .FirstOrDefault();

            var oldId = _oracleCommon.OldColumnValue("HRS.TBNON_DEPENDENT_REASON", "ID", notDependentReason.ID.ToString());
            var TBNON_DEPENDENT_REASON = _TBNON_DEPENDENT_REASONRepository.GetQueryable();
            var entity = TBNON_DEPENDENT_REASON
                .Where(x=>x.ID.ToString() == oldId)
                .ToList()
                .FirstOrDefault();

            entity.CODE = notDependentReason.CODE;
            entity.NAME = notDependentReason.NAME;
            _TBNON_DEPENDENT_REASONRepository.SaveChanges();
        }
        public void ConvertToNotDependentReasonService_Delete_ToOracleTable(string ID)
        {
            var notDependentReasonQueryable = _sqlRepositoryNotDependentReason.GetQueryable();
            var notDependentReason = notDependentReasonQueryable
                .Where(x => x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    CODE = x.Code,
                    NAME = x.Title
                })
                .FirstOrDefault();

            var oldId = _oracleCommon.OldColumnValue("HRS.TBNON_DEPENDENT_REASON", "ID", notDependentReason.ID.ToString());
            var TBNON_DEPENDENT_REASON = _TBNON_DEPENDENT_REASONRepository.GetQueryable();
            var entity = TBNON_DEPENDENT_REASON
                .Where(x => x.ID.ToString() == oldId)
                .ToList()
                .FirstOrDefault();

            _TBNON_DEPENDENT_REASONRepository.Delete(entity);
            _TBNON_DEPENDENT_REASONRepository.SaveChanges();
        }        
    }
}
