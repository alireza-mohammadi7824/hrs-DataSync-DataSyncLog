using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class PersonEducationService : IPersonEducationService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBPERSONNEL_GRADUATION> _oraclePERSONNEL_GRADUATIONRepository;
        private readonly IOracleRepository<TBPERSONNEL> _oraclePERSONNELRepository;
        private readonly IOracleRepository<TBPERSONNEL_TOTAL> _oraclePERSONNEL_TOTALRepository;
        private readonly ISqlRepository<PersonEducation> _sqlRepositoryPersonEducation;

        public PersonEducationService(IOracleCommon oracleCommon,
            IOracleRepository<TBPERSONNEL_GRADUATION> oraclePERSONNEL_GRADUATIONRepository,
            IOracleRepository<TBPERSONNEL> oraclePERSONNELRepository,
            IOracleRepository<TBPERSONNEL_TOTAL> oraclePERSONNEL_TOTALRepository,
            ISqlRepository<PersonEducation> sqlRepositoryPersonEducation)
        {
            _oracleCommon = oracleCommon;
            _oraclePERSONNELRepository = oraclePERSONNELRepository;
            _oraclePERSONNEL_GRADUATIONRepository= oraclePERSONNEL_GRADUATIONRepository;
            _oraclePERSONNEL_TOTALRepository= oraclePERSONNEL_TOTALRepository;
            _sqlRepositoryPersonEducation= sqlRepositoryPersonEducation;
        }


        public void ConvertToPersonEducationService_Insert_ToOracleTable(string ID) //PersonId
        {
            try
            {
                #region insert into TBPERSONNEL_EDUCATION
                var personEducationQueryable = _sqlRepositoryPersonEducation.GetQueryable();
                var personEducation = personEducationQueryable
                    .Include(x => x.DegreeType)
                    .Include(p => p.Person)
                    .ThenInclude(ed => ed.EmployeeDetails)
                    .ThenInclude(e => e.Employee)
                    .ThenInclude(edu => edu.EmployeeAppointmentUnits)
                    .ThenInclude(u => u.Unit)
                    .ThenInclude(ud => ud.UnitDetails)
                    .Where(x => x.PersonId.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        ccc = x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString(),
                        RECIVE_DATE = _oracleCommon.ToStringDateTime(x.GraduationDate == 0 ? x.Person.EmployeeDetails.Select(y => y.EffectiveDateFrom).FirstOrDefault() : x.GraduationDate),
                        AVERAGE = x.DegreeScore,
                        RECORD_TYPE_CODE = 1,
                        DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                        RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                        PERSONNEL_ID = _oracleCommon.OldColumnValue("HRS.TBPERSONNEL", "ID", x.PersonId.ToString()),
                        DIPLOMA_CODE = _oracleCommon.OldColumnValue("HRS.TBCDIPLOMA", "ID", x.DegreeLevelID.ToString()),
                        EDUCATION_STUDY_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", x.StudyFieldID.ToString()),
                        STATE_CODE = 2,
                        EQUAL_CODE = x.DegreeType.Id.ToString(),
                        NON_CORRESPONDED_TYPE_CODE = _oracleCommon.OldColumnValue("HRS.TBCNON_CORRESPONDED_TYPE", "ID", x.NotRelatedTypeID.ToString()),
                        HAS_THESIS_CODE = x.HasThesis == true ? 1 : 2,
                        UNIVERSITY_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", x.StudyFieldID.ToString()),
                        EDUCATION_BRANCH_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_BRANCH", "ID", x.StudyBranchlID.ToString()),
                        DESCRIPTION = x.Description,
                        CORRESPOND_EXPERT_CODE = x.NotRelatedTypeID == Guid.Empty ? 1 : 0,
                        EXEC_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDate),
                        DIPLOMA_TYPE_CODE = _oracleCommon.OldColumnValue("HRS.DegreeType", "ID", x.DegreeTypeID.ToString()),
                        IS_SUCCESSFUL_CODE = 1,
                    }).FirstOrDefault();

                var TBPERSONNEL_GRADUATION = new TBPERSONNEL_GRADUATION
                {
                    ID = personEducation.ID.ToString(),
                    RECEIVE_DATE = personEducation.RECIVE_DATE,
                    AVERAGE = (long)(personEducation.AVERAGE),
                    RECORD_TYPE_CODE = personEducation.RECORD_TYPE_CODE,
                    DOMAIN_CODE = int.Parse(personEducation.DOMAIN_CODE),
                    RECORD_ACTIVE = int.Parse(personEducation.RECORD_ACTIVE),
                    PERSONNEL_ID = personEducation.PERSONNEL_ID,
                    DIPLOMA_CODE = int.Parse(personEducation.DIPLOMA_CODE),
                    EDUCATION_STUDY_ID = personEducation.EDUCATION_STUDY_ID,
                    STATE_CODE = personEducation.STATE_CODE,
                    EQUAL_CODE = int.Parse(personEducation.EQUAL_CODE),
                    NON_CORRESPONDED_TYPE_CODE = int.Parse(personEducation.NON_CORRESPONDED_TYPE_CODE),
                    HAS_THESIS_CODE = personEducation.HAS_THESIS_CODE,
                    UNIVERSITY_ID = personEducation.UNIVERSITY_ID,
                    EDUCATION_BRANCH_ID = personEducation.EDUCATION_BRANCH_ID,
                    DESCRIPTION = personEducation.DESCRIPTION,
                    CORRESPOND_EXPERT_CODE = personEducation.CORRESPOND_EXPERT_CODE,
                    EXEC_DATE = personEducation.EXEC_DATE,
                    DIPLOMA_TYPE_CODE = int.Parse(personEducation.DIPLOMA_TYPE_CODE)
                };
                _oraclePERSONNEL_GRADUATIONRepository.Create(TBPERSONNEL_GRADUATION);
                _oraclePERSONNEL_GRADUATIONRepository.SaveChanges();

                _oracleCommon.InsertInto_DataConverter_MappingId(personEducation.ID.ToString(), ID, "HRS.TBPERSONNEL_GRADUATION", "Id", "PersonEducation", "Id");
                _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID, 1212, 8589934592);

                #endregion insert into TBPERSONNEL_EDUCATION

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


        public void ConvertToPersonEducationPromotionToOracleTable(string ID)
        {
            try
            {
                var personEducationQueryable = _sqlRepositoryPersonEducation.GetQueryable();
                var personEducation = personEducationQueryable
                    .Include(x => x.DegreeType)
                    .Include(p => p.Person)
                    .ThenInclude(ed => ed.EmployeeDetails)
                    .ThenInclude(e => e.Employee)
                    .ThenInclude(edu => edu.EmployeeAppointmentUnits)
                    .ThenInclude(u => u.Unit)
                    .ThenInclude(ud => ud.UnitDetails)
                    .Where(x => x.PersonId.ToString() == ID)
                    .Select(x => new
                    {
                        ID = x.Id,
                        ccc = x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString(),
                        RECIVE_DATE = _oracleCommon.ToStringDateTime(x.GraduationDate == 0 ? x.Person.EmployeeDetails.Select(y => y.EffectiveDateFrom).FirstOrDefault() : x.GraduationDate),
                        AVERAGE = x.DegreeScore,
                        RECORD_TYPE_CODE = 1,
                        DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                        RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                        PERSONNEL_ID = _oracleCommon.OldColumnValue("HRS.TBPERSONNEL", "ID", x.PersonId.ToString()),
                        DIPLOMA_CODE = _oracleCommon.OldColumnValue("HRS.TBCDIPLOMA", "ID", x.DegreeLevelID.ToString()),
                        EDUCATION_STUDY_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", x.StudyFieldID.ToString()),
                        STATE_CODE = 2,
                        EQUAL_CODE = x.DegreeType.Id.ToString(),
                        NON_CORRESPONDED_TYPE_CODE = _oracleCommon.OldColumnValue("HRS.TBCNON_CORRESPONDED_TYPE", "ID", x.NotRelatedTypeID.ToString()),
                        HAS_THESIS_CODE = x.HasThesis == true ? 1 : 2,
                        UNIVERSITY_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", x.StudyFieldID.ToString()),
                        EDUCATION_BRANCH_ID = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_BRANCH", "ID", x.StudyBranchlID.ToString()),
                        DESCRIPTION = x.Description,
                        CORRESPOND_EXPERT_CODE = x.NotRelatedTypeID == Guid.Empty ? 1 : 0,
                        EXEC_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDate),
                        DIPLOMA_TYPE_CODE = _oracleCommon.OldColumnValue("HRS.DegreeType", "ID", x.DegreeTypeID.ToString()),
                        IS_SUCCESSFUL_CODE = 1,
                    }).FirstOrDefault();
                #region edit TBPERSONEL
                var TBPERSONEL = _oraclePERSONNELRepository.GetQueryable();
                var entityPersonel = TBPERSONEL.Where(x => x.ID == personEducation.ID.ToString()).ToList().FirstOrDefault();
                entityPersonel.AVERAGE = int.Parse(personEducation.AVERAGE.ToString());
                entityPersonel.EDUCATION_END_DATE = personEducation.RECIVE_DATE;
                entityPersonel.DIPLOMA_CODE = int.Parse(personEducation.DIPLOMA_CODE);
                entityPersonel.EDUCATION_BRANCH_ID = personEducation.EDUCATION_BRANCH_ID;
                entityPersonel.EDUCATION_STUDY_ID = personEducation.EDUCATION_STUDY_ID;
                entityPersonel.UNIVERSITY_ID = personEducation.UNIVERSITY_ID;
                _oraclePERSONNELRepository.SaveChanges();
                #endregion edit TBPERSONEL

                #region edit TBPERSONEL_TOTAL
                var TBPERSONEL_TOTAL = _oraclePERSONNEL_TOTALRepository.GetQueryable();
                var entityPersonelTotal = TBPERSONEL_TOTAL.Where(x => x.ID == personEducation.ID.ToString()).ToList().FirstOrDefault();
                //entityPersonelTotal.AVERAGE = int.Parse(personEducation.AVERAGE.ToString());
                //entityPersonelTotal.EDUCATION_END_DATE = personEducation.RECIVE_DATE;
                entityPersonelTotal.DIPLOMA_CODE = int.Parse(personEducation.DIPLOMA_CODE);
                //entityPersonelTotal.EDUCATION_BRANCH_ID = personEducation.EDUCATION_BRANCH_ID;
                //entityPersonelTotal.EDUCATION_STUDY_ID = personEducation.EDUCATION_STUDY_ID;
                //entityPersonelTotal.UNIVERSITY_ID = personEducation.UNIVERSITY_ID;
                _oraclePERSONNEL_TOTALRepository.SaveChanges();
                #endregion edit TBPERSONEL_TOTAL

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
