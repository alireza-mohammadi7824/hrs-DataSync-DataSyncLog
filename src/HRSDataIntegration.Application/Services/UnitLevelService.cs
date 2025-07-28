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
    public class UnitLevelService : IUnitLevelService
    {
        private readonly IOracleRepository<TBCUNIT_TYPE> _unitTypeRepository;
        private readonly ISqlRepository<UnitLevel> _sqlRepositoryUnitLevel;
        private readonly IOracleCommon _oracleCommon;

        public UnitLevelService(IOracleRepository<TBCUNIT_TYPE> unitTypeRepository, ISqlRepository<UnitLevel> sqlRepositoryUnitLevel, IOracleCommon oracleCommon)
        {
            _oracleCommon = oracleCommon;
            _unitTypeRepository = unitTypeRepository;
            _sqlRepositoryUnitLevel = sqlRepositoryUnitLevel;
        }
        public void ConvertSqlUnitLevelTable_Insert_ToOracletable(string Id) //unitLevelId
        {
            var sqlUnitLevelQueryable = _sqlRepositoryUnitLevel.GetQueryable();
            var sqlUnitLevel = sqlUnitLevelQueryable.Where(x => x.Id.ToString() == Id).Select(x => new
            {
                CODE = x.Code,
                CAPTION = x.Title,
                NAME = x.Title
            })
                .FirstOrDefault();

            var TBCUNIT_TYPE = new TBCUNIT_TYPE()
            {
                CODE = sqlUnitLevel.CODE,
                CAPTION = sqlUnitLevel.CAPTION,
                NAME = sqlUnitLevel.NAME,
            };
            _unitTypeRepository.Create(TBCUNIT_TYPE);
            _unitTypeRepository.SaveChanges();

            _oracleCommon.InsertInto_DataConverter_MappingId(TBCUNIT_TYPE.CODE.ToString() , Id , "HRS.TBCUNIT_TYPE" , "ID" , "OrganChart.UnitLevel" , "ID");
        }  
        public void ConvertSqlUnitLevelTable_Update_ToOracletable(string Id) //unitLevelId
        {
            var sqlUnitLevelQueryable = _sqlRepositoryUnitLevel.GetQueryable();
            var sqlUnitLevel = sqlUnitLevelQueryable.Where(x => x.Id.ToString() == Id).Select(x => new
            {
                CODE = x.Code,
                CAPTION = x.Title,
                NAME = x.Title
            })
              .FirstOrDefault();
            var oldCode = _oracleCommon.OldColumnValue("HRS.TBCUNIT_TYPE", "CODE", sqlUnitLevel.CODE.ToString());
            var oracleTBCUNIT_TYPEQueryable = _unitTypeRepository.GetQueryable();


            var entity = oracleTBCUNIT_TYPEQueryable
                        .Where(x=>x.CODE == int.Parse(oldCode))
                        .ToList()
                        .FirstOrDefault();
            entity.CAPTION = sqlUnitLevel.CAPTION;
            entity.NAME = sqlUnitLevel.NAME;
            _unitTypeRepository.SaveChanges();

        }

        public void ConvertSqlUnitLevelTable_Delete_ToOracletable(string Id) //unitLevelId
        {
            var sqlUnitLevelQueryable = _sqlRepositoryUnitLevel.GetQueryable();
            var sqlUnitLevel = sqlUnitLevelQueryable.Where(x => x.Id.ToString() == Id).Select(x => new
            {
                CODE = x.Code,
                CAPTION = x.Title,
                NAME = x.Title
            })
              .FirstOrDefault();

            var oldCode = _oracleCommon.OldColumnValue("HRS.TBCUNIT_TYPE", "CODE", sqlUnitLevel.CODE.ToString());

            var oracleTBCUNIT_TYPEQueryable = _unitTypeRepository.GetQueryable();
            var entity = oracleTBCUNIT_TYPEQueryable
                        .Where(x => x.CODE == int.Parse(oldCode))
                        .ToList()
                        .FirstOrDefault();
             var oracleCode = _oracleCommon.Get_Old_ColumnValue(Id, "HRS.TBCUNIT_TYPE", "ID", "OrganChart.UnitLevel", "ID");
            entity.CODE = int.Parse(oracleCode);
            _unitTypeRepository.Delete(entity);
            _unitTypeRepository.SaveChanges();
        }
    }
}
