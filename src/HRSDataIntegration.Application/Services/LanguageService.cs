using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBFOREIGN_LANGUAGE> _TBFOREIGN_LANGUAGE;
        private readonly ISqlRepository<Language> _sqlLanguage;

        public LanguageService(IOracleCommon oracleCommon, IOracleRepository<TBFOREIGN_LANGUAGE> TBFOREIGN_LANGUAGE, ISqlRepository<Language> sqlLanguage)
        {
            _oracleCommon = oracleCommon;
            _TBFOREIGN_LANGUAGE = TBFOREIGN_LANGUAGE;
            _sqlLanguage = sqlLanguage;
        }
        public void ConvertToLanguage_Insert_ToOracleTable(string ID) //LanguageId
        {
            try
            {
                var languageQueryable = _sqlLanguage.GetQueryable();
                var language = languageQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Title = x.Title,
                    })
                    .FirstOrDefault();

                var TBFOREIGN_LANGUAGE = new TBFOREIGN_LANGUAGE()
                {
                    ID = language.Id.ToString(),
                    CODE = language.Code,
                    NAME = language.Title
                };

                _TBFOREIGN_LANGUAGE.Create(TBFOREIGN_LANGUAGE);
                _TBFOREIGN_LANGUAGE.SaveChanges();

                _oracleCommon.InsertInto_DataConverter_MappingId(language.Id.ToString(), ID, "HRS.TBFOREIGN_LANGUAGE", "ID", "Employee.Language", "ID");
                _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID, 1226, 8589934592);
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(ID) ,true);
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
        public void ConvertToLanguage_Update_ToOracleTable(string ID)
        {
            try
            {
                var languageQueryable = _sqlLanguage.GetQueryable();
                var language = languageQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Title,
                    })
                    .FirstOrDefault();
                var oldId = _oracleCommon.OldColumnValue("HRS.TBFOREIGN_LANGUAGE", "ID", language.Id.ToString());
                var TBFOREIGN_LANGUAGE_Queryable = _TBFOREIGN_LANGUAGE.GetQueryable();
                var entity = TBFOREIGN_LANGUAGE_Queryable
                    .Where(x => x.ID.ToString() == language.Id.ToString())
                    //  .Where(x=>x.ID.ToString() == oldId)
                    .ToList()
                    .FirstOrDefault();
                entity.CODE = language.Code;
                entity.NAME = language.Name;
                _TBFOREIGN_LANGUAGE.SaveChanges();

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
        public void ConvertToLanguage_Delete_ToOracleTable(string ID)
        {
            try
            {
                var languageQueryable = _sqlLanguage.GetQueryable();
                var language = languageQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Title,
                    })
                    .FirstOrDefault();
                var oldId = _oracleCommon.OldColumnValue("HRS.TBFOREIGN_LANGUAGE", "ID", language.Id.ToString());
                var TBFOREIGN_LANGUAGE_Queryable = _TBFOREIGN_LANGUAGE.GetQueryable();
                var entity = TBFOREIGN_LANGUAGE_Queryable
                    //  .Where(x => x.ID.ToString() == oldId)
                    .Where(x => x.ID.ToString() == language.Id.ToString())
                    .ToList()
                    .FirstOrDefault();

                _TBFOREIGN_LANGUAGE.Delete(entity);
                _TBFOREIGN_LANGUAGE.SaveChanges();
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
