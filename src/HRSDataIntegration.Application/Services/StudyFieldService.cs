using HRSDataIntegration.DTOs;
using HRSDataIntegration.DTOs.Chart;
using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class StudyFieldService : IStudyFieldService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBEDUCATION_STUDY> _TBEDUCATION_STUDY;
        private readonly ISqlRepository<StudyField> _sqlStudyField;
        private readonly IOracleRepository<TBEDUCATION_BRANCH> _TBEDUCATION_BRANCH;
        private readonly ISqlRepository<StudyBranch> _sqlStudyBranch;
        private readonly ISqlRepository<MappingId> _sqlMappingId;

        public StudyFieldService(IOracleCommon oracleCommon, IOracleRepository<TBEDUCATION_STUDY> TBEDUCATION_STUDY, ISqlRepository<StudyField> sqlStudyField,
            ISqlRepository<StudyBranch> sqlStudyBranch, IOracleRepository<TBEDUCATION_BRANCH> TBEDUCATION_BRANCH)
        {
            _oracleCommon = oracleCommon;
            _TBEDUCATION_STUDY = TBEDUCATION_STUDY;
            _sqlStudyField = sqlStudyField;
            _sqlStudyBranch = sqlStudyBranch;
            _TBEDUCATION_BRANCH = TBEDUCATION_BRANCH;
        }
        public void ConvertToStudyField_Insert_ToOracleTable(string ID) 
        {
            #region insert into studyField
                var sqlStudyFieldQeuryable = _sqlStudyField.GetQueryable();
                var sqlStudyField = sqlStudyFieldQeuryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title,
                        IsRelatedToBank = x.IsRelatedToBank == true ? 1 : 2,
                    }).FirstOrDefault();

                var TBEDUCATION_STUDY = new TBEDUCATION_STUDY()
                {
                    ID = sqlStudyField.ID.ToString(),
                    CODE = sqlStudyField.CODE,
                    NAME = sqlStudyField.TITLE,
                    CORRESPOND_CODE = sqlStudyField.IsRelatedToBank
                };

                _TBEDUCATION_STUDY.Create(TBEDUCATION_STUDY);
                _TBEDUCATION_STUDY.SaveChanges();

                _oracleCommon.InsertInto_DataConverter_MappingId(sqlStudyField.ID.ToString(), ID, "HRS.TBEDUCATION_STUDY", "ID", "Employee.StudyField", "ID");
                _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID, 1220, 8589934592);
            #endregion insert into studyField
            #region insert into studyBranch
            //var sqlStudyBranchQueryable = _sqlStudyBranch.GetQueryable();
            //var sqlStudyBranch = sqlStudyBranchQueryable
            //    .Where(x => x.StudyFieldId.ToString() == ID)
            //    .Select(x => new
            //    {
            //        ID = x.Id,
            //        CODE = x.Code,
            //        TITLE = x.Title,
            //        StudyFieldId = x.StudyFieldId
            //    }).FirstOrDefault();

            //var oldStudyFieldId = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", sqlStudyBranch.StudyFieldId.ToString());

            //var TBEDUCATION_BRANCH = new TBEDUCATION_BRANCH()
            //{
            //    ID = sqlStudyBranch.ID.ToString(),
            //    CODE = sqlStudyBranch.CODE,
            //    NAME = sqlStudyBranch.TITLE,
            //    EDUCATION_STUDY_ID = oldStudyFieldId,
            //};

            //_TBEDUCATION_BRANCH.Create(TBEDUCATION_BRANCH);
            //_TBEDUCATION_BRANCH.SaveChanges();

            //_oracleCommon.InsertInto_DataConverter_MappingId(sqlStudyBranch.ID.ToString(), ID, "TBEDUCATION_BRANCH", "ID", "Employee.StudyBranch", "ID");
            //_oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID, 1220, 8589934592);

            var sqlStudyBranchQueryable = _sqlStudyBranch.GetQueryable();
            var sqlStudyBranch = sqlStudyBranchQueryable
                .Where(x => x.StudyFieldId.ToString() == ID)
                .Select(x => new TBEDUCATION_BRANCH
                {
                    ID = x.Id.ToString(),
                    CODE = x.Code,
                    NAME = x.Title,
                    EDUCATION_STUDY_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", x.StudyFieldId.ToString())
                }).ToList();

            _TBEDUCATION_BRANCH.CreateList(sqlStudyBranch);
            _TBEDUCATION_BRANCH.SaveChanges();
            foreach (var item in sqlStudyBranch)
            {
              _oracleCommon.InsertInto_DataConverter_MappingId(item.ID.ToString(), ID , "TBEDUCATION_BRANCH", "ID", "Employee.StudyBranch", "ID");
            }
            _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID, 1220, 8589934592);
            #endregion insert into studyBranch
        }
        public void ConvertToStudyField_Update_ToOracleTable(string ID)
        {
            #region update field
                var sqlStudyFieldQeuryable = _sqlStudyField.GetQueryable();
                var sqlStudyField = sqlStudyFieldQeuryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title,
                        IsRelatedToBank = x.IsRelatedToBank == true ? 1 : 2,
                    }).FirstOrDefault();

                var oldId = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", sqlStudyField.ID.ToString());

                var TBEDUCATION_STUDY_QUERYABLE = _TBEDUCATION_STUDY.GetQueryable();
                var entity = TBEDUCATION_STUDY_QUERYABLE
                    .Where(x=> x.ID.ToString() == oldId)
                    .ToList()
                    .FirstOrDefault();

                entity.CODE  = sqlStudyField.CODE;
                entity.NAME = sqlStudyField.TITLE;
                entity.CORRESPOND_CODE = sqlStudyField.IsRelatedToBank;
                _TBEDUCATION_STUDY.SaveChanges();
            #endregion update field
            #region update branch
                var sqlStudyBranchQueryable = _sqlStudyBranch.GetQueryable();
                var sqlStudyBranch = sqlStudyBranchQueryable
                    .Where(x => x.StudyFieldId.ToString() == ID)
                    .Select(x => new TBEDUCATION_BRANCH
                    {
                        ID = x.Id.ToString(),
                        CODE = x.Code,
                        NAME = x.Title,
                        EDUCATION_STUDY_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", x.StudyFieldId.ToString())
                    })
                    .ToList();

                var MappingIdQueryable = _sqlMappingId.GetQueryable();
                var mapping = MappingIdQueryable
                .Select(x=> new TBEDUCATION_BRANCH
                { 
                    ID = x.Id.ToString() ,
                    CODE = 0,
                    NAME = x.NewColumnValue ,
                    EDUCATION_STUDY_ID = "0"
                })
                .Where(x => x.NAME == ID)
                .ToList();

            var existInmapping = mapping.Except(sqlStudyBranch, new Comparer()).ToList();
            if (existInmapping.Count > 0)
            {
                _TBEDUCATION_BRANCH.DeleteList(existInmapping);
                _TBEDUCATION_BRANCH.SaveChanges();
            }

            var notExistInMapping = mapping.Except(mapping, new Comparer()).ToList();
            if (notExistInMapping.Count > 0)
            {
                _TBEDUCATION_BRANCH.CreateList(notExistInMapping);
                _TBEDUCATION_BRANCH.SaveChanges();
            }

            //var oldStudyFieldId = _oracleCommon.OldColumnValue("TBEDUCATION_BRANCH", "ID", sqlStudyBranch..ToString());
            //var TBEDUCATION_BRANCH_QUERYABLE = _TBEDUCATION_BRANCH.GetQueryable();
            //var entityBranch = TBEDUCATION_BRANCH_QUERYABLE
            //    .Where(x => x.ID.ToString() == oldStudyFieldId)
            //    .ToList()
            //    .FirstOrDefault();

            //entityBranch.CODE = sqlStudyBranch.CODE;
            //entityBranch.NAME = sqlStudyBranch.TITLE;
            //entityBranch.EDUCATION_STUDY_ID = sqlStudyBranch.STUDY_FIELD_ID.ToString();

            _TBEDUCATION_BRANCH.SaveChanges();
            #endregion update branch

        }
        public void ConvertToStudyField_Delete_ToOracleTable(string ID)
        {
            #region Delete Branch
            var sqlStudyBranchQueryable = _sqlStudyBranch.GetQueryable();
            var sqlStudyBranch = sqlStudyBranchQueryable
                .Where(x => x.StudyFieldId.ToString() == ID)
                .Select(x => new TBEDUCATION_BRANCH
                {
                    ID = x.Id.ToString(),
                    CODE = x.Code,
                    NAME = x.Title,
                    EDUCATION_STUDY_ID = _oracleCommon.OldColumnValue("TBEDUCATION_BRANCH", "ID", x.StudyFieldId.ToString()),
                })
                .ToList();
            _TBEDUCATION_BRANCH.DeleteList(sqlStudyBranch);
            _TBEDUCATION_BRANCH.SaveChanges();
            //   var oldStudyFieldId = _oracleCommon.OldColumnValue("TBEDUCATION_BRANCH", "ID", sqlStudyBranch.STYDY_FIELD_ID.ToString());
            //var TBEDUCATION_BRANCH_QUERYABLE = _TBEDUCATION_BRANCH.GetQueryable();
            //var entityBrach = TBEDUCATION_BRANCH_QUERYABLE
            //    .Where(x => x.ID.ToString() == oldStudyFieldId)
            //    .ToList()
            //    .FirstOrDefault();

            //_TBEDUCATION_BRANCH.Delete(entityBrach);
            //_TBEDUCATION_BRANCH.SaveChanges();
            #endregion Delete Branch
            #region Delete field
            var sqlStudyFieldQeuryable = _sqlStudyField.GetQueryable();
                var sqlStudyField = sqlStudyFieldQeuryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        CODE = x.Code,
                        TITLE = x.Title,
                        IsRelatedToBank = x.IsRelatedToBank == true ? 1 : 2,
                    }).FirstOrDefault();

                var oldId = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", sqlStudyField.ID.ToString());

                var TBEDUCATION_STUDY_QUERYABLE = _TBEDUCATION_STUDY.GetQueryable();
                var entity = TBEDUCATION_STUDY_QUERYABLE
                    .Where(x => x.ID.ToString() == oldId)
                    .ToList()
                    .FirstOrDefault();

                _TBEDUCATION_STUDY.Delete(entity);
                _TBEDUCATION_STUDY.SaveChanges();
            #endregion Delete Field
           
        }
    }

    class Comparer : IEqualityComparer<TBEDUCATION_BRANCH>
    {
        public bool Equals(TBEDUCATION_BRANCH x, TBEDUCATION_BRANCH y)
        {
            return x.ID == y.ID; // مقایسه بر اساس `Id`
        }      

        public int GetHashCode([DisallowNull] TBEDUCATION_BRANCH obj)
        {
            throw new NotImplementedException();
        }
    }
}
