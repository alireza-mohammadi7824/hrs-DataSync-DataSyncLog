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
    public class UniversityService : IUniversityService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBUNIVERSITY> _TBUNIVERSITYRepository;
        private readonly ISqlRepository<University> _sqlRepositoryUniversity;
        public UniversityService(IOracleCommon oracleCommon, IOracleRepository<TBUNIVERSITY> TBUNIVERSITYRepository, ISqlRepository<University> sqlRepositoryUniversity)
        {
            _oracleCommon = oracleCommon;
            _TBUNIVERSITYRepository = TBUNIVERSITYRepository;
            _sqlRepositoryUniversity = sqlRepositoryUniversity;
        }

        public void ConvertToUniversity_Insert_ToOracleTable(string ID)
        {
            try
            {
                var universityQueryable = _sqlRepositoryUniversity.GetQueryable();
                var university = universityQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title,
                        CITY_ID = x.CountryDivisionId,
                        UNIVERSITY_TYPE_ID = x.UniversityTypeId,
                        STATE_CODE = 2
                    })
                    .FirstOrDefault();

                var oldCityId = _oracleCommon.OldColumnValue("TBCITY", "ID", university.CITY_ID.ToString());
                var oldUniversityId = _oracleCommon.OldColumnValue("TBUNIVERSITY_TYPE", "ID", university.UNIVERSITY_TYPE_ID.ToString());

                var TBUNIVERSITY = new TBUNIVERSITY()
                {
                    ID = university.ID.ToString(),
                    CODE = university.CODE,
                    NAME = university.TITLE,
                    CITY_ID = oldCityId,
                    UNIVERSITY_TYPE_ID = oldUniversityId,
                    STATE_CODE = 2
                };

                _TBUNIVERSITYRepository.Create(TBUNIVERSITY);
                _TBUNIVERSITYRepository.SaveChanges();
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID), true);
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

        public void ConvertToUniversity_Update_ToOracleTable(string ID)
        {
            try
            {
                var universityQueryable = _sqlRepositoryUniversity.GetQueryable();
                var university = universityQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        NAME = x.Title,
                        CITY_ID = x.CountryDivisionId,
                        UNIVERSITY_TYPE_ID = x.UniversityTypeId
                    })
                    .FirstOrDefault();

                var oldId = _oracleCommon.OldColumnValue("HRS.TBUNIVERSITY", "ID", university.ID.ToString());
                var oldCityId = _oracleCommon.OldColumnValue("HRS.TBCITY", "ID", university.CITY_ID.ToString());
                var oldUniversityId = _oracleCommon.OldColumnValue("HRS.TBUNIVERSITY_TYPE", "ID", university.UNIVERSITY_TYPE_ID.ToString());

                var TBUNIVERSITYQUERABLE = _TBUNIVERSITYRepository.GetQueryable();
                var entity = TBUNIVERSITYQUERABLE
                    .Where(x => x.ID == university.ID.ToString())
                    .ToList()
                    .FirstOrDefault();

                entity.CODE = university.CODE;
                entity.NAME = university.NAME;
                entity.CITY_ID = oldCityId;
                entity.UNIVERSITY_TYPE_ID = oldUniversityId;
                _TBUNIVERSITYRepository.SaveChanges();
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
        public void ConvertToUniversity_Delete_ToOracleTable(string ID)
        {
            try
            {
                var universityQueryable = _sqlRepositoryUniversity.GetQueryable();
                var university = universityQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        NAME = x.Title,
                        CITY_ID = x.CountryDivisionId,
                        UNIVERSITY_TYPE_ID = x.UniversityTypeId
                    })
                    .FirstOrDefault();

                var oldId = _oracleCommon.OldColumnValue("HRS.TBUNIVERSITY", "ID", university.ID.ToString());
                var oldCityId = _oracleCommon.OldColumnValue("HRS.TBCITY", "ID", university.CITY_ID.ToString());
                var oldUniversityId = _oracleCommon.OldColumnValue("HRS.TBUNIVERSITY_TYPE", "ID", university.UNIVERSITY_TYPE_ID.ToString());

                var TBUNIVERSITYQUERABLE = _TBUNIVERSITYRepository.GetQueryable();
                var entity = TBUNIVERSITYQUERABLE
                    .Where(x => x.ID.ToString() == university.ID.ToString())
                    .ToList()
                    .FirstOrDefault();

                _TBUNIVERSITYRepository.Delete(entity);
                _TBUNIVERSITYRepository.SaveChanges();
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

       
    }
}
