using HRSDataIntegration.DTOs;
using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using MassTransit.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class PersonContactService : IPersonContactService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBPERSONNEL_MOBILE> _oracleTBPERSONEL_MOBILE;
        private readonly IOracleRepository<TBFAMILY_MOBILE> _oracleTBFAMILY_MOBILE; 
        private readonly IOracleRepository<TBDEPENDENT> _oracleTBDEPENDENT; 
        private readonly ISqlRepository<PersonContact> _sqlPersonContact;
        private readonly ISqlRepository<PersonPersonType> _sqlPersonPersonType;
        private readonly ISqlRepository<EmployeeDetail> _sqlEmployeeDetail;
        private readonly ISqlRepository<EmployeeDependentDetail> _sqlEmployeeDependentDetail;
        private readonly ISqlRepository<MappingId> _sqlMappingId;

        public PersonContactService(
            IOracleCommon oracleCommon,
            IOracleRepository<TBPERSONNEL_MOBILE> oracleTBPERSONEL_MOBILE,
            IOracleRepository<TBFAMILY_MOBILE> oracleTBFAMILY_MOBILE,
            ISqlRepository<PersonContact> sqlPersonContact,
            ISqlRepository<PersonPersonType> sqlPersonPersonType,
            ISqlRepository<MappingId> sqlMappingId
            )
            {
                _oracleCommon =oracleCommon;
                _oracleTBPERSONEL_MOBILE = oracleTBPERSONEL_MOBILE;
                _oracleTBFAMILY_MOBILE= oracleTBFAMILY_MOBILE;
                _sqlPersonContact = sqlPersonContact;
                _sqlPersonPersonType = sqlPersonPersonType;
                _sqlMappingId = sqlMappingId;
            }
        public void ConvertSqlInsertIntoPersoncontactToOracletable(string Id) //PersonId
        {
            try
            {
                var personContactQueryable = _sqlPersonContact.GetQueryable();
                var personPersonTypeQueryable = _sqlPersonPersonType.GetQueryable();
                var personCotact = personContactQueryable
                    .Where(x => x.PersonId.ToString() == Id)
                    .Select(x => new
                    {
                        ID = x.Id.ToString(),
                        PersonId = x.PersonId.ToString(),
                        MOBILE_NUMBER = x.ContactValue,
                        effectiveDateFrom = x.EffectiveDateFrom,
                        FROM_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                        STATE_CODE = 3,
                        DESCRIPTION = x.Description,
                        DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                        RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                        RECORD_TYPE_CODE = 1
                    })
                    .FirstOrDefault();

                var personPersonType = personPersonTypeQueryable
                    .Where(x => x.PersonId.ToString() == personCotact.PersonId &&
                     x.EffectiveDateFrom <= personCotact.effectiveDateFrom && (
                      x.EffectiveDateTo > personCotact.effectiveDateFrom || x.EffectiveDateTo == 0)
                     )
                    .FirstOrDefault();

                var personTypeId = personPersonType.PersonTypeId.ToString();

                if (personTypeId == "518AC95F-BE91-B2D6-61BD-3A19EF598325") // پرسنل Personel
                {
                    var employeeDetailsQueryable = _sqlEmployeeDetail.GetQueryable();
                    var employeeId = employeeDetailsQueryable.Where(x => x.PersonId.ToString() == personCotact.ID && x.EffectiveDateFrom <= personCotact.effectiveDateFrom && (
                      x.EffectiveDateTo > personCotact.effectiveDateFrom || x.EffectiveDateTo == 0)
                     )
                    .FirstOrDefault().EmployeeId;
                    var TBPERSONNEL_MOBILE = new TBPERSONNEL_MOBILE()
                    {

                        ID = personCotact.ID.ToString(),
                        PERSONNEL_ID = _oracleCommon.OldColumnValue("HRS.TBPERSONNEL", "ID", employeeId.ToString()),
                        MOBILE_NUMBER = int.Parse(personCotact.MOBILE_NUMBER),
                        FROM_DATE = personCotact.FROM_DATE,
                        STATE_CODE = personCotact.STATE_CODE,
                        DESCRIPTION = personCotact.DESCRIPTION,
                        DOMAIN_CODE = long.Parse(personCotact.DOMAIN_CODE),
                        RECORD_ACTIVE = long.Parse(personCotact.RECORD_ACTIVE),
                        RECORD_TYPE_CODE = personCotact.RECORD_TYPE_CODE,
                    };

                    _oracleTBPERSONEL_MOBILE.Create(TBPERSONNEL_MOBILE);
                    _oracleTBPERSONEL_MOBILE.SaveChanges();
                    _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 1291, 8589934592);
                    _oracleCommon.InsertInto_DataConverter_MappingId(TBPERSONNEL_MOBILE.ID.ToString(), Id, "HRS.TBPERSONNEL_MOBILE", "ID", "Employee.PersonContact", "ID");

                }
                else if (personTypeId == "57E6E957-28F2-3D23-DF14-3A19EF5AB9E2") // بستگان Family
                {
                    var employeeDependentDetailQueryable = _sqlEmployeeDependentDetail.GetQueryable();
                    var employeeDependentId = employeeDependentDetailQueryable.Where(x => x.PersonId.ToString() == personCotact.ID && x.EffectiveDateFrom <= personCotact.effectiveDateFrom && (
                      x.EffectiveDateTo > personCotact.effectiveDateFrom || x.EffectiveDateTo == 0)
                     )
                    .FirstOrDefault().EmployeeDependentId;

                    var dependentId = _oracleCommon.OldColumnValue("HRS.TBDEPENDENT", "ID", employeeDependentId.ToString());
                    var oracleFamilyId = _oracleTBDEPENDENT.GetQueryable().Where(x => x.ID == dependentId).FirstOrDefault().FAMILY_ID;

                    var tbFamilyMobile = new TBFAMILY_MOBILE()
                    {
                        ID = personCotact.ID.ToString(),
                        FAMILY_ID = oracleFamilyId.ToString(),
                        MOBILE_NUMBER = int.Parse(personCotact.MOBILE_NUMBER),
                        FROM_DATE = personCotact.FROM_DATE,
                        STATE_CODE = personCotact.STATE_CODE,
                        DESCRIPTION = personCotact.DESCRIPTION,
                        DOMAIN_CODE = long.Parse(personCotact.DOMAIN_CODE),
                        RECORD_ACTIVE = long.Parse(personCotact.RECORD_ACTIVE),
                        RECORD_TYPE_CODE = personCotact.RECORD_TYPE_CODE,
                    };
                    _oracleTBFAMILY_MOBILE.Create(tbFamilyMobile);
                    _oracleTBFAMILY_MOBILE.SaveChanges();
                    _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 1292, 8589934592);
                    _oracleCommon.InsertInto_DataConverter_MappingId(tbFamilyMobile.ID.ToString(), Id, "HRS.TBFAMILY_MOBILE", "ID", "Employee.PersonContact", "ID");
                    _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id) , true);
                }
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

        public void ConvertSqlUpdatePersoncontactToOracletable(string Id) //personId
        {
            try
            {
                var PresonType = _sqlMappingId.GetQueryable().Where(x => x.NewColumnValue.ToString() == Id).FirstOrDefault().OldTableName;
                if (PresonType == "HRS.TBPERSONNEL_MOBILE")
                {
                    var personContactQueryable = _sqlPersonContact.GetQueryable();
                    var personContact = personContactQueryable
                        .Where(x => x.Id.ToString() == Id)
                        .Select(x => new
                        {
                            Id = x.Id,
                            MOBILE_NUMBER = x.ContactValue,
                            FROM_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                            DESCRIPTION = x.Description,
                        })
                        .FirstOrDefault();

                    var tbPersonelMobileQueryable = _oracleTBPERSONEL_MOBILE.GetQueryable();
                    var entity = tbPersonelMobileQueryable
                        .Where(x => x.ID == personContact.Id.ToString())
                        .ToList()
                        .FirstOrDefault();

                    entity.MOBILE_NUMBER = int.Parse(personContact.MOBILE_NUMBER);
                    entity.DESCRIPTION = personContact.DESCRIPTION;
                    entity.FROM_DATE = personContact.FROM_DATE;
                    _oracleTBPERSONEL_MOBILE.SaveChanges();
                }
                else if (PresonType == "HRS.TBFAMILY_MOBILE")
                {
                    var personContactQueryable = _sqlPersonContact.GetQueryable();
                    var personContact = personContactQueryable
                        .Where(x => x.Id.ToString() == Id)
                        .Select(x => new
                        {
                            Id = x.Id,
                            MOBILE_NUMBER = x.ContactValue,
                            FROM_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                            DESCRIPTION = x.Description,
                        })
                        .FirstOrDefault();

                    var tbPersonelMobileQueryable = _oracleTBPERSONEL_MOBILE.GetQueryable();
                    var entity = tbPersonelMobileQueryable
                        .Where(x => x.ID == personContact.Id.ToString())
                        .ToList()
                        .FirstOrDefault();

                    entity.MOBILE_NUMBER = int.Parse(personContact.MOBILE_NUMBER);
                    entity.DESCRIPTION = personContact.DESCRIPTION;
                    entity.FROM_DATE = personContact.FROM_DATE;
                    _oracleTBFAMILY_MOBILE.SaveChanges();
                }
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

        public void ConvertSqlDeletePersoncontactToOracletable(string Id)
        {
            try
            {
                var personCotactQueryable = _sqlPersonContact.GetQueryable();
                var personContact = personCotactQueryable.Where(x => x.Id.ToString() == Id).Select(x => new
                {
                    Id = x.Id,
                    MOBILE_NUMBER = x.ContactValue,
                })
                    .FirstOrDefault();
                var tbPersonelMobileQueryable = _oracleTBPERSONEL_MOBILE.GetQueryable();
                var entity = tbPersonelMobileQueryable.Where(x => x.ID == personContact.Id.ToString())
                    .ToList()
                    .FirstOrDefault();

                _oracleTBPERSONEL_MOBILE.Delete(entity);
                _oracleTBPERSONEL_MOBILE.SaveChanges();
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

        } // طبق گفته خانم بلال پور family  نیاز به کانورت ندارند
    }
}
