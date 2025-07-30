using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class StudyBranchService : IStudyBranchService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBEDUCATION_BRANCH> _TBEDUCATION_BRANCH;
        private readonly ISqlRepository<StudyBranch> _sqlStudyBranch;
        public void ConvertToStudyBranch_Insert_ToOracleTable(string ID)
        {
            try
            {
                var sqlStudyBranchQueryable = _sqlStudyBranch.GetQueryable();
                var sqlStudyBranch = sqlStudyBranchQueryable
                    .Where(x => x.StudyFieldId.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title,
                        StudyFieldId = x.StudyFieldId
                    }).FirstOrDefault();

                var oldStudyFieldId = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", sqlStudyBranch.StudyFieldId.ToString());

                var TBEDUCATION_BRANCH = new TBEDUCATION_BRANCH()
                {
                    ID = sqlStudyBranch.ID.ToString(),
                    CODE = sqlStudyBranch.CODE,
                    NAME = sqlStudyBranch.TITLE,
                    EDUCATION_STUDY_ID = oldStudyFieldId,
                };

                _TBEDUCATION_BRANCH.Create(TBEDUCATION_BRANCH);
                _TBEDUCATION_BRANCH.SaveChanges();

                _oracleCommon.InsertInto_DataConverter_MappingId(sqlStudyBranch.ID.ToString(), ID, "TBEDUCATION_BRANCH", "ID", "Employee.StudyBranch", "ID");
                _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID, 1220, 8589934592);
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

        public void ConvertToStudyBranch_Update_ToOracleTable(string ID)
        {
            try
            {
                var sqlStudyBranchQueryable = _sqlStudyBranch.GetQueryable();
                var sqlStudyBranch = sqlStudyBranchQueryable
                    .Where(x => x.StudyFieldId.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title,
                        STYDY_FIELD_ID = x.StudyFieldId
                    })
                    .FirstOrDefault();

                var oldStudyFieldId = _oracleCommon.OldColumnValue("TBEDUCATION_BRANCH", "ID", sqlStudyBranch.STYDY_FIELD_ID.ToString());
                var TBEDUCATION_BRANCH_QUERYABLE = _TBEDUCATION_BRANCH.GetQueryable();
                var entity = TBEDUCATION_BRANCH_QUERYABLE
                    .Where(x => x.EDUCATION_STUDY_ID.ToString() == oldStudyFieldId)
                    .ToList()
                    .FirstOrDefault();

                entity.CODE = sqlStudyBranch.CODE;
                entity.NAME = sqlStudyBranch.TITLE;
                entity.EDUCATION_STUDY_ID = sqlStudyBranch.STYDY_FIELD_ID.ToString();
                _TBEDUCATION_BRANCH.SaveChanges();
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
        public void ConvertToStudyBranch_Delete_ToOracleTable(string ID)
        {
            try
            {
                var sqlStudyBranchQueryable = _sqlStudyBranch.GetQueryable();
                var sqlStudyBranch = sqlStudyBranchQueryable
                    .Where(x => x.StudyFieldId.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title,
                        STYDY_FIELD_ID = x.StudyFieldId
                    })
                    .FirstOrDefault();

                var oldStudyFieldId = _oracleCommon.OldColumnValue("TBEDUCATION_BRANCH", "ID", sqlStudyBranch.STYDY_FIELD_ID.ToString());
                var TBEDUCATION_BRANCH_QUERYABLE = _TBEDUCATION_BRANCH.GetQueryable();
                var entity = TBEDUCATION_BRANCH_QUERYABLE
                    .Where(x => x.EDUCATION_STUDY_ID.ToString() == oldStudyFieldId)
                    .ToList()
                    .FirstOrDefault();

                _TBEDUCATION_BRANCH.Delete(entity);
                _TBEDUCATION_BRANCH.SaveChanges();
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
