using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class NotFamilyRightReasonService : INotFamilyRightReasonService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly ISqlRepository<NotFamilyRightReason> _sqlRepositoryNotFamilyRightReason;
        private readonly IOracleRepository<TBNON_LIABILITY_REASON> _oracleRepositoryTBNON_LIABILITY_REASON;

        public NotFamilyRightReasonService(IOracleRepository<TBNON_LIABILITY_REASON> oracleRepositoryTBNON_LIABILITY_REASON,
            ISqlRepository<NotFamilyRightReason> sqlRepositoryNotFamilyRightReason,
            IOracleCommon oracleCommon)
        {
            _oracleCommon = oracleCommon;
            _sqlRepositoryNotFamilyRightReason = sqlRepositoryNotFamilyRightReason;
            _oracleRepositoryTBNON_LIABILITY_REASON = oracleRepositoryTBNON_LIABILITY_REASON;
        }
       

        public void ConvertNotFamilyRightReasonInsertToOracleTable(string Id)
        {
            var notFamilyRightReasonQueryable = _sqlRepositoryNotFamilyRightReason.GetQueryable();
            var notFamilyRightReason = notFamilyRightReasonQueryable
                .Where(x => x.Id.ToString() == Id)
                .Select(x => new
                {
                    ID = x.Id,
                    CODE = x.Code,
                    TITLE = x.Title
                })
                .FirstOrDefault();
            // var oldId = _oracleCommon.OldColumnValue("HRS.TBNON_LIABILITY_REASON","ID", notFamilyRightReason.ID.ToString());
            var TBNON_LIABILITY_REASON = new TBNON_LIABILITY_REASON()
            {
                ID = notFamilyRightReason.ID.ToString(),
                CODE = notFamilyRightReason.CODE,
                NAME = notFamilyRightReason.TITLE,
            };

            _oracleRepositoryTBNON_LIABILITY_REASON.Create(TBNON_LIABILITY_REASON);
            _oracleRepositoryTBNON_LIABILITY_REASON.SaveChanges();

            _oracleCommon.InsertInto_DataConverter_MappingId(notFamilyRightReason.ID.ToString(), Id, "HRS.TBNON_LIABILITY_REASON", "ID", "Employee.NotFamilyRightReason", "ID");
        }

        public void ConvertNotFamilyRightReasonUpdateToOracleTable(string Id)
        {
            var notFamilyRightReasonQueryable = _sqlRepositoryNotFamilyRightReason.GetQueryable();
            var notFamilyRightReason = notFamilyRightReasonQueryable
                .Where(x => x.Id.ToString() == Id)
                .Select(x => new
                {
                    ID = x.Id,
                    CODE = x.Code,
                    NAME = x.Title
                })
                .FirstOrDefault();
            var oldId = _oracleCommon.OldColumnValue("HRS.TBNON_LIABILITY_REASON", "ID", notFamilyRightReason.ID.ToString());
            var TBNON_LIABILITY_REASON = _oracleRepositoryTBNON_LIABILITY_REASON.GetQueryable();

            var entity = TBNON_LIABILITY_REASON
                .Where(x => x.ID.ToString() == notFamilyRightReason.ID.ToString()) //  اینجا  oldId  باید باشد؟
                .ToList()
                .FirstOrDefault();
            entity.CODE = notFamilyRightReason.CODE;
            entity.NAME = notFamilyRightReason.NAME;
            _oracleRepositoryTBNON_LIABILITY_REASON.SaveChanges();
        }

        public void ConvertNotFamilyRightReasonDeleteToOracleTable(string Id)
        {
            var notFamilyRightReasonQueryable = _sqlRepositoryNotFamilyRightReason.GetQueryable();
            var notFamilyRightReason = notFamilyRightReasonQueryable
                .Where(x => x.Id.ToString() == Id)
                .Select(x => new
                {
                    ID = x.Id,
                    CODE = x.Code,
                    NAME = x.Title
                })
                .FirstOrDefault();

            var oldId = _oracleCommon.OldColumnValue("HRS.TBNON_LIABILITY_REASON", "ID", notFamilyRightReason.ID.ToString());
            var TBNON_LIABILITY_REASON = _oracleRepositoryTBNON_LIABILITY_REASON.GetQueryable();

            var entity = TBNON_LIABILITY_REASON
                .Where(x => x.ID.ToString() == notFamilyRightReason.ID.ToString())  //  اینجا  oldId  باید باشد؟
                .ToList()
                .FirstOrDefault();

            _oracleRepositoryTBNON_LIABILITY_REASON.Delete(entity);
            _oracleRepositoryTBNON_LIABILITY_REASON.SaveChanges();
        }
    }
}
