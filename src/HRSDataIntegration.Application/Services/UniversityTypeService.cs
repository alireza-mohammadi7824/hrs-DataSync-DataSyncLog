using HRSDataIntegration.DTOs;
using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class UniversityTypeService : IUniversityTypeService
    {
        private readonly IOracleRepository<TBUNIVERSITY_TYPE> _TBUNIVERSITY_TYPERepository;
        private readonly ISqlRepository<UniversityType> _sqlRepositoryUniversityType;
        private readonly IOracleCommon _oracleCommon;
        public UniversityTypeService(IOracleRepository<TBUNIVERSITY_TYPE> TBUNIVERSITY_TYPERepository,
            ISqlRepository<UniversityType> sqlRepositoryUniversityType,
            IOracleCommon oracleCommon)
        {
            _oracleCommon = oracleCommon;
            _sqlRepositoryUniversityType = sqlRepositoryUniversityType;
            _TBUNIVERSITY_TYPERepository = TBUNIVERSITY_TYPERepository;
        }
        public void ConvertToUniversityType_Insert_ToOracleTable(string ID)
        {
            try
            {
                var universityTypeQueryable = _sqlRepositoryUniversityType.GetQueryable();
                var universityType = universityTypeQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title
                    })
                    .FirstOrDefault();
                // var oldId = _oracleCommon.OldColumnValue("HRS.TBUNIVERSITY_TYPE", "ID", universityType.ID.ToString());
                var TBUNIVERSITY_TYPE = new TBUNIVERSITY_TYPE()
                {
                    ID = universityType.ID.ToString(),
                    CODE = universityType.CODE,
                    NAME = universityType.TITLE,
                    //STATE_CODE=
                };
                _TBUNIVERSITY_TYPERepository.Create(TBUNIVERSITY_TYPE);
                _TBUNIVERSITY_TYPERepository.SaveChanges();

                _oracleCommon.InsertInto_DataConverter_MappingId(universityType.ID.ToString(), ID, "HRS.TBUNIVERSITY_TYPE", "ID", "Employee.UniversityType", "ID");
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID), true);
            }
            catch (Exception ex)
            {
                string message = ex.Message?.Length > 500
                                ? ex.Message.Substring(0, 500)
                                : ex.Message;
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID), false, message);
                throw ex;
            }
        }

        public void ConvertToUniversityType_Update_ToOracleTable(string ID)
        {
            try
            {
                var universityTypeQueryable = _sqlRepositoryUniversityType.GetQueryable();
                var universityType = universityTypeQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title
                    })
                    .FirstOrDefault();
                var oldId = _oracleCommon.OldColumnValue("HRS.TBUNIVERSITY_TYPE", "ID", universityType.ID.ToString());
                var TBUNIVERSITY_TYPEQUERYABLE = _TBUNIVERSITY_TYPERepository.GetQueryable();
                var entity = TBUNIVERSITY_TYPEQUERYABLE
                   .Where(x => x.ID.ToString() == universityType.ID.ToString())
                   // .Where(x => x.ID.ToString() == oldId)
                   .ToList()
                   .FirstOrDefault();
                entity.CODE = universityType.CODE;
                entity.NAME = universityType.TITLE;
                _TBUNIVERSITY_TYPERepository.SaveChanges();
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID) , true);
            }
            catch (Exception ex)
            {
                string message = ex.Message?.Length > 500
                               ? ex.Message.Substring(0, 500)
                               : ex.Message;
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID), false, message);
                throw ex;
            }
        }
        public void ConvertToUniversityType_Delete_ToOracleTable(string ID)
        {
            try
            {
                var universityTypeQueryable = _sqlRepositoryUniversityType.GetQueryable();
                var universityType = universityTypeQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title
                    })
                    .FirstOrDefault();
                var oldId = _oracleCommon.OldColumnValue("HRS.TBUNIVERSITY_TYPE", "ID", universityType.ID.ToString());
                var TBUNIVERSITY_TYPEQUERYABLE = _TBUNIVERSITY_TYPERepository.GetQueryable();
                var entity = TBUNIVERSITY_TYPEQUERYABLE
                   .Where(x => x.ID.ToString() == universityType.ID.ToString())
                   // .Where(x => x.ID.ToString() == oldId)
                   .ToList()
                   .FirstOrDefault();

                _TBUNIVERSITY_TYPERepository.Delete(entity);
                _TBUNIVERSITY_TYPERepository.SaveChanges();
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID) , true); 
            }
            catch (Exception ex)
            {
                string message = ex.Message?.Length > 500
                                ? ex.Message.Substring(0, 500)
                                : ex.Message;
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID), false, message);
                throw ex;
            }
        }        
    }
}
