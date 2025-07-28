using HRSDataIntegration.DTOs;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IOracleRepository<TBPROVINCE> _provinceRepository;
        private readonly IOracleRepository<TBPROVINCE_NAME_DETAIL> _provinceNameDetailRepository;
        private readonly IOracleRepository<TBPROVINCE_POLI_PROVINCE_DTL> _provincePoliProvinceRepository;
        private readonly ISqlRepository<ProvinceDetail> _sqlRepositoryProvinceDetail;
        private readonly IOracleCommon _oracleCommon;
        public ProvinceService(ISqlRepository<ProvinceDetail> sqlRepositoryProvinceDetail, IOracleCommon oracleCommon,
            IOracleRepository<TBPROVINCE> provinceRepository, IOracleRepository<TBPROVINCE_NAME_DETAIL> provinceNameDetailRepository,
            IOracleRepository<TBPROVINCE_POLI_PROVINCE_DTL> provincePoliProvinceRepository)
        {
            _sqlRepositoryProvinceDetail = sqlRepositoryProvinceDetail;
            _oracleCommon = oracleCommon;
            _provinceRepository = provinceRepository;
            _provinceNameDetailRepository = provinceNameDetailRepository;
            _provincePoliProvinceRepository= provincePoliProvinceRepository;
        }
        public void InsertProvinceToOracle(string Id)
        {
            #region insert into TBPROVINCE
            var provinceDetail = _sqlRepositoryProvinceDetail.GetQueryable();
            var sqlProvinceDetail = provinceDetail
                                   .Where(x => x.ProvinceId.ToString() == Id)
                                   .Select(x => new
                                   {
                                       ID = x.ProvinceId,
                                       CODE = x.Code,
                                       NAME = x.Title,
                                       DOMAIN_CODE = 8589934592,
                                       POLITICAL_PROVINCE_ID = x.CountryDivisionId,
                                       EVALUATTION_CODE = false,
                                       EXEC_DATE = x.EffectiveDateFrom,
                                       STATE_CODE = 100
                                   }).FirstOrDefault();


            var oldPoliticalProvince = _oracleCommon.OldColumnValue("TBPOLITICAL_PROVINCE", "ID", sqlProvinceDetail.POLITICAL_PROVINCE_ID.ToString());
            var oldStateCode = "100";// _oracleCommon.OldColumnValue("TBCORGAN_STATE", "ID", sqlProvinceDetail.STATE_CODE.ToString());
           

            // var getLatestDomainCode = _provinceRepository.GetQueryable().Where(x=>x.NAME != "نامعلوم").OrderByDescending(x=>x.DOMAIN_CODE).ToList().Take(1);
            var getLatestDomainCode = _provinceRepository.GetQueryable()
                 .Where(x => x.NAME != "نامعلوم")
                 .ToList()
                 .OrderByDescending(x => x.DOMAIN_CODE)
                 .Select(x => x.DOMAIN_CODE)
                 .FirstOrDefault(); 

            var TBProvinceData = new TBPROVINCE()
            {
                ID = sqlProvinceDetail.ID.ToString(),
                CODE = sqlProvinceDetail.CODE,
                NAME = sqlProvinceDetail.NAME,
                DOMAIN_CODE = getLatestDomainCode * 2,
                POLITICAL_PROVINCE_ID = oldPoliticalProvince,
                EVALUATION_CODE = 1,
                EXEC_DATE = _oracleCommon.ToStringDateTime(sqlProvinceDetail.EXEC_DATE),
                STATE_CODE=int.Parse(oldStateCode),
            };

            _provinceRepository.Create(TBProvinceData);
            _provinceRepository.SaveChanges();
            _oracleCommon.InsertInto_DataConverter_MappingId(TBProvinceData.ID, Id, "HRS.TBPROVINCE", "ID", "OrganChart.Province", "ID");
            // _oracleCommon.InsertInto_DataConverter_MappingId(TBProvinceData.ID, Id);
            #endregion insert into TBPROVINCE

            #region insert into TBPROVINCE_NAME_DETAIL
            var oldProvinceId = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "ID", Id);
            var tbProvinceNameDetail = provinceDetail.Select(x => new
            {
                
                NAME = x.Title,
                PROVINCE_ID = x.Id.ToString(),
                EXEC_DATE = x.EffectiveDateFrom
            }).FirstOrDefault(x => x.PROVINCE_ID.ToString() == oldProvinceId);

            var TBPROVINCE_NAME_DETAIL = new TBPROVINCE_NAME_DETAIL()
            {
                ID= Guid.NewGuid().ToString(),
                NAME = tbProvinceNameDetail.NAME,
                PROVINCE_ID = oldProvinceId,
                EXEC_DATE = _oracleCommon.ToStringDateTime(tbProvinceNameDetail.EXEC_DATE)
            };
            _provinceNameDetailRepository.Create(TBPROVINCE_NAME_DETAIL);
            _provinceNameDetailRepository.SaveChanges();
            //  _oracleCommon.InsertInto_DataConverter_MappingId(TBPROVINCE_NAME_DETAIL.ID, Id);
            #endregion insert into TBPROVINCE_NAME_DETAIL

            #region insert into TBPROVINCE_POLI_PROVINCE_DTL
            var tbProvincePoliProvinceDTL = provinceDetail.Select(x => new
            {
                PROVINCE_ID = x.Id.ToString(),
                POLITICAL_PROVINCE_ID = x.CountryDivisionId.ToString(),// مقدار شناسه mappingId را نداده بود
                EXEC_DATE = x.EffectiveDateFrom,
            }).FirstOrDefault(x => x.PROVINCE_ID.ToString() == oldProvinceId);



            var TBPROVINCE_POLI_PROVINCE_DTL = new TBPROVINCE_POLI_PROVINCE_DTL()
            {
                ID = Guid.NewGuid().ToString(),
                PROVINCE_ID = oldProvinceId,
                POLITICAL_PROVINCE_ID = oldPoliticalProvince,
                EXEC_DATE = _oracleCommon.ToStringDateTime(tbProvincePoliProvinceDTL.EXEC_DATE)
            };
            _provincePoliProvinceRepository.Create(TBPROVINCE_POLI_PROVINCE_DTL);
            _provincePoliProvinceRepository.SaveChanges();
            #endregion insert into TBPROVINCE_POLI_PROVINCE_DTL
            _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 1002, (getLatestDomainCode* 2));
        }
        public void UpdateProvinceToOracleBy(string Id)
        {
            #region Get Data From 
                var provinceDetail = _sqlRepositoryProvinceDetail.GetQueryable();
                var sqlProvinceDetail = provinceDetail.Where(x => x.ProvinceId.ToString() == Id)
                    .Select(x => new
                    {
                        Id = x.Id,
                        ProvinceId = x.ProvinceId,
                        Code = x.Code,
                        Title = x.Title,
                        CountryProvinceId = x.CountryDivisionId,
                        EffectiveDateTo = x.EffectiveDateTo,
                        EffectiveDateFrom = x.EffectiveDateFrom
                    }).FirstOrDefault();
                var oldProvinceId = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "ID", Id);
                var oldPoliticalProvinceId = _oracleCommon.OldColumnValue("HRS.TBPOLITICAL_PROVINCE", "ID", sqlProvinceDetail.CountryProvinceId.ToString());
            #endregion Get Data From 
            #region Word With Oracle
            var oracleTBPROVINCEbyPROVINCEID = _provinceRepository.GetQueryable();
            var entity = oracleTBPROVINCEbyPROVINCEID.Where(x => x.ID == oldProvinceId).ToList().FirstOrDefault();
            if (sqlProvinceDetail.Title != entity.NAME)
            {
                var TBPROVINCE_NAME_DETAIL = new TBPROVINCE_NAME_DETAIL()
                {
                    ID = Guid.NewGuid().ToString(),
                    NAME = sqlProvinceDetail.Title,
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlProvinceDetail.EffectiveDateFrom),
                    PROVINCE_ID = oldProvinceId,
                };
                _provinceNameDetailRepository.Create(TBPROVINCE_NAME_DETAIL);
                _provinceNameDetailRepository.SaveChanges();
            }
            entity.NAME = sqlProvinceDetail.Title;
            entity.POLITICAL_PROVINCE_ID = oldPoliticalProvinceId;
            entity.EXEC_DATE = _oracleCommon.ToStringDateTime(sqlProvinceDetail.EffectiveDateFrom);
            _provinceRepository.SaveChanges();

            var TBPROVINCE_POLI_PROVINCE_DTL = new TBPROVINCE_POLI_PROVINCE_DTL()
            {
                ID = Guid.NewGuid().ToString(),
                PROVINCE_ID = oldProvinceId,
                POLITICAL_PROVINCE_ID=oldPoliticalProvinceId,
                EXEC_DATE = _oracleCommon.ToStringDateTime(sqlProvinceDetail.EffectiveDateFrom)
            };
            _provincePoliProvinceRepository.Create(TBPROVINCE_POLI_PROVINCE_DTL);
            _provincePoliProvinceRepository.SaveChanges();
            #endregion Word With Oracle
        }
        public void RemoveProvinceToOracle(string Id) //ProvinceId
        {
            var provinceDetail = _sqlRepositoryProvinceDetail.GetQueryable();
            var sqlProvinceDetail = provinceDetail
                                   .Where(x => x.ProvinceId.ToString() == Id)
                                   .Select(x=>new
                                   {
                                       Title = x.Title,
                                   }).FirstOrDefault();

          //  var sqlProvinceDetail = _sqlRepositoryProvinceDetail.GetQueryable().Where(x=>x.ProvinceId.ToString() == Id).FirstOrDefault();
            var oldProvinceId = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "ID", Id);

            var oracleTBPROVINCEbyPROVINCEID = _provinceRepository.GetQueryable();
            var entity = oracleTBPROVINCEbyPROVINCEID.Where(x => x.ID == oldProvinceId).ToList().FirstOrDefault();
            _provinceRepository.Delete(entity);
            _provinceRepository.SaveChanges();

        }

       
    }
}
