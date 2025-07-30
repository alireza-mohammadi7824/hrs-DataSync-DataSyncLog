using HRSDataIntegration.DTOs;
using HRSDataIntegration.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class UnitService : IUnitService
    {
        private readonly IOracleRepository<TBUNIT> _unitRepository;
        private readonly IOracleRepository<TBUNIT_PARENT_DETAIL> _unitParentRepository;
        private readonly IOracleRepository<TBUNIT_PROVINCE_DETAIL> _unitProvinceRepository;
        private readonly IOracleRepository<TBUNIT_POLI_PROVINCE_DETAIL> _unitPoliProvinceRepository;
        private readonly IOracleRepository<TBUNIT_TOWNSHIP_DETAIL> _unitTownshipRepository;
        private readonly IOracleRepository<TBUNIT_BIG_VILLAGE_DETAIL> _unitBigVillageRepository;
        private readonly IOracleRepository<TBUNIT_VILLAGE_DETAIL> _unitVillageRepository;
        private readonly IOracleRepository<TBUNIT_CITY_DETAIL> _unitCityRepository;
        private readonly IOracleRepository<TBUNIT_PART_DETAIL> _unitPartRepository;
        private readonly IOracleRepository<TBUNIT_DESTROY_DETAIL> _unitDestroyRepository;
        private readonly IOracleRepository<TBUNIT_NAME> _unitNameRepository;
        private readonly IOracleRepository<TBUNIT_TYPE_DETAIL> _unitTypeDetailRepository;
        private readonly ISqlRepository<UnitDetail> _sqlRepositoryUnitDetail;
        private readonly ISqlRepository<CountryDivisionDetail> _sqlRepositoryCountryDivisionDetail;
        private readonly IOracleCommon _oracleCommon;
        public UnitService(IOracleRepository<TBUNIT> unitRepository, ISqlRepository<UnitDetail> sqlRepositoryUnitDetail, IOracleCommon oracleCommon, 
            ISqlRepository<CountryDivisionDetail> sqlRepositoryCountryDivisionDetail, IOracleRepository<TBUNIT_TOWNSHIP_DETAIL> unitTownshipRepository,
            IOracleRepository<TBUNIT_BIG_VILLAGE_DETAIL> unitBigVillageRepository, IOracleRepository<TBUNIT_PARENT_DETAIL> unitParentRepository,
            IOracleRepository<TBUNIT_VILLAGE_DETAIL> unitVillageRepository, IOracleRepository<TBUNIT_CITY_DETAIL> unitCityRepository, IOracleRepository<TBUNIT_PART_DETAIL> unitPartRepository,
            IOracleRepository<TBUNIT_DESTROY_DETAIL> unitDestroyRepository, IOracleRepository<TBUNIT_NAME> unitNameRepository,
            IOracleRepository<TBUNIT_TYPE_DETAIL> unitTypeDetailRepository, IOracleRepository<TBUNIT_PROVINCE_DETAIL> unitProvinceRepository)
        {
            _oracleCommon = oracleCommon;
            _unitRepository = unitRepository;
            _sqlRepositoryUnitDetail = sqlRepositoryUnitDetail;
            _sqlRepositoryCountryDivisionDetail = sqlRepositoryCountryDivisionDetail;
            _unitTownshipRepository = unitTownshipRepository;
            _unitBigVillageRepository = unitBigVillageRepository;
            _unitVillageRepository = unitVillageRepository;
            _unitCityRepository = unitCityRepository;
            _unitPartRepository = unitPartRepository;
            _unitDestroyRepository = unitDestroyRepository;
            _unitNameRepository = unitNameRepository;
            _unitTypeDetailRepository = unitTypeDetailRepository;
            _unitParentRepository = unitParentRepository;
            _unitProvinceRepository = unitProvinceRepository;
        }
        #region insert to unit tables according to scenario
        public void ConvertSqlUnitTableConvertToOracleTBUNITtableWhenInsert(string Id)
        {

            try
            {
                var CountryDivisionDetailQueryabel = _sqlRepositoryCountryDivisionDetail.GetQueryable();

                var CountryDivisionIdByUnitId = _sqlRepositoryUnitDetail.GetQueryable().Select(c => new
                {
                    ParentId = c.ParentId,
                    CountryDivisionIdd = c.CountryDivisionId,
                    c.UnitId,
                    EffectiveDateFrom = c.EffectiveDateFrom,
                    EffectiveDateTo = c.EffectiveDateTo,
                    Id = c.ID,
                })
                    .Where(x => x.UnitId.ToString() == Id)
                    .FirstOrDefault();


                var CountryDivisionDetailCity_Id = CountryDivisionDetailQueryabel
                                                   .Where(x => x.CountryDivisionId == CountryDivisionIdByUnitId.CountryDivisionIdd &&
                                                   x.EffectiveDateFrom <= CountryDivisionIdByUnitId.EffectiveDateFrom &&
                                                   (x.EffectiveDateTo > CountryDivisionIdByUnitId.EffectiveDateFrom || x.EffectiveDateTo == 0)
                                                   ).FirstOrDefault();


                var cityTypeId = CountryDivisionDetailCity_Id.CountryDivisionTypeId;

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
                    cityId = CountryDivisionDetailCity_Id.CountryDivisionId.ToString();
                    var cityParentId = CountryDivisionDetailCity_Id.CountryDivisionDetailParentId.ToString();
                    partId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(cityParentId)).CountryDivisionId.ToString();
                    var partParentId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(cityParentId)).CountryDivisionDetailParentId;
                    townshipId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionId.ToString();
                    var townshipParentId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionDetailParentId;
                    politicalProvinceId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionId.ToString();
                    var politicalProvinceParentId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionDetailParentId;
                    countryId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == politicalProvinceParentId).CountryDivisionId.ToString();
                }
                #endregion if city
                #region if village
                else if (cityTypeId == Guid.Parse("8D827E19-B70D-4C72-8AA6-0FEAF737A78A"))
                {
                    villageId = CountryDivisionDetailCity_Id.CountryDivisionId.ToString();
                    var villageParentId = CountryDivisionDetailCity_Id.CountryDivisionDetailParentId.ToString();
                    bigVillageId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(villageParentId)).CountryDivisionId.ToString();
                    var bigVillageParentId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == Guid.Parse(villageParentId)).CountryDivisionDetailParentId;
                    partId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == bigVillageParentId).CountryDivisionId.ToString();
                    var partParentId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == bigVillageParentId).CountryDivisionDetailParentId;
                    townshipId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionId.ToString();
                    var townshipParentId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == partParentId).CountryDivisionDetailParentId;
                    politicalProvinceId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionId.ToString();
                    var politicalProvinceParentId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == townshipParentId).CountryDivisionDetailParentId;
                    countryId = CountryDivisionDetailQueryabel.FirstOrDefault(x => x.ID == politicalProvinceParentId).CountryDivisionId.ToString();
                }
                #endregion if village
                #region sql unitdetail query
                var sqlUnitDetail = _sqlRepositoryUnitDetail.GetQueryable()
                          .Where(x => x.UnitId.ToString() == Id)
                          .Select(x => new
                          {
                              ID = x.Unit.Id,
                              UNIT_ID = x.UnitId,
                              CODE = x.Code, //24333725,
                              TITLE = x.Title,
                              ADDRESS = x.Address,
                              TELEPHONE = x.Unit.UnitTels.Select(x => x.Number).FirstOrDefault(),
                              FAX = x.Unit.UnitTels.Select(x => x.Number).FirstOrDefault(),
                              CREATE_DATE = x.EffectiveDateFrom,
                              DESTROY_DATE = x.EffectiveDateTo,
                              TYPE_CODE = x.UnitLevelId,
                              PARENT_UNIT_ID = x.ParentId,
                              PROVINCE_ID = x.ProvinceId,
                              CITY_ID = cityId,
                              PART_ID = partId,
                              BIG_VILLAGE_ID = bigVillageId,
                              VILLAGE_ID = villageId,
                              STATE_CODE = 100,
                              MEHR_CODE = x.MehrGostarCode,
                              TOWNSHIP_ID = townshipId,
                              POLITICAL_PROVINCE_ID = politicalProvinceId,
                              LATITUDE = x.Latitude,
                              LONGITUDE = x.Longitude,
                              CBI_CODE = x.CentralBankCode,
                              EffectiveDateFrom = x.EffectiveDateFrom,
                              EffectiveDateTo = x.EffectiveDateTo,
                          }).FirstOrDefault();
                #endregion sql unitdetail query
                #region oracle id
                var oldPROVINCE_ID = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "ID", sqlUnitDetail.PROVINCE_ID.ToString());
                var OldCITY_IDMappingId = _oracleCommon.OldColumnValue("HRS.TBCITY", "ID", sqlUnitDetail.CITY_ID.ToString());
                var OldPART_IDMappingId = _oracleCommon.OldColumnValue("HRS.TBPART", "ID", sqlUnitDetail.PART_ID.ToString());
                var OldBIG_VILLAGE_IDMappingId = _oracleCommon.OldColumnValue("HRS.TBBIG_VILLAGE", "ID", sqlUnitDetail.BIG_VILLAGE_ID.ToString());
                var OldVILLAGE_IDMappingId = _oracleCommon.OldColumnValue("HRS.TBVILLAGE", "ID", sqlUnitDetail.VILLAGE_ID.ToString());
                var OldTOWNSHIP_IDMappingId = _oracleCommon.OldColumnValue("HRS.TBTOWNSHIP", "ID", sqlUnitDetail.TOWNSHIP_ID.ToString());
                var OldPOLITICAL_PROVINCE_IDId = _oracleCommon.OldColumnValue("HRS.TBPOLITICAL_PROVINCE", "ID", sqlUnitDetail.POLITICAL_PROVINCE_ID.ToString());
                var OldTYPE_CODE_IDId = _oracleCommon.OldColumnValue("HRS.TBCUNIT_TYPE", "ID", sqlUnitDetail.TYPE_CODE.ToString());
                var OldParent_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.PARENT_UNIT_ID.ToString());
                var OldSTATECode_ID = 2; //_oracleCommon.OldColumnValue("HRS.TBCUNIT_STATE", "CODE", sqlUnitDetail.TYPE_CODE.ToString());
                #endregion oracle id
                #region create oracle object
                var tbUnit = new TBUNIT()
                {
                    ID = sqlUnitDetail.ID.ToString(),
                    CODE = sqlUnitDetail.CODE,
                    NAME = sqlUnitDetail.TITLE,
                    ADDRESS = sqlUnitDetail.ADDRESS,
                    TELEPHONE = sqlUnitDetail.TELEPHONE,
                    FAX = sqlUnitDetail.FAX,
                    CREATE_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.EffectiveDateFrom),
                    DESTROY_DATE = sqlUnitDetail.EffectiveDateTo == 0 ? null : _oracleCommon.ToStringDateTime(sqlUnitDetail.EffectiveDateTo),
                    TYPE_CODE = int.Parse(OldTYPE_CODE_IDId),               //   uncomment 
                    PARENT_UNIT_ID = OldParent_UnitId.ToString(),//sqlUnitDetail.PARENT_UNIT_ID,
                    PROVINCE_ID = oldPROVINCE_ID,
                    CITY_ID = OldCITY_IDMappingId,
                    PART_ID = OldPART_IDMappingId,
                    BIG_VILLAGE_ID = OldBIG_VILLAGE_IDMappingId != null ? OldBIG_VILLAGE_IDMappingId : null,
                    VILLAGE_ID = OldVILLAGE_IDMappingId != null ? OldVILLAGE_IDMappingId : null,
                    STATE_CODE = 2,//int.Parse(OldSTATECode_ID),
                    MEHR_CODE = sqlUnitDetail.MEHR_CODE,
                    TOWNSHIP_ID = OldTOWNSHIP_IDMappingId,
                    POLITICAL_PROVINCE_ID = OldPOLITICAL_PROVINCE_IDId,
                    LATITUDE = sqlUnitDetail.LATITUDE,
                    LONGITUDE = sqlUnitDetail.LONGITUDE,
                    CBI_CODE = sqlUnitDetail.CBI_CODE?.ToString(),
                };
                #endregion create oracle object
                #region insert to db
                _unitRepository.Create(tbUnit);
                _unitRepository.SaveChanges();
                _oracleCommon.InsertInto_DataConverter_MappingId(tbUnit.ID, Id, "HRS.TBUNIT", "ID", "OrganChart.Unit", "ID");
                #endregion insert to db

                #region insert to TBUNIT_PARENT_DETAIL
                var oldParentUnitId = _oracleCommon.OldColumnValue("TBUNIT", "ID", sqlUnitDetail.PARENT_UNIT_ID.ToString());

                var TBUNIT_PARENT_DETAIL = new TBUNIT_PARENT_DETAIL()
                {
                    ID = Guid.NewGuid().ToString(),//tbParentDetail.ID.ToString(),
                    UNIT_ID = sqlUnitDetail.ID.ToString(),
                    PARENT_UNIT_ID = oldParentUnitId,
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE) // tbParentDetail.EXEC_DATE
                };
                _unitParentRepository.Create(TBUNIT_PARENT_DETAIL);
                _unitParentRepository.SaveChanges();
                #endregion insert to TBUNIT_PARENT_DETAIL

                #region insert to TBUNIT_PROVINCE_DETAIL
                var oldProvinceUnitId = _oracleCommon.OldColumnValue("TBPROVINCE", "ID", sqlUnitDetail.PROVINCE_ID.ToString());


                var TBUNIT_PROVINCE_DETAIL = new TBUNIT_PROVINCE_DETAIL()
                {
                    ID = Guid.NewGuid().ToString(), //tbProvinceDetail.ID.ToString(),
                    UNIT_ID = sqlUnitDetail.ID.ToString(),
                    PROVINCE_ID = oldProvinceUnitId,
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE) //_oracleCommon.ToStringDateTime(tbProvinceDetail.EXEC_DATE)
                };

                _unitProvinceRepository.Create(TBUNIT_PROVINCE_DETAIL);
                _unitProvinceRepository.SaveChanges();
                #endregion insert to TBUNIT_PROVINCE_DETAIL

                #region insert to TBUNIT_NAME
                var TBUNIT_NAME = new TBUNIT_NAME()
                {
                    ID = Guid.NewGuid().ToString(),//tbUnitName.ID,
                    UNIT_ID = sqlUnitDetail.ID.ToString(),
                    NAME = sqlUnitDetail.TITLE,
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE) //_oracleCommon.ToStringDateTime(tbUnitName.EXEC_DATE),
                };
                _unitNameRepository.Create(TBUNIT_NAME);
                _unitNameRepository.SaveChanges();

                #endregion insert to TBUNIT_NAME

                #region insert to TBUNIT_TYPE_DETAIL
                var oldUnitTypeCodeId = _oracleCommon.OldColumnValue("TBCUNIT_TYPE", "ID", sqlUnitDetail.TYPE_CODE.ToString());

                var TBUNIT_TYPE_DETAIL = new TBUNIT_TYPE_DETAIL()
                {
                    ID = Guid.NewGuid().ToString(),//tbUnitTypeDetail.ID,
                    UNIT_ID = sqlUnitDetail.ID.ToString(),//Guid.Parse(oldUnitTypeDetailId),
                    UNIT_TYPE_CODE = int.Parse(oldUnitTypeCodeId),
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE)
                };

                _unitTypeDetailRepository.Create(TBUNIT_TYPE_DETAIL);
                _unitTypeDetailRepository.SaveChanges();

                //  _oracleCommon.InsertInto_DataConverter_MappingId(TBUNIT_TYPE_DETAIL.ID, Id);

                _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 1000, 8589934592);
                #endregion insert to TBUNIT_TYPE_DETAIL

                #region  TBUNIT_POLI_PROVINCE_DETAIL

                var OldPOLI_PROVINCE_IDId = _oracleCommon.OldColumnValue("TBPOLITICAL_PROVINCE", "ID", sqlUnitDetail.POLITICAL_PROVINCE_ID.ToString());


                var TBUNIT_POLI_PROVINCE_DETAIL = new TBUNIT_POLI_PROVINCE_DETAIL()
                {
                    ID = Guid.NewGuid().ToString(),//tbPoliProvinceDetail.ID,
                    UNIT_ID = sqlUnitDetail.ID.ToString(), //Guid.Parse(OldPOLI_PROVINCE_IDId),
                    POLI_PROVINCE_ID = OldPOLI_PROVINCE_IDId,
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE)
                };



                #endregion  TBUNIT_POLI_PROVINCE_DETAIL

                #region  TBUNIT_TOWNSHIP_DETAIL
                var oldTownshipID = _oracleCommon.OldColumnValue("TBTOWNSHIP", "ID", sqlUnitDetail.TOWNSHIP_ID.ToString());

                var TBUNIT_TOWNSHIP_DETAIL = new TBUNIT_TOWNSHIP_DETAIL()
                {
                    ID = Guid.NewGuid(),// tbUnitTownshipDetail.ID,
                    UNIT_ID = sqlUnitDetail.ID, // Guid.Parse(oldTownshipUnitId),
                    TOWNSHIP_ID = Guid.Parse(oldTownshipID),
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE)
                };


                #endregion  TBUNIT_TOWNSHIP_DETAIL

                #region  TBUNIT_BIG_VILLAGE_DETAIL

                var oldBigVillageDetailId = _oracleCommon.OldColumnValue("TBBIG_VILLAGE", "ID", sqlUnitDetail.BIG_VILLAGE_ID.ToString());


                var TBUNIT_BIG_VILLAGE_DETAIL = new TBUNIT_BIG_VILLAGE_DETAIL()
                {
                    ID = Guid.NewGuid(), //tbUitBigVillageDetail.ID,
                    UNIT_ID = sqlUnitDetail.ID, //Guid.Parse(oldBigVillageUnitId),
                    BIG_VILLAGE_ID = Guid.Parse(oldBigVillageDetailId),
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE)
                };

                #endregion  TBUNIT_BIG_VILLAGE_DETAIL

                #region TBUNIT_VILLAGE_DETAIL

                var oldVillageDetailId = _oracleCommon.OldColumnValue("TBVILLAGE", "ID", sqlUnitDetail.VILLAGE_ID.ToString());

                var TBUNIT_VILLAGE_DETAIL = new TBUNIT_VILLAGE_DETAIL()
                {
                    ID = Guid.NewGuid(), // tbUnitVillageDetail.ID,
                    UNIT_ID = sqlUnitDetail.ID, //Guid.Parse(oldVillageUnitId),
                    VILLAGE_ID = Guid.Parse(oldVillageDetailId),
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE)
                };

                //_unitVillageRepository.Create(TBUNIT_VILLAGE_DETAIL);
                //_unitVillageRepository.SaveChanges();
                //_oracleCommon.InsertInto_DataConverter_MappingId(TBUNIT_VILLAGE_DETAIL.ID, Id);

                #endregion TBUNIT_VILLAGE_DETAIL

                #region TBUNIT_CITY_DETAIL

                var oldunitCityDetailId = _oracleCommon.OldColumnValue("TBCITY", "ID", sqlUnitDetail.CITY_ID.ToString());

                var TBUNIT_CITY_DETAIL = new TBUNIT_CITY_DETAIL()
                {
                    ID = Guid.NewGuid(), // tbUnitCityDetail.ID,
                    UNIT_ID = sqlUnitDetail.ID, //Guid.Parse(oldCityUnitId),
                    CITY_ID = Guid.Parse(oldunitCityDetailId),
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE),
                };

                //_unitCityRepository.Create(TBUNIT_CITY_DETAIL);
                //_unitCityRepository.SaveChanges();
                //_oracleCommon.InsertInto_DataConverter_MappingId(TBUNIT_CITY_DETAIL.ID, Id);
                #endregion TBUNIT_CITY_DETAIL

                #region TBUNIT_PART_DETAIL

                var oldUnitPartDetailId = _oracleCommon.OldColumnValue("TBPART", "ID", sqlUnitDetail.PART_ID.ToString());

                var TBUNIT_PART_DETAIL = new TBUNIT_PART_DETAIL()
                {
                    ID = Guid.NewGuid(), //  tbUnitPartDetail.ID,
                    UNIT_ID = sqlUnitDetail.ID, //Guid.Parse(oldPartUnitId),
                    PART_ID = Guid.Parse(oldUnitPartDetailId),
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.CREATE_DATE)
                };

                //_unitPartRepository.Create(TBUNIT_PART_DETAIL);
                //_unitPartRepository.SaveChanges();
                //_oracleCommon.InsertInto_DataConverter_MappingId(TBUNIT_PART_DETAIL.ID, Id);
                #endregion TBUNIT_PART_DETAIL

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///

                //var tbUnitDestroyDetail = parentDetail.Select(x => new
                //{
                //    Id = x.ID,
                //    UNIT_ID = x.UnitId,
                //    DESTROY_START_DATE = x.EffectiveDateFrom,
                //    DESTRIY_END_DATE = x.EffectiveDateTo
                //}).FirstOrDefault(x => x.UNIT_ID.ToString() == Id);

                //var oldDestroyUnitId = _oracleCommon.OldColumnValue("TBUNIT", "ID", tbUnitDestroyDetail.UNIT_ID.ToString());

                //var TBUNIT_DESTROY_DETAIL = new TBUNIT_DESTROY_DETAIL()
                //{
                //    ID = tbUnitDestroyDetail.Id,
                //    UNIT_ID = Guid.Parse(oldDestroyUnitId),
                //    DESTROY_START_DATE = _oracleCommon.ToStringDateTime(tbUnitDestroyDetail.DESTROY_START_DATE),
                //    DESTROY_END_DATE = _oracleCommon.ToStringDateTime(tbUnitDestroyDetail.DESTRIY_END_DATE)
                //};


                //_unitDestroyRepository.Create(TBUNIT_DESTROY_DETAIL);
                //_unitDestroyRepository.SaveChanges();
                // _oracleCommon.InsertInto_DataConverter_MappingId(TBUNIT_DESTROY_DETAIL.ID, Id);





                // var CountryDivisionParentId = _sqlRepositoryCountryDivisionDetail.GetQueryable().FirstOrDefault(x => x.CountryDivisionId == CountryDivisionIdByUnitId.CountryDivisionId).CountryDivisionDetailParentId;



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

        #endregion insert to unit tables according to scenario
        public void ConvertUpdateTBUNIT_PARENT_DETAIL(string Id)
        {
            try
            {
                var unitDetail = _sqlRepositoryUnitDetail.GetQueryable();
                var sqlUnitDetail = unitDetail
                        .Where(x => x.UnitId.ToString() == Id)
                        .Select(x => new
                        {
                            ID = x.ID,
                            UNIT_ID = x.UnitId,
                            PARENT_UNIT_ID = x.ParentId,
                            EffectiveDateFrom = x.EffectiveDateFrom,
                            EffectiveDateTo = x.EffectiveDateTo
                        }).FirstOrDefault();
                var OldParent_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.PARENT_UNIT_ID.ToString());//16bbb957-28aa-4263-b624-4e5456f5fa61
                var Old_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.UNIT_ID.ToString());//993c6cf2-e852-4d87-5b44-08dd96b387c3

                #region Update to TBUNIT
                var OracleTBUNITByUnitId = _unitRepository.GetQueryable();
                var entity = OracleTBUNITByUnitId
                        .Where(x => x.ID == Old_UnitId)
                        .ToList()
                        .FirstOrDefault();
                entity.PARENT_UNIT_ID = OldParent_UnitId;
                _unitRepository.SaveChanges();

                #endregion Update To TBUNIT

                #region Insert to TBUNIT_PARENT_DETAIL
                var TBUNIT_PARENT_DETAIL = new TBUNIT_PARENT_DETAIL()
                {
                    ID = sqlUnitDetail.ID.ToString(),
                    UNIT_ID = Old_UnitId,
                    PARENT_UNIT_ID = OldParent_UnitId,
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.EffectiveDateFrom)
                };

                _unitParentRepository.Create(TBUNIT_PARENT_DETAIL);
                _unitParentRepository.SaveChanges();
                #endregion Insert to TBUNIT_PARENT_DETAIL
                // _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 1000, 8589934592);
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
        public void ConvertUpdateTBUNIT_Name(string Id)
        {
            try
            {
                var unitDetail = _sqlRepositoryUnitDetail.GetQueryable();
                var sqlUnitDetail = unitDetail
                        .Where(x => x.UnitId.ToString() == Id)
                        .Select(x => new
                        {
                            ID = x.ID,
                            UNIT_ID = x.UnitId,
                            NAME = x.Title,
                            EffectiveDateFrom = x.EffectiveDateFrom,
                            EffectiveDateTo = x.EffectiveDateTo
                        }).FirstOrDefault();
                var Old_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.UNIT_ID.ToString());

                var OracleTBUNITByUnitId = _unitRepository.GetQueryable();
                var entity = OracleTBUNITByUnitId
                       .Where(x => x.ID == Old_UnitId)
                       .ToList()
                       .FirstOrDefault();
                entity.NAME = sqlUnitDetail.NAME;
                _unitRepository.SaveChanges();
                #region Insert to TBUNIT_NAME

                var TBUNIT_NAME = new TBUNIT_NAME
                {
                    ID = sqlUnitDetail.ID.ToString(),
                    UNIT_ID = Old_UnitId,
                    NAME = sqlUnitDetail.NAME,
                    EXEC_DATE = _oracleCommon.ToStringDateTime(sqlUnitDetail.EffectiveDateFrom)
                };
                _unitNameRepository.Create(TBUNIT_NAME);
                _unitNameRepository.SaveChanges();
                #endregion Insert to TBUNIT_NAME
                // _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 72200, 8589934592);
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
        public void ConvertUpdateTBUNIT_Address(string Id)
        {
            try
            {
                var unitDetail = _sqlRepositoryUnitDetail.GetQueryable();
                var sqlUnitDetail = unitDetail
                        .Where(x => x.UnitId.ToString() == Id)
                        .Select(x => new
                        {
                            ID = x.ID,
                            UNIT_ID = x.UnitId,
                            Address = x.Address,
                            EffectiveDateFrom = x.EffectiveDateFrom,
                            EffectiveDateTo = x.EffectiveDateTo
                        }).FirstOrDefault();
                var Old_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.UNIT_ID.ToString());

                var OracleTBUNITByUnitId = _unitRepository.GetQueryable();
                var entity = OracleTBUNITByUnitId
                       .Where(x => x.ID == Old_UnitId)
                       .ToList()
                       .FirstOrDefault();
                entity.ADDRESS = sqlUnitDetail.Address;
                _unitRepository.SaveChanges();
                // _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 72200, 8589934592);
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
        public void ConvertUpdateTBUNIT_Tels(string Id)
        {
            try
            {
                var unitDetail = _sqlRepositoryUnitDetail.GetQueryable();
                var sqlUnitDetail = unitDetail
                        .Where(x => x.UnitId.ToString() == Id)
                        .Select(x => new
                        {
                            ID = x.ID,
                            UNIT_ID = x.UnitId,
                            Tels = x.Unit.UnitTels.Select(x => x.Number).FirstOrDefault(),
                            EffectiveDateFrom = x.EffectiveDateFrom,
                            EffectiveDateTo = x.EffectiveDateTo
                        }).FirstOrDefault();
                var Old_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.UNIT_ID.ToString());

                var OracleTBUNITByUnitId = _unitRepository.GetQueryable();
                var entity = OracleTBUNITByUnitId
                       .Where(x => x.ID == Old_UnitId)
                       .ToList()
                       .FirstOrDefault();
                entity.TELEPHONE = sqlUnitDetail.Tels;
                _unitRepository.SaveChanges();
                // _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN",Id,72300, 8589934592);
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id) , true);
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
        public void ConvertDestroy_Enhelal_TBUNIT(string Id)
        {
            try
            {
                var unitDetail = _sqlRepositoryUnitDetail.GetQueryable();
                var sqlUnitDetail = unitDetail
                        .Where(x => x.UnitId.ToString() == Id)
                        .Select(x => new
                        {
                            ID = x.ID,
                            UNIT_ID = x.UnitId,
                            Tels = x.Unit.UnitTels.Select(x => x.Number).FirstOrDefault(),
                            EffectiveDateFrom = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                            EffectiveDateTo = x.EffectiveDateTo == 0 ? null : _oracleCommon.ToStringDateTime(x.EffectiveDateTo),
                        }).FirstOrDefault();
                var Old_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.UNIT_ID.ToString());

                var OracleTBUNITByUnitId = _unitRepository.GetQueryable();
                var entity = OracleTBUNITByUnitId
                       .Where(x => x.ID == Old_UnitId)
                       .ToList()
                       .FirstOrDefault();
                entity.STATE_CODE = 3;
                entity.DESTROY_DATE = sqlUnitDetail.EffectiveDateFrom;
                _unitRepository.SaveChanges();

                var TBUNIT_DESTROY_DETAIL = new TBUNIT_DESTROY_DETAIL()
                {
                    ID = sqlUnitDetail.ID.ToString(),
                    UNIT_ID = Old_UnitId,
                    DESTROY_START_DATE = sqlUnitDetail.EffectiveDateFrom,
                    DESTROY_END_DATE = sqlUnitDetail.EffectiveDateTo
                };

                _unitDestroyRepository.Create(TBUNIT_DESTROY_DETAIL);
                _unitDestroyRepository.SaveChanges();
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id) ,true);
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
        public void ConvertDestroy_Edgham_TBUNIT(string Id)
        {
            try
            {
                var unitDetail = _sqlRepositoryUnitDetail.GetQueryable();
                var sqlUnitDetail = unitDetail
                        .Where(x => x.UnitId.ToString() == Id)
                        .Select(x => new
                        {
                            ID = x.ID,
                            UNIT_ID = x.UnitId,
                            Tels = x.Unit.UnitTels.Select(x => x.Number).FirstOrDefault(),
                            EffectiveDateFrom = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                            EffectiveDateTo = x.EffectiveDateTo == 0 ? null : _oracleCommon.ToStringDateTime(x.EffectiveDateTo),
                        }).FirstOrDefault();
                var Old_UnitId = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", sqlUnitDetail.UNIT_ID.ToString());

                var OracleTBUNITByUnitId = _unitRepository.GetQueryable();
                var entity = OracleTBUNITByUnitId
                       .Where(x => x.ID == Old_UnitId)
                       .ToList()
                       .FirstOrDefault();
                entity.STATE_CODE = 3;
                entity.DESTROY_DATE = sqlUnitDetail.EffectiveDateFrom;
                _unitRepository.SaveChanges();

                var TBUNIT_DESTROY_DETAIL = new TBUNIT_DESTROY_DETAIL()
                {
                    ID = sqlUnitDetail.ID.ToString(),
                    UNIT_ID = Old_UnitId,
                    DESTROY_START_DATE = sqlUnitDetail.EffectiveDateFrom,
                    DESTROY_END_DATE = sqlUnitDetail.EffectiveDateTo
                };

                _unitDestroyRepository.Create(TBUNIT_DESTROY_DETAIL);
                _unitDestroyRepository.SaveChanges();
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id) , true);
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
    }
}
