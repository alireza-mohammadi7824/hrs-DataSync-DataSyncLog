using HRSDataIntegration.DTOs;
using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HRSDataIntegration.Services
{
    public class PersonDetailService : IPersonDetailService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBPERSONNEL> _TBPERSONNEL;
        private readonly IOracleRepository<TBPERSONNEL_TOTAL> _TBPERSONNEL_TOTAL;
        private readonly IOracleRepository<TBFAMILY> _TBFAMILY;
        private readonly IOracleRepository<TBFAMILY_MARRIAGE> _TBFAMILY_MARRAIAGE;
        private readonly IOracleRepository<TBDEPENDENT> _TBDEPENDENT;
        private readonly ISqlRepository<PersonDetail> _sqlPersonDetail;
        private readonly ISqlRepository<EmployeeDetail> _sqlEmployeeDetail;
        private readonly ISqlRepository<EmployeeDependent> _sqlEmployeeDependent;
        private readonly ISqlRepository<CountryDivisionDetail> _sqlRepositoryCountryDivisionDetail;
        private readonly ISqlRepository<EmployeeDependentDetail> _sqlRepositoryEmployeeDependentDetail;
        private readonly ISqlRepository<EmployeeMaritalDetail> _sqlRepositoryEmployeeMaritalDetail;
        private readonly IOracleRepository<TBFAMILY_TOTAL> _TBFAMILY_TOTAL;
        public PersonDetailService(IOracleCommon oracleCommon, IOracleRepository<TBPERSONNEL> TBPERSONNEL,
            ISqlRepository<PersonDetail> sqlPersonDetail, ISqlRepository<EmployeeDetail> sqlEmployeeDetail, ISqlRepository<EmployeeDependent> sqlEmployeeDependent,
            ISqlRepository<CountryDivisionDetail> sqlRepositoryCountryDivisionDetail,
            ISqlRepository<EmployeeDependentDetail> sqlRepositoryEmployeeDependentDetail,
            IOracleRepository<TBFAMILY> TBFAMILY, ISqlRepository<EmployeeMaritalDetail> sqlRepositoryEmployeeMaritalDetail,
            IOracleRepository<TBFAMILY_MARRIAGE> TBFAMILY_MARRAIAGE, IOracleRepository<TBFAMILY_TOTAL> TBFAMILY_TOTAL, IOracleRepository<TBDEPENDENT> TBDEPENDENT,
            IOracleRepository<TBPERSONNEL_TOTAL> TBPERSONNEL_TOTAL)
        {
            _oracleCommon = oracleCommon;
            _TBPERSONNEL = TBPERSONNEL;
            _sqlPersonDetail = sqlPersonDetail;
            _sqlEmployeeDetail = sqlEmployeeDetail;
            _sqlRepositoryEmployeeDependentDetail = sqlRepositoryEmployeeDependentDetail;
            _TBFAMILY = TBFAMILY;
            _sqlRepositoryEmployeeMaritalDetail = sqlRepositoryEmployeeMaritalDetail;
            _TBFAMILY_MARRAIAGE = TBFAMILY_MARRAIAGE;
            _TBFAMILY_TOTAL = TBFAMILY_TOTAL;
            _sqlEmployeeDependent = sqlEmployeeDependent;
            _sqlRepositoryCountryDivisionDetail = sqlRepositoryCountryDivisionDetail;
            _TBDEPENDENT = TBDEPENDENT;
            _TBPERSONNEL_TOTAL = TBPERSONNEL_TOTAL;
        }



        public void ConvertToPersonDetail_Insert_ToOracleTable(string ID) //EmployeeId
        {
            #region Queryables
            var employeeDependentQueryable = _sqlRepositoryEmployeeDependentDetail.GetQueryable();
            var employeeDetailQueryable = _sqlEmployeeDetail.GetQueryable();
            var countryDivisionDetailQueryabel = _sqlRepositoryCountryDivisionDetail.GetQueryable();
            var employeeMaritialDetailQueryable = _sqlRepositoryEmployeeMaritalDetail.GetQueryable();
            #endregion Queryables

            #region insert into TBPERSONEL
            var countryDivisionByPersonId = employeeDetailQueryable.Select(p => new
            {
                CountryDivisionId = p.Person.PersonDetails.FirstOrDefault().BirthCountryDivisionId,
                EffectiveDateFrom = p.Person.PersonDetails.FirstOrDefault().EffectiveDateFrom,
                EffectiveDateTo = p.Person.PersonDetails.FirstOrDefault().EffectiveDateTo,
                //PersonId = p.PersonId,
                EmployeeId = p.EmployeeId
            }).Where(x => x.EmployeeId.ToString() == ID).FirstOrDefault();

            var CountryDivisionDetailCityId = countryDivisionDetailQueryabel.Where(x => x.CountryDivisionId == countryDivisionByPersonId.CountryDivisionId &&
                                                                          x.EffectiveDateFrom <= countryDivisionByPersonId.EffectiveDateFrom &&
                                                                          (x.EffectiveDateTo > countryDivisionByPersonId.EffectiveDateFrom ||
                                                                          x.EffectiveDateTo == 0)).FirstOrDefault();


            var identityCountryDivisionByPersonId = employeeDetailQueryable.Select(p => new
            {
                CountryDivisionId = p.Person.PersonDetails.FirstOrDefault().IdentityCountryDivisionId,
                EffectiveDateFrom = p.EffectiveDateFrom,
                EffectiveDateTo = p.EffectiveDateTo,
                PersonId = p.PersonId,
                EmployeeId = p.EmployeeId
            })
                .Where(x => x.EmployeeId.ToString() == ID)
                .FirstOrDefault();

            var identityCountryDivisionDetailCityId = countryDivisionDetailQueryabel.Where(x => x.CountryDivisionId == countryDivisionByPersonId.CountryDivisionId &&
                                                                          x.EffectiveDateFrom <= countryDivisionByPersonId.EffectiveDateFrom &&
                                                                          (x.EffectiveDateTo > countryDivisionByPersonId.EffectiveDateFrom ||
                                                                          x.EffectiveDateTo == 0)).FirstOrDefault();



            var cityTypeId = CountryDivisionDetailCityId.CountryDivisionTypeId;
            var idCityTypeId = identityCountryDivisionDetailCityId.CountryDivisionTypeId;
            string cityId = "";
            string partId = "";
            string townshipId = "";
            string politicalProvinceId = "";
            string countryId = "";
            string villageId = "";
            string bigVillageId = "";
            #region if city
            if (cityTypeId == Guid.Parse("A576E653-B25E-4DE1-9FDB-26C5F71AE964"))
            {
                cityId = CountryDivisionDetailCityId.CountryDivisionId.ToString();
                var cityParentId = CountryDivisionDetailCityId.CountryDivisionDetailParentId.ToString();
                partId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(cityParentId)).CountryDivisionId.ToString();
                var partParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(cityParentId)).CountryDivisionDetailParentId;
                townshipId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionId.ToString();
                var townshipParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionDetailParentId;
                politicalProvinceId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionId.ToString();
                var politicalProvinceParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionDetailParentId;
                countryId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == politicalProvinceParentId).CountryDivisionId.ToString();
            }
            #endregion if city
            #region if village
            else if (cityTypeId == Guid.Parse("8D827E19-B70D-4C72-8AA6-0FEAF737A78A"))
            {

                villageId = CountryDivisionDetailCityId.CountryDivisionId.ToString();
                var villageParentId = CountryDivisionDetailCityId.CountryDivisionDetailParentId.ToString();
                bigVillageId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(villageParentId)).CountryDivisionId.ToString();
                var bigVillageParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(villageParentId)).CountryDivisionDetailParentId;
                partId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == bigVillageParentId).CountryDivisionId.ToString();
                var partParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == bigVillageParentId).CountryDivisionDetailParentId;
                townshipId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionId.ToString();
                var townshipParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionDetailParentId;
                politicalProvinceId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionId.ToString();
                var politicalProvinceParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionDetailParentId;
                countryId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == politicalProvinceParentId).CountryDivisionId.ToString();
            }
            #endregion if village

            #region ID_CARD_COUNTRY_ID

            string idPartId = "";
            string idTownshipId = "";
            string idPoliticalProvinceId = "";
            string idCountryId = "";
            string idVillageId = "";
            string idBigVillageId = "";
            string idCityId = "";
            if (idCityTypeId == Guid.Parse("A576E653-B25E-4DE1-9FDB-26C5F71AE964"))
            {
                idCityId = CountryDivisionDetailCityId.CountryDivisionId.ToString();
                var idCityParentId = CountryDivisionDetailCityId.CountryDivisionDetailParentId.ToString();
                idPartId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(idCityParentId)).CountryDivisionId.ToString();
                var idPartParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(idCityParentId)).CountryDivisionDetailParentId;
                idTownshipId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idPartParentId).CountryDivisionId.ToString();
                var idTownshipParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idPartParentId).CountryDivisionDetailParentId;
                idPoliticalProvinceId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idTownshipParentId).CountryDivisionId.ToString();
                var idPoliticalProvinceParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idTownshipParentId).CountryDivisionDetailParentId;
                idCountryId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idPoliticalProvinceParentId).CountryDivisionId.ToString();
            }
            #endregion if city
            #region if village
            else if (cityTypeId == Guid.Parse("8D827E19-B70D-4C72-8AA6-0FEAF737A78A"))
            {
                idVillageId = identityCountryDivisionDetailCityId.CountryDivisionId.ToString();
                var idVillageParentId = identityCountryDivisionDetailCityId.CountryDivisionDetailParentId.ToString();
                idBigVillageId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(idVillageParentId)).CountryDivisionId.ToString();
                var idbigVillageParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(idVillageParentId)).CountryDivisionDetailParentId;
                idPartId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idbigVillageParentId).CountryDivisionId.ToString();
                var idPartParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idbigVillageParentId).CountryDivisionDetailParentId;
                townshipId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idPartParentId).CountryDivisionId.ToString();
                var idTownshipParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idPartParentId).CountryDivisionDetailParentId;
                politicalProvinceId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idTownshipParentId).CountryDivisionId.ToString();
                var idPoliticalProvinceParentId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idTownshipParentId).CountryDivisionDetailParentId;
                countryId = countryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == idPoliticalProvinceParentId).CountryDivisionId.ToString();
            }
            #endregion ID_CARD_COUNTRY_ID


            var personDetail = employeeDetailQueryable.
                Where(x => x.EmployeeId.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    NO = x.EmployeeNumber,
                    NAME = x.Person.PersonDetails.FirstOrDefault().NamePersian,
                    FAMILY = x.Person.PersonDetails.FirstOrDefault().FamilyPersian,
                    FATHER_NAME = x.Person.PersonDetails.FirstOrDefault().FatherName,
                    ID_NO = x.Person.PersonDetails.FirstOrDefault().IdentityNo,
                    NATIONAL_ID = x.Person.PersonDetails.FirstOrDefault().NationalCode,
                    BIRTH_DATE = x.Person.PersonDetails.FirstOrDefault().EffectiveDateFrom,
                    RECORD_TYPE_CODE = 1,
                    STATE_CODE = 100,
                    ENTRY_STATUS_CODE = x.EntranceTypeId,
                    ID_CARD_CITY_ID = x.Person.PersonDetails.FirstOrDefault().IdentityCountryDivisionId,
                    BIRTH_CITY_ID = x.Person.PersonDetails.FirstOrDefault().BirthCountryDivisionId,
                    SEX_TYPE_CODE = x.Person.PersonDetails.FirstOrDefault().GenderId,
                    RELIGION_CODE = x.Person.PersonDetails.FirstOrDefault().ReligionId,
                    RELIGIN_BRANCH_DOCE = x.Person.PersonDetails.FirstOrDefault().ReligionBranchId,
                    LATIN_NAME = x.Person.PersonDetails.FirstOrDefault().NameLatin,
                    LATIN_FAMILY = x.Person.PersonDetails.FirstOrDefault().FamilyLatin,
                    ID_SERIAL1 = x.Person.PersonDetails.FirstOrDefault().IdentitySer.Substring(0, 1),
                    ID_SERIAL2 = x.Person.PersonDetails.FirstOrDefault().IdentitySer.Substring(1, 3),
                    ID_SERIAL3 = x.Person.PersonDetails.FirstOrDefault().IdentitySerial,
                    BIRTH_COUNTRY_ID = countryId,
                    BIRHT_POLITICAL_PROVINCE_ID = politicalProvinceId,
                    BIRHT_TOWNSHIP_ID = townshipId,
                    BIRTH_PART_ID = partId,
                    BIRTH_BIGVILLAGE_ID = bigVillageId,
                    BIRTH_VILLAGE_ID = villageId,
                    ID_CARD_COUNTRY_ID = idCountryId,
                    ID_CARD_POLITICAL_PROVICE_ID = idPoliticalProvinceId,
                    ID_CARD_TOWNSHIP_ID = idTownshipId,
                    ID_CARD_PART_ID = idPartId,
                    ID_CARD_BIGVILLAGE_ID = idBigVillageId,
                    ID_CARD_VILLAGE_ID = idVillageId,
                    EDUCATION_STUDY_ID = x.Person.PersonEducations.Where(p => p.GraduationDate.HasValue).OrderByDescending(p => p.GraduationDate).Select(p => p.StudyFieldID.GetValueOrDefault()).FirstOrDefault(),
                    EDUCATION_BRANCH_ID = x.Person.PersonEducations.Where(p => p.GraduationDate.HasValue).OrderByDescending(p => p.GraduationDate).Select(p => p.StudyBranchlID.GetValueOrDefault()).FirstOrDefault(),
                    EDUCATION_END_DATE = x.Person.PersonEducations.Where(p => p.GraduationDate.HasValue).OrderByDescending(p => p.GraduationDate).Select(p => p.GraduationDate.GetValueOrDefault()).FirstOrDefault(),
                    AVERAGE = x.Person.PersonEducations.Where(p => p.GraduationDate.HasValue).OrderByDescending(p => p.GraduationDate).Select(p => p.DegreeScore.GetValueOrDefault()).FirstOrDefault(),
                    DIPLOMA_CODE = x.Person.PersonEducations.Where(p => p.GraduationDate.HasValue).OrderByDescending(p => p.GraduationDate).Select(p => p.DegreeLevelID).FirstOrDefault(),
                    SOLDIER_STATUS_ID = x.Person.PersonMilitaryStatuses.Select(p => p.MilitaryStatusId).FirstOrDefault().ToString() == "00000000-0000-0000-0000-000000000000" ? "edd11f32-5803-3399-1d06-3a166034b76c" : x.Person.PersonMilitaryStatuses.Select(p => p.MilitaryStatusId).FirstOrDefault().ToString(),
                    JOB_START_DATE = x.Person.EmployeeDetails.Select(p => p.EffectiveDateFrom).FirstOrDefault(),
                    DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                    RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                }).FirstOrDefault();

            var oracleCountryID = _oracleCommon.OldColumnValue("HRS.TBCOUNTRY", "ID", personDetail.BIRTH_COUNTRY_ID);
            var oraclePoliticalProvinceId = _oracleCommon.OldColumnValue("HRS.TBPOLITICAL_PROVINCE", "ID", personDetail.BIRHT_POLITICAL_PROVINCE_ID);
            var oracleTownshipId = _oracleCommon.OldColumnValue("TBTOWNSHIP", "ID", personDetail.BIRHT_TOWNSHIP_ID);
            var oraclePartId = _oracleCommon.OldColumnValue("TBPART", "ID", personDetail.BIRTH_PART_ID);
            var oracleBigVillageId = _oracleCommon.OldColumnValue("TBBIG_VILLAGE", "ID", personDetail.BIRTH_BIGVILLAGE_ID);
            var oracleVillageId = _oracleCommon.OldColumnValue("TBVILLAGE", "ID", personDetail.BIRTH_VILLAGE_ID);
            var oracleCityId = _oracleCommon.OldColumnValue("HRS.TBCITY", "ID", personDetail.BIRTH_CITY_ID.ToString());

            var oracleIdCardCountryId = _oracleCommon.OldColumnValue("HRS.TBCOUNTRY", "ID", personDetail.ID_CARD_COUNTRY_ID);
            var oracleIdCardPoliticalProvinceId = _oracleCommon.OldColumnValue("HRS.TBPOLITICAL_PROVINCE", "ID", personDetail.ID_CARD_POLITICAL_PROVICE_ID);
            var oracleIdCardTownshipId = _oracleCommon.OldColumnValue("HRS.TBTOWNSHIP", "ID", personDetail.ID_CARD_TOWNSHIP_ID);
            var oracleIdCardPartId = _oracleCommon.OldColumnValue("TBPART", "ID", personDetail.ID_CARD_PART_ID);
            var oracleIdCardBigVillageId = _oracleCommon.OldColumnValue("TBBIG_VILLAGE", "ID", personDetail.ID_CARD_BIGVILLAGE_ID);
            var oracleIdCardVillageId = _oracleCommon.OldColumnValue("TBVILLAGE", "ID", personDetail.ID_CARD_VILLAGE_ID);
            var oracleIdCardCityId = _oracleCommon.OldColumnValue("HRS.TBCITY", "ID", personDetail.ID_CARD_CITY_ID.ToString());

            var oldEducationStudyId = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_STUDY", "ID", personDetail.EDUCATION_STUDY_ID.ToString());
            var oldEducationBranchId = _oracleCommon.OldColumnValue("HRS.TBEDUCATION_BRANCH", "ID", personDetail.EDUCATION_BRANCH_ID.ToString());
            var oldDiplomaCode = _oracleCommon.OldColumnValue("HRS.TBCDIPLOMA", "CODE", personDetail.DIPLOMA_CODE.ToString() == "cbb5d6e8-9afd-4b0d-8468-9669a52f36f5" ? "220879d9-81d0-591c-33c6-3a1a1453b0c4" : "220879d9-81d0-591c-33c6-3a1a1453b0c4");
            var oldMilitaryStatus = _oracleCommon.OldColumnValue("HRS.TBSOLDIER_STATUS", "CODE", personDetail.SOLDIER_STATUS_ID.ToString());

            var marageTypeCode = employeeDetailQueryable
                .Include(x => x.Person)
                .Include(x => x.Employee)
                .ThenInclude(x => x.EmployeeDependentDetails.Where(z => z.DependentRelationTypeId == Guid.Parse("AAA60627-BFDE-4F22-BA92-2EB090F2CD10")))
                .ThenInclude(x => x.EmployeeDependent)
                .ThenInclude(x => x.EmployeeMaritalDetails)
                .Select(x => new
                {
                    isSingle = x.Person.EmployeeDependentDetails.SelectMany(ed => ed.EmployeeDependent.EmployeeMaritalDetails).All(md => md.EffectiveDateTo != 0) //  تاریخ پایان دارند یعنی مجرد 
                    || !x.Person.EmployeeDependentDetails.SelectMany(ed => ed.EmployeeDependent.EmployeeMaritalDetails).Any() //یا اصلاً رکوردی وجود نداشته باشه 
                     == true ? "81de07ea-fdf1-4906-82a6-3a16c0e52ca0" : "50fe8fc8-3feb-5762-4d79-3a16c0e517da"
                }).FirstOrDefault();


            var tbPersonel = new TBPERSONNEL()
            {
                ID = personDetail.ID.ToString(),
                NO = personDetail.NO,
                NAME = personDetail.NAME,
                FAMILY = personDetail.FAMILY,
                FATHER_NAME = personDetail.FATHER_NAME,
                ID_NO = personDetail.ID_NO,
                NATIONAL_ID = personDetail.NATIONAL_ID,
                BIRTH_DATE = _oracleCommon.ToStringDateTime(personDetail.BIRTH_DATE),
                EDUCATION_END_DATE = _oracleCommon.ToStringDateTime(personDetail.EDUCATION_END_DATE),
                JOB_START_DATE = _oracleCommon.ToStringDateTime(personDetail.JOB_START_DATE),
                RECORD_TYPE_CODE = 1,
                DOMAIN_CODE = long.Parse(personDetail.DOMAIN_CODE),
                RECORD_ACTIVE = long.Parse(personDetail.RECORD_ACTIVE),
                STATE_CODE = 3,
                ENTRY_STATUS_CODE = int.Parse(_oracleCommon.OldColumnValue("HRS.TBCENTRY_STATUS", "CODE", personDetail.ENTRY_STATUS_CODE.ToString())),
                ID_CARD_CITY_ID = oracleIdCardCityId,
                BIRTH_CITY_ID = oracleCityId,
                DIPLOMA_CODE = int.Parse(oldDiplomaCode),
                EDUCATION_BRANCH_ID = oldEducationStudyId,
                EDUCATION_STUDY_ID = oldEducationStudyId,
                SEX_TYPE_CODE = int.Parse(_oracleCommon.OldColumnValue("HRS.TBCSEX_TYPE", "CODE", personDetail.SEX_TYPE_CODE.ToString())),
                MARRIAGE_TYPE_CODE = int.Parse(_oracleCommon.OldColumnValue("HRS.TBCMARRIAGE_TYPE", "CODE", marageTypeCode.isSingle.ToString())),     //1 motaahel -- 2mojarad                     //    <= پرسیده شود
                SOLDIER_STATUS_ID = oldMilitaryStatus,
                HAS_BAD_HEALTH_CODE = 2,
                RELIGION_CODE = int.Parse(_oracleCommon.OldColumnValue("HRS.TBCRELIGION", "CODE", personDetail.RELIGION_CODE.ToString())),
                RELIGIN_BRANCH_CODE = 1,     // int.Parse(_oracleCommon.OldColumnValue("HRS.TBCRELIGION_BRANCH" , "ID" , personDetail.RELIGIN_BRANCH_DOCE.ToString())),
                LATIN_NAME = personDetail.LATIN_NAME == "" ? "" : personDetail.LATIN_NAME,
                LATIN_FAMILY = personDetail.LATIN_FAMILY == "" ? "" : personDetail.LATIN_FAMILY,
                ID_SERIAL1 = personDetail.ID_SERIAL1,
                ID_SERIAL2 = personDetail.ID_SERIAL2,
                ID_SERIAL3 = personDetail.ID_SERIAL3,
                BIRTH_COUNTRY_ID = oracleCountryID,
                BIRTH_POLITICAL_PROVINCE_ID = oraclePoliticalProvinceId,
                BIRTH_TOWNSHIP_ID = oracleTownshipId,
                BIRTH_PART_ID = oraclePartId,
                BIRTH_BIGVILLAGE_ID = oracleBigVillageId,
                BIRTH_VILLAGE_ID = oracleVillageId,
                ID_CARD_COUNTRY_ID = oracleIdCardCountryId,
                ID_CARD_POLITICAL_PROVINCE_ID = oracleIdCardPoliticalProvinceId,
                ID_CARD_TOWNSHIP_ID = oracleIdCardTownshipId,
                ID_CARD_PART_ID = oracleIdCardPartId,
                ID_CARD_BIGVILLAGE_ID = oracleIdCardBigVillageId,
                ID_CARD_VILLAGE_ID = oracleIdCardVillageId,
            };

            _TBPERSONNEL.Create(tbPersonel);
            _TBPERSONNEL.SaveChanges();


            #endregion insert into TBPERSONNEL
            #region insert to TBPERSONNEL_TOTAL
            var oldEntryStausCode = _oracleCommon.OldColumnValue("HRS.TBCENTRY_STATUS", "CODE", personDetail.ENTRY_STATUS_CODE.ToString());
            var TBPERSONNEL_TOTAL = new TBPERSONNEL_TOTAL()
            {
                ID = personDetail.ID.ToString(),
                NO = personDetail.NO,
                NAME = personDetail.NAME,
                FAMILY = personDetail.FAMILY,
                FATHER_NAME = personDetail.FATHER_NAME,
                LATIN_NAME = personDetail.LATIN_NAME,
                LATIN_FAMILY = personDetail.LATIN_FAMILY,
                ID_NO = personDetail.ID_NO,
                NATIONAL_ID = personDetail.NATIONAL_ID,
                BIRTH_DATE = _oracleCommon.ToStringDateTime(personDetail.BIRTH_DATE),
                RECORD_TYPE_CODE = 1,
                ENTRY_STATUS_CODE = int.Parse(oldEntryStausCode),
                DOMAIN_CODE = long.Parse(personDetail.DOMAIN_CODE),
                RECORD_ACTIVE = long.Parse(personDetail.DOMAIN_CODE),
                STATE_CODE = 3,
                ID_CARD_CITY_ID = oracleIdCardCityId,
                BIRTH_CITY_ID = oracleCityId,
                DIPLOMA_CODE = int.Parse(oldDiplomaCode),
                SEX_TYPE_CODE = int.Parse(_oracleCommon.OldColumnValue("HRS.TBCSEX_TYPE", "CODE", personDetail.SEX_TYPE_CODE.ToString())),
                MARRIAGE_TYPE_CODE = int.Parse(_oracleCommon.OldColumnValue("HRS.TBCMARRIAGE_TYPE", "CODE", marageTypeCode.isSingle.ToString())),
                ID_SERIAL1 = personDetail.ID_SERIAL1,
                ID_SERIAL2 = personDetail.ID_SERIAL2,
                ID_SERIAL3 = personDetail.ID_SERIAL3,
                JOB_START_DATE = _oracleCommon.ToStringDateTime(personDetail.JOB_START_DATE),
            };
            _TBPERSONNEL_TOTAL.Create(TBPERSONNEL_TOTAL);
            _TBPERSONNEL_TOTAL.SaveChanges();
            #endregion insert to TBPERSONNEL_TOTAL
            #region insert into TBFAMILY
            var employeeDependent = employeeDependentQueryable
                .Include(x => x.EmployeeDependentStatus)
                .Where(x => x.EmployeeId.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    FAMILY_ID = x.EmployeeDependentId,
                    NO = x.EmployeeDependentNumber,
                    NAME = x.Person.PersonDetails.Select(p => p.NamePersian).FirstOrDefault(),
                    FAMILY = x.Person.PersonDetails.Select(p => p.FamilyPersian).FirstOrDefault(),
                    FATHER_NAME = x.Person.PersonDetails.Select(p => p.FatherName).FirstOrDefault(),
                    ID_NO = x.Person.PersonDetails.Select(p => p.IdentityNo).FirstOrDefault(),
                    NATINAL_ID = x.Person.PersonDetails.Select(p => p.NationalCode).FirstOrDefault(),
                    BIRTH_DATE = x.Person.PersonDetails.Select(p => p.EffectiveDateFrom).FirstOrDefault(),
                    DEATH_DATE = x.Person.PersonDetails.Select(p => p.EffectiveDateTo).FirstOrDefault(),
                    RECORD_TYPE_CODE = 1,
                    DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                    RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                    PERSONNEL_ID = x.EmployeeId,
                    STATE_CODE = 2,
                    TYPE_CODE = _oracleCommon.OldColumnValue("HRS.TBCFAMILY_TYPE", "CODE", x.DependentRelationTypeId.ToString()),
                    CITY_ID = oracleCityId,
                    ID_SERIAL1 = x.Person.PersonDetails.Select(p => p.IdentitySer).FirstOrDefault().Substring(0, 1),
                    ID_SERIAL2 = x.Person.PersonDetails.Select(p => p.IdentitySer).FirstOrDefault().Substring(1, 3),
                    ID_SERIAL3 = x.Person.PersonDetails.Select(p => p.IdentitySerial).FirstOrDefault(),
                    EXIST_DESCRIPTION = x.Description,
                    EMPLOYEE_DEPENDENT_STATUS_ID = x.EmployeeDependentStatus.Id
                }).FirstOrDefault();

            var oraclePersonelId = _oracleCommon.OldColumnValue("HRS.TBPERSONNEL", "ID", employeeDependent.PERSONNEL_ID.ToString());
            var oracleStateCode = _oracleCommon.OldColumnValue("HRS.TBCFAMILY_STATE", "ID", employeeDependent.STATE_CODE.ToString());



            var TBFAMILY = new TBFAMILY()
            {
                ID = employeeDependent.ID.ToString(),
                NO = employeeDependent.NO,
                NAME = employeeDependent.NAME,
                FAMILY = employeeDependent.FAMILY,
                FATHER_NAME = employeeDependent.FATHER_NAME,
                ID_NO = employeeDependent.ID_NO,
                NATIONAL_ID = employeeDependent.NATINAL_ID,
                BIRTH_DATE = _oracleCommon.ToStringDateTime(employeeDependent.BIRTH_DATE),
                DEATH_DATE = employeeDependent.DEATH_DATE == 0 ? null : _oracleCommon.ToStringDateTime(employeeDependent.DEATH_DATE),
                RECORD_TYPE_CODE = employeeDependent.RECORD_TYPE_CODE,
                DOMAIN_CODE = long.Parse(personDetail.DOMAIN_CODE),
                RECORD_ACTIVE = long.Parse(personDetail.RECORD_ACTIVE),
                PERSONNEL_ID = oraclePersonelId,
                STATE_CODE = 2,//int.Parse(oracleStateCode),
                TYPE_CODE = int.Parse(employeeDependent.TYPE_CODE),
                CITY_ID = employeeDependent.CITY_ID,
                ID_SERIAL1 = employeeDependent.ID_SERIAL1,
                ID_SERIAL2 = employeeDependent.ID_SERIAL2,
                ID_SERIAL3 = employeeDependent.ID_SERIAL3,
            };

            _TBFAMILY.Create(TBFAMILY);
            _TBFAMILY.SaveChanges();

            #endregion insert into TBFAMILY
            #region insert into TBFAMILY_MARRIAGE
            var employeeMaritalDetail = employeeDependentQueryable
                .Where(emp => emp.Id == employeeDependent.ID)
                .Select(x => new
                {
                    ID = x.Id,
                    MARRIAGE_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                    MARRIAGE_DESCRIPTION = x.Description,
                    DIVORCE_DATE = x.EffectiveDateTo == 0 ? null : _oracleCommon.ToStringDateTime(x.EffectiveDateTo),
                    DIVORCE_DESCRIPTION = x.Description,
                    RECORD_TYPE_CODE = 1,
                    DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                    RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId))).FirstOrDefault().ToString()),
                    FAMILY_ID = x.EmployeeDependentId,
                    STATE_CODE = 2,
                })
                .FirstOrDefault();

            var oldFamilyId = _oracleCommon.OldColumnValue("HRS.TBFAMILY", "ID", employeeMaritalDetail.FAMILY_ID.ToString());
            var TBFAMILY_MARRAIAGE = new TBFAMILY_MARRIAGE()
            {
                ID = employeeMaritalDetail.ID.ToString(),
                MARRIAGE_DATE = employeeMaritalDetail.MARRIAGE_DATE,
                MARRIAGE_DESCRIPTION = employeeMaritalDetail.MARRIAGE_DESCRIPTION,
                DIVORCE_DATE = employeeMaritalDetail.DIVORCE_DATE,
                DIVORCE_DESCRIPTION = employeeMaritalDetail.DIVORCE_DESCRIPTION,
                RECORD_TYPE_CODE = employeeMaritalDetail.RECORD_TYPE_CODE,
                DOMAIN_CODE = long.Parse(personDetail.DOMAIN_CODE),
                RECORD_ACTIVE = long.Parse(personDetail.RECORD_ACTIVE),
                FAMILY_ID = oldFamilyId.ToString(),
                STATE_CODE = employeeMaritalDetail.STATE_CODE
            };
            _TBFAMILY_MARRAIAGE.Create(TBFAMILY_MARRAIAGE);
            _TBFAMILY_MARRAIAGE.SaveChanges();

            #endregion insert into TBFAMILY_MARRIAGE
            var oldEmployeeDependentStatus = _oracleCommon.OldColumnValue("HRS.", "ID", employeeDependent.EMPLOYEE_DEPENDENT_STATUS_ID.ToString());
            #region insert into TBDEPENDENT
            var tbDependent = new TBDEPENDENT()
            {
                ID = employeeDependent.ID.ToString(),
                FROM_DATE = _oracleCommon.ToStringDateTime(employeeDependent.BIRTH_DATE),
                EXIT_DATE = employeeDependent.DEATH_DATE == 0 ? null : _oracleCommon.ToStringDateTime(employeeDependent.DEATH_DATE),
                RECORD_TYPE_CODE = employeeDependent.RECORD_TYPE_CODE,
                DOMAIN_CODE = long.Parse(personDetail.DOMAIN_CODE),
                RECORD_ACTIVE = long.Parse(personDetail.RECORD_ACTIVE),
                STATE_CODE = 2,
                NON_DEPENDENT_REASON_ID = oldEmployeeDependentStatus,
                FAMILY_ID = oldFamilyId.ToString()
            };
            _TBDEPENDENT.Create(tbDependent);
            _TBDEPENDENT.SaveChanges();
            #endregion insert into TBDEPENDENT

            _oracleCommon.InsertInto_DataConverter_MappingId(personDetail.ID.ToString(), ID, "HRS.TBPERSONNEL", "ID", "Employee.Employee", "ID");
            _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID, 1200, 8589934592);

        }

        public void ConvertDivorceRegistrationProcessForPersonelToOracleTable(string ID) //EmployeeMaritalDetailId
        {
            var employeeMaritalDetailQueryable = _sqlRepositoryEmployeeMaritalDetail.GetQueryable();
            var employeeMaritalDetail = employeeMaritalDetailQueryable
                .Where(x => x.Id == Guid.Parse(ID))
                .Select(x => new
                {
                    EffectiveDateTo = _oracleCommon.ToStringDateTime(x.EffectiveDateTo),
                    EffectiveDateFrom = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                    Description = x.Description
                })
                .FirstOrDefault();
            var oldFamilyId = _oracleCommon.OldColumnValue("HRS.TBFAMILY", "ID", ID);

            var TBFAMILY_MARRIAGEQueryable = _TBFAMILY_MARRAIAGE.GetQueryable();
            var entityMarriage = TBFAMILY_MARRIAGEQueryable
                .Where(x => x.FAMILY_ID == oldFamilyId && x.MARRIAGE_DATE == employeeMaritalDetail.EffectiveDateFrom)
                // .OrderByDescending(x=>x.ID)
                .ToList()
                .FirstOrDefault();
            entityMarriage.DIVORCE_DATE = employeeMaritalDetail.EffectiveDateTo;
            entityMarriage.DIVORCE_DESCRIPTION = employeeMaritalDetail.Description;

            //var employeeDetailQueryable = _sqlEmployeeDetail.GetQueryable();
            //var personDetailList = employeeDetailQueryable
            //    .Include(x => x.Person)
            //    .ThenInclude(xp => xp.EmployeeDependentDetails)
            //    .ThenInclude(x => x.EmployeeDependent)
            //    .ThenInclude(z => z.EmployeeMaritalDetails)
            //    .Where(z => z.Employee.)
            //    .Select(x => new
            //    {
            //        EffectiveDateTo = _oracleCommon.ToStringDateTime(x.Person.PersonDetails.FirstOrDefault().EffectiveDateTo),
            //        EffectiveDateFrom = _oracleCommon.ToStringDateTime(x.Person.PersonDetails.FirstOrDefault().EffectiveDateFrom)
            //    })
            //    .FirstOrDefault();

            //var oldFamilyId = _oracleCommon.OldColumnValue("HRS.TBFAMILY", "ID", ID);

            //var TBFAMILY_MARRIAGEQueryable = _TBFAMILY_MARRAIAGE.GetQueryable();
            //var entityMarriage = TBFAMILY_MARRIAGEQueryable
            //    .Where(x=>x.FAMILY_ID == oldFamilyId && x.MARRIAGE_DATE == employeeMaritalDetail.EffectiveDateFrom)
            //   // .OrderByDescending(x=>x.ID)
            //    .ToList()
            //    .FirstOrDefault();
            //var entityPersonelQueryable = _TBPERSONNEL.GetQueryable();
            //var entityPersonel = entityPersonelQueryable.Where(x=>x.ID == oldFamilyId)
            //    .ToList()
            //    .FirstOrDefault();
            //var entityPersonelTotalQeuryable = _TBPERSONNEL_TOTAL.GetQueryable();
            //var entityPersonelTotal = entityPersonelTotalQeuryable.Where(x=>x.ID == oldFamilyId)
            //    .ToList()
            //    .FirstOrDefault();
            //entityMarriage.DIVORCE_DATE = employeeMaritalDetail.EffectiveDateTo;

            //if (personDetailList.All(x => x.EffectiveDateTo != 0))
            //{
            //    //entityPersonel.MARRIAGE_TYPE_CODE = 2;
            //    //entityPersonelTotal.MARRIAGE_TYPE_CODE = 2;  } <=  با هماهنگی آقای شمس الدین
            //}
        }

        public void ConvertPersonelMarriageProcessToOracleTable(string ID) // EmployeeMaritalDetailId
        {
            var employeeMaritalDetailQueryable = _sqlRepositoryEmployeeMaritalDetail.GetQueryable();
            var employeeMaritalDetail = employeeMaritalDetailQueryable
                .Include(x => x.EmployeeDependent)
                .ThenInclude(x => x.EmployeeDependentDetails)
                .ThenInclude(x => x.Person)
                .ThenInclude(x => x.PersonDetails)
                .Where(x => x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Id),
                    EmployeeId = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EmployeeId).FirstOrDefault(),
                    NO = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EmployeeDependentNumber),
                    MarriageDate = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EffectiveDateFrom).FirstOrDefault()),
                    DivorceDate = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EffectiveDateTo).FirstOrDefault()),
                    DivorceDescription = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Description).FirstOrDefault(),
                    Name = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.NamePersian).FirstOrDefault()).FirstOrDefault(),
                    Family = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.FamilyPersian).FirstOrDefault()).FirstOrDefault(),
                    FatherName = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.FatherName).FirstOrDefault()).FirstOrDefault(),
                    Id_No = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentityNo).FirstOrDefault()).FirstOrDefault(),
                    CodeMelli = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.NationalCode).FirstOrDefault()).FirstOrDefault(),
                    Birth_Date = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.SelectMany(x => x.Person.PersonDetails.Select(x => x.EffectiveDateFrom)).FirstOrDefault()),
                    Death_Date = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.SelectMany(x => x.Person.PersonDetails.Select(x => x.EffectiveDateTo)).FirstOrDefault()),
                    Record_Type_Code = 1,
                    Domain_Code = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.EmployeeDependent.EmployeeDependentDetails.SelectMany(z => z.Person.EmployeeDetails).SelectMany(ed => ed.Employee.EmployeeAppointmentUnits).SelectMany(eau => eau.Unit.UnitDetails).Select(ud => ud.ProvinceId).FirstOrDefault().ToString()),
                    Record_Active = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.EmployeeDependent.EmployeeDependentDetails.SelectMany(z => z.Person.EmployeeDetails).SelectMany(ed => ed.Employee.EmployeeAppointmentUnits).SelectMany(eau => eau.Unit.UnitDetails).Select(ud => ud.ProvinceId).FirstOrDefault().ToString()),
                    Personel_Id = _oracleCommon.OldColumnValue("HRS.TBPERSONNEL", "ID", x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Id).ToString()),
                    state_code = 2,
                    type_code = _oracleCommon.OldColumnValue("HRS.TBCFAMILY_TYPE", "ID", x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.DependentRelationTypeId).ToString()),
                    city_ID = _oracleCommon.OldColumnValue("HRS.TBCITY", "ID", x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.BirthCountryDivisionId)).ToString()),
                    Id_Serial1 = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySer)).FirstOrDefault(),
                    Id_Serial2 = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySer)).FirstOrDefault(),
                    Id_Serial3 = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySerial)).FirstOrDefault(),
                    Exit_Description = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Description).ToString(),
                }).FirstOrDefault();

            var tBFAMILYQueryable = _TBFAMILY.GetQueryable();
            var entityFamily = tBFAMILYQueryable
                .Where(x => x.STATE_CODE == 2 && x.NATIONAL_ID == employeeMaritalDetail.CodeMelli.ToString() && x.PERSONNEL_ID == employeeMaritalDetail.EmployeeId.ToString())
                .Select(x => new { Id = x.ID })
                .ToList()
                .FirstOrDefault();

            if (entityFamily.Id == null)
            {
                #region insert into TBFAMILY
                var TBFAMILY = new TBFAMILY()
                {
                    ID = employeeMaritalDetail.ID.ToString(),
                    NO = employeeMaritalDetail.NO.ToString(),
                    NAME = employeeMaritalDetail.Name,
                    FAMILY = employeeMaritalDetail.Family,
                    FATHER_NAME = employeeMaritalDetail.FatherName,
                    ID_NO = employeeMaritalDetail.Id_No,
                    NATIONAL_ID = employeeMaritalDetail.CodeMelli,
                    BIRTH_DATE = employeeMaritalDetail.Birth_Date,
                    DEATH_DATE = employeeMaritalDetail.Death_Date,
                    RECORD_TYPE_CODE = 1,
                    DOMAIN_CODE = int.Parse(employeeMaritalDetail.Domain_Code),
                    RECORD_ACTIVE = int.Parse(employeeMaritalDetail.Record_Active),
                    PERSONNEL_ID = employeeMaritalDetail.Personel_Id,
                    STATE_CODE = 2,
                    TYPE_CODE = int.Parse(employeeMaritalDetail.type_code),
                    CITY_ID = employeeMaritalDetail.city_ID,
                    ID_SERIAL1 = employeeMaritalDetail.Id_Serial1.ToString(),
                    ID_SERIAL2 = employeeMaritalDetail.Id_Serial2.ToString(),
                    ID_SERIAL3 = employeeMaritalDetail.Id_Serial3.ToString(),
                };
                _TBFAMILY.Create(TBFAMILY);
                _TBFAMILY.SaveChanges();
                #endregion insert into TBFAMILY 
                #region insert into TBFAMILY_TOTAL
                var tbFamily_Total = new TBFAMILY_TOTAL()
                {
                    ID = employeeMaritalDetail.ID.ToString(),
                    NO = employeeMaritalDetail.NO.ToString(),
                    NAME = employeeMaritalDetail.Name,
                    FAMILY = employeeMaritalDetail.Family,
                    FATHER_NAME = employeeMaritalDetail.FatherName,
                    ID_NO = employeeMaritalDetail.Id_No,
                    NATIONAL_ID = employeeMaritalDetail.CodeMelli,
                    BIRTH_DATE = employeeMaritalDetail.Birth_Date,
                    DEATH_DATE = employeeMaritalDetail.Death_Date,
                    RECORD_TYPE_CODE = 1,
                    DOMAIN_CODE = int.Parse(employeeMaritalDetail.Domain_Code),
                    RECORD_ACTIVE = int.Parse(employeeMaritalDetail.Record_Active),
                    PERSONNEL_ID = employeeMaritalDetail.Personel_Id,
                    STATE_CODE = 2,
                    TYPE_CODE = int.Parse(employeeMaritalDetail.type_code),
                    CITY_ID = employeeMaritalDetail.city_ID,
                    ID_SERIAL1 = employeeMaritalDetail.Id_Serial1.ToString(),
                    ID_SERIAL2 = employeeMaritalDetail.Id_Serial2.ToString(),
                    ID_SERIAL3 = employeeMaritalDetail.Id_Serial3.ToString(),
                };
                _TBFAMILY_TOTAL.Create(tbFamily_Total);
                _TBFAMILY_TOTAL.SaveChanges();
                #endregion insert into TBFAMILY_TOTAL
                #region insert into TBFamily_Marriage

                var oldFamilyIdMariageId = _oracleCommon.OldColumnValue("HRS.TBFAMILY_MARIAGE", "ID", employeeMaritalDetail.EmployeeId.ToString());
                var oldFamilyId = _TBFAMILY_MARRAIAGE.GetQueryable().Where(x => x.ID == oldFamilyIdMariageId).FirstOrDefault().FAMILY_ID;
                var TBFAMILY_MARRIAGE = new TBFAMILY_MARRIAGE()
                {
                    ID = employeeMaritalDetail.ID.ToString(),
                    MARRIAGE_DATE = employeeMaritalDetail.MarriageDate,
                    MARRIAGE_DESCRIPTION = employeeMaritalDetail.DivorceDescription,
                    DIVORCE_DATE = employeeMaritalDetail.DivorceDate,
                    DIVORCE_DESCRIPTION = employeeMaritalDetail.DivorceDescription,
                    RECORD_TYPE_CODE = 1,
                    DOMAIN_CODE = int.Parse(employeeMaritalDetail.Domain_Code),
                    RECORD_ACTIVE = int.Parse(employeeMaritalDetail.Record_Active),
                    FAMILY_ID = oldFamilyId, //TBFAMILY.ID,                         //<=============================================================
                    STATE_CODE = 2,
                };
                _TBFAMILY_MARRAIAGE.Create(TBFAMILY_MARRIAGE);
                _TBFAMILY_MARRAIAGE.SaveChanges();
                #endregion insert into TBFamily_Marriage

            }
            else
            {
                var TBFAMILY_MARRIAGE = new TBFAMILY_MARRIAGE()
                {
                    ID = employeeMaritalDetail.ID.ToString(),
                    MARRIAGE_DATE = employeeMaritalDetail.MarriageDate,
                    MARRIAGE_DESCRIPTION = employeeMaritalDetail.DivorceDescription,
                    DIVORCE_DATE = employeeMaritalDetail.DivorceDate,
                    DIVORCE_DESCRIPTION = employeeMaritalDetail.DivorceDescription,
                    RECORD_TYPE_CODE = 1,
                    DOMAIN_CODE = int.Parse(employeeMaritalDetail.Domain_Code),
                    RECORD_ACTIVE = int.Parse(employeeMaritalDetail.Record_Active),
                    FAMILY_ID = entityFamily.Id,
                    STATE_CODE = 2,
                };
                _TBFAMILY_MARRAIAGE.Create(TBFAMILY_MARRIAGE);
                _TBFAMILY_MARRAIAGE.SaveChanges();
            }

        }

        public void ConvertProcessOfRegisteringDeathFamilyOfPersonelToOracleTable(string ID) // EmployeeDependentId
        {
            var employeeMaritalDependentQueryable = _sqlEmployeeDependent.GetQueryable();
            var employeeMaritalDetail = employeeMaritalDependentQueryable
                .Include(x => x.EmployeeDependentDetails)
                .ThenInclude(x => x.Person)
                .ThenInclude(x => x.PersonDetails)
                .Where(x => x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    CodeMelli = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.NationalCode).FirstOrDefault()).FirstOrDefault(),
                    DateTo = _oracleCommon.ToStringDateTime(x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.EffectiveDateTo).FirstOrDefault()).FirstOrDefault()),
                }).FirstOrDefault();
            #region update tbfamily
            var TBFAMILYQueryable = _TBFAMILY.GetQueryable();
            var entityFamily = TBFAMILYQueryable
                .Where(x => x.STATE_CODE == 3 && x.NATIONAL_ID == employeeMaritalDetail.CodeMelli.ToString())
                .ToList()
                .FirstOrDefault();

            entityFamily.DEATH_DATE = employeeMaritalDetail.DateTo;
            entityFamily.STATE_CODE = 3;

            _TBFAMILY.SaveChanges();
            #endregion update tbFAmily

            #region update tbFamilyTotal
            var tbFamilyTotalQueryable = _TBFAMILY_TOTAL.GetQueryable();
            var entityFamilyTotal = tbFamilyTotalQueryable.Where(x => x.STATE_CODE == 3 && x.NATIONAL_ID == employeeMaritalDetail.CodeMelli.ToString())
                .ToList()
                .FirstOrDefault();
            entityFamilyTotal.DEATH_DATE = employeeMaritalDetail.DateTo;
            entityFamilyTotal.STATE_CODE = 3;

            _TBFAMILY_TOTAL.SaveChanges();
            #endregion update tbFamilyTotal
        }

        public void ConvertProcessOfRegistrationBirthOfEmployeesChildrean(string ID) // EmployeeDependentId
        {
            var employeeDependentQueryable = _sqlEmployeeDependent.GetQueryable();
            var employeeMaritalDetail = employeeDependentQueryable
                .Include(x => x.EmployeeDependentDetails)
                .ThenInclude(x => x.Person)
                .ThenInclude(x => x.PersonDetails)
                .Where(x => x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.EmployeeDependentDetails.Select(x => x.Id),
                    NO = x.EmployeeDependentDetails.Select(x => x.EmployeeDependentNumber),
                    MarriageDate = _oracleCommon.ToStringDateTime(x.EmployeeDependentDetails.Select(x => x.EffectiveDateFrom).FirstOrDefault()),
                    DivorceDate = _oracleCommon.ToStringDateTime(x.EmployeeDependentDetails.Select(x => x.EffectiveDateTo).FirstOrDefault()),
                    DivorceDescription = x.EmployeeDependentDetails.Select(x => x.Description).FirstOrDefault(),
                    Name = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.NamePersian).FirstOrDefault()).FirstOrDefault(),
                    Family = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.FamilyPersian).FirstOrDefault()).FirstOrDefault(),
                    FatherName = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.FatherName).FirstOrDefault()).FirstOrDefault(),
                    Id_No = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentityNo).FirstOrDefault()).FirstOrDefault(),
                    CodeMelli = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.NationalCode).FirstOrDefault()).FirstOrDefault(),
                    Birth_Date = _oracleCommon.ToStringDateTime(x.EmployeeDependentDetails.SelectMany(x => x.Person.PersonDetails.Select(x => x.EffectiveDateFrom)).FirstOrDefault()),
                    Death_Date = _oracleCommon.ToStringDateTime(x.EmployeeDependentDetails.SelectMany(x => x.Person.PersonDetails.Select(x => x.EffectiveDateTo)).FirstOrDefault()),
                    Record_Type_Code = 1,
                    Domain_Code = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.EmployeeDependentDetails.SelectMany(x => x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)))).FirstOrDefault().ToString()),
                    Record_Active = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.EmployeeDependentDetails.SelectMany(x => x.Person.EmployeeDetails.SelectMany(x => x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)))).FirstOrDefault().ToString()),
                    Personel_Id = _oracleCommon.OldColumnValue("HRS.TBPERSONNEL", "ID", x.EmployeeDependentDetails.Select(x => x.Id).ToString()),
                    state_code = 2,
                    type_code = _oracleCommon.OldColumnValue("HRS.TBCFAMILY_TYPE", "ID", x.EmployeeDependentDetails.Select(x => x.DependentRelationTypeId).ToString()),
                    city_ID = _oracleCommon.OldColumnValue("HRS.TBCITY", "ID", x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.BirthCountryDivisionId)).ToString()),
                    Id_Serial1 = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySer)).FirstOrDefault(),
                    Id_Serial2 = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySer)).FirstOrDefault(),
                    Id_Serial3 = x.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySerial)).FirstOrDefault(),
                    Exit_Description = x.EmployeeDependentDetails.Select(x => x.Description).ToString()
                }).FirstOrDefault();

            var TBFAMILYQueryable = _TBFAMILY.GetQueryable();


            #region insert into TBFAMILY
            var TBFAMILY = new TBFAMILY()
            {
                ID = employeeMaritalDetail.ID.ToString(),
                NO = employeeMaritalDetail.NO.ToString(),
                NAME = employeeMaritalDetail.Name,
                FAMILY = employeeMaritalDetail.Family,
                FATHER_NAME = employeeMaritalDetail.FatherName,
                ID_NO = employeeMaritalDetail.Id_No,
                NATIONAL_ID = employeeMaritalDetail.CodeMelli,
                BIRTH_DATE = employeeMaritalDetail.Birth_Date,
                DEATH_DATE = employeeMaritalDetail.Death_Date,
                RECORD_TYPE_CODE = 1,
                DOMAIN_CODE = int.Parse(employeeMaritalDetail.Domain_Code),
                RECORD_ACTIVE = int.Parse(employeeMaritalDetail.Record_Active),
                PERSONNEL_ID = employeeMaritalDetail.Personel_Id,
                STATE_CODE = 2,
                TYPE_CODE = int.Parse(employeeMaritalDetail.type_code),
                CITY_ID = employeeMaritalDetail.city_ID,
                ID_SERIAL1 = employeeMaritalDetail.Id_Serial1.ToString(),
                ID_SERIAL2 = employeeMaritalDetail.Id_Serial2.ToString(),
                ID_SERIAL3 = employeeMaritalDetail.Id_Serial3.ToString(),
            };
            _TBFAMILY.Create(TBFAMILY);
            _TBFAMILY.SaveChanges();
            #endregion insert into TBFAMILY
            #region insert into TBFamily_Totla
            var TBFAMILY_TOTAL = new TBFAMILY_TOTAL()
            {
                ID = employeeMaritalDetail.ID.ToString(),
                NO = employeeMaritalDetail.NO.ToString(),
                NAME = employeeMaritalDetail.Name,
                FAMILY = employeeMaritalDetail.Family,
                FATHER_NAME = employeeMaritalDetail.FatherName,
                ID_NO = employeeMaritalDetail.Id_No,
                NATIONAL_ID = employeeMaritalDetail.CodeMelli,
                BIRTH_DATE = employeeMaritalDetail.Birth_Date,
                DEATH_DATE = employeeMaritalDetail.Death_Date,
                RECORD_TYPE_CODE = 1,
                DOMAIN_CODE = int.Parse(employeeMaritalDetail.Domain_Code),
                RECORD_ACTIVE = int.Parse(employeeMaritalDetail.Record_Active),
                PERSONNEL_ID = employeeMaritalDetail.Personel_Id,
                STATE_CODE = 2,
                TYPE_CODE = int.Parse(employeeMaritalDetail.type_code),
                CITY_ID = employeeMaritalDetail.city_ID,
                ID_SERIAL1 = employeeMaritalDetail.Id_Serial1.ToString(),
                ID_SERIAL2 = employeeMaritalDetail.Id_Serial2.ToString(),
                ID_SERIAL3 = employeeMaritalDetail.Id_Serial3.ToString(),
            };
            _TBFAMILY_TOTAL.Create(TBFAMILY_TOTAL);
            _TBFAMILY.SaveChanges();
            #endregion insert into TBFamily_Total
        }

        public void ConvertProcessOfRegistrationMarriageOfEmployeesChildrean(string ID) // EmployeeDependentMaritalDetailId
        {
            var employeeMaritalDetailQueryable = _sqlRepositoryEmployeeMaritalDetail.GetQueryable();
            var employeeMaritalDetail = employeeMaritalDetailQueryable
                .Include(x => x.EmployeeDependent)
                .ThenInclude(x => x.EmployeeDependentDetails)
                .ThenInclude(x => x.Person)
                .ThenInclude(x => x.PersonDetails)
                .Where(x => x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Id),
                    EmployeeId = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EmployeeId).FirstOrDefault(),
                    NO = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EmployeeDependentNumber),
                    MarriageDate = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EffectiveDateFrom).FirstOrDefault()),
                    DivorceDate = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.EffectiveDateTo).FirstOrDefault()),
                    DivorceDescription = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Description).FirstOrDefault(),
                    Name = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.NamePersian).FirstOrDefault()).FirstOrDefault(),
                    Family = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.FamilyPersian).FirstOrDefault()).FirstOrDefault(),
                    FatherName = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.FatherName).FirstOrDefault()).FirstOrDefault(),
                    Id_No = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentityNo).FirstOrDefault()).FirstOrDefault(),
                    CodeMelli = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.NationalCode).FirstOrDefault()).FirstOrDefault(),
                    Birth_Date = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.SelectMany(x => x.Person.PersonDetails.Select(x => x.EffectiveDateFrom)).FirstOrDefault()),
                    Death_Date = _oracleCommon.ToStringDateTime(x.EmployeeDependent.EmployeeDependentDetails.SelectMany(x => x.Person.PersonDetails.Select(x => x.EffectiveDateTo)).FirstOrDefault()),
                    Record_Type_Code = 1,
                    Domain_Code = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "Domain_Code", x.EmployeeDependent.EmployeeDependentDetails.SelectMany(z => z.Person.EmployeeDetails).SelectMany(ed => ed.Employee.EmployeeAppointmentUnits).SelectMany(eau => eau.Unit.UnitDetails).Select(ud => ud.ProvinceId).FirstOrDefault().ToString()),
                    Record_Active = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "Domain_Code", x.EmployeeDependent.EmployeeDependentDetails.SelectMany(z => z.Person.EmployeeDetails).SelectMany(ed => ed.Employee.EmployeeAppointmentUnits).SelectMany(eau => eau.Unit.UnitDetails).Select(ud => ud.ProvinceId).FirstOrDefault().ToString()),
                    Personel_Id = _oracleCommon.OldColumnValue("TBPERSONNEL", "ID", x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Id).ToString()),
                    state_code = 2,
                    type_code = _oracleCommon.OldColumnValue("TBCFAMILY_TYPE", "ID", x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.DependentRelationTypeId).ToString()),
                    city_ID = _oracleCommon.OldColumnValue("TBCITY", "ID", x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.BirthCountryDivisionId)).ToString()),
                    Id_Serial1 = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySer)).FirstOrDefault(),
                    Id_Serial2 = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySer)).FirstOrDefault(),
                    Id_Serial3 = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Person.PersonDetails.Select(x => x.IdentitySerial)).FirstOrDefault(),
                    Exit_Description = x.EmployeeDependent.EmployeeDependentDetails.Select(x => x.Description).ToString()
                }).FirstOrDefault();

            var TBFAMILYQueryable = _TBFAMILY.GetQueryable();
            var entityFamily = TBFAMILYQueryable
                .Where(x => x.STATE_CODE == 2 && x.NATIONAL_ID == employeeMaritalDetail.CodeMelli.ToString())
                .Select(x => new { Id = x.ID })
                .ToList()
                .FirstOrDefault();

            var oldDependentId = _oracleCommon.OldColumnValue("HRS.TBDEPENDENT", "ID", employeeMaritalDetail.EmployeeId.ToString());
            var oldFamilyId = _TBDEPENDENT.GetQueryable().Where(x => x.ID == oldDependentId).FirstOrDefault().FAMILY_ID;
            var TBFAMILY_MARRIAGE = new TBFAMILY_MARRIAGE()
            {
                ID = employeeMaritalDetail.ID.ToString(),
                MARRIAGE_DATE = employeeMaritalDetail.MarriageDate,
                MARRIAGE_DESCRIPTION = employeeMaritalDetail.DivorceDescription,
                DIVORCE_DATE = employeeMaritalDetail.DivorceDate,
                DIVORCE_DESCRIPTION = employeeMaritalDetail.DivorceDescription,
                RECORD_TYPE_CODE = 1,
                DOMAIN_CODE = int.Parse(employeeMaritalDetail.Domain_Code),
                RECORD_ACTIVE = int.Parse(employeeMaritalDetail.Record_Active),
                FAMILY_ID = oldFamilyId, // entityFamily.Id,                   
                STATE_CODE = 2,
            };

        }
    }
}
