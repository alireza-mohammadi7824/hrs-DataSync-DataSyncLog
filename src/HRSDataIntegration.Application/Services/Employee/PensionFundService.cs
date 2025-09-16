using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class PensionFundService : IPensionFundService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBCPENSION_FUND> _TBCPENSION_FUND;
        private readonly ISqlRepository<PensionFund> _sqlPensionFund;

        public PensionFundService(IOracleCommon oracleCommon ,
                                  IOracleRepository<TBCPENSION_FUND> TBCPENSION_FUND ,
                                  ISqlRepository<PensionFund> sqlPensionFund)
        {
            _oracleCommon = oracleCommon;
            _TBCPENSION_FUND = TBCPENSION_FUND;
            _sqlPensionFund = sqlPensionFund;
        }
        public void ConvertToPensionFund_Insert_ToOracleTable(string ID)
        {
            try
            {
                var PensionFundQueryable = _sqlPensionFund.GetQueryable();
                var PensionFund = PensionFundQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Code = x.Code,
                        Title = x.Title,
                        Name = x.Title
                    })
                    .FirstOrDefault();

                var TBCPENSION_FUND = new TBCPENSION_FUND()
                {
                    CODE = PensionFund.Code,
                    CAPTION = PensionFund.Title,
                    NAME = PensionFund.Name,
                };

                _TBCPENSION_FUND.Create(TBCPENSION_FUND);
                _TBCPENSION_FUND.SaveChanges();
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

        public void ConvertToPensionFund_Update_ToOracleTable(string ID)
        {
            try
            {
                var PensionFundQueryable = _sqlPensionFund.GetQueryable();
                var PensionFund = PensionFundQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Code = x.Code,
                        Title = x.Title,
                        Name = x.Title
                    })
                    .FirstOrDefault();
                var oldCode = _oracleCommon.OldColumnValue("HRS.TBCPENSION_FUND", "CODE", PensionFund.Code.ToString());
                var TBCPENSION_FUND_Queryable = _TBCPENSION_FUND.GetQueryable();
                var entity = TBCPENSION_FUND_Queryable
                    .Where(x => x.CODE == PensionFund.Code)
                    // .Where(x => x.CODE == int.Parse(oldCode))
                    .ToList()
                    .FirstOrDefault();

                entity.CAPTION = PensionFund.Title;
                entity.NAME = PensionFund.Name;

                _TBCPENSION_FUND.SaveChanges();

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
        public void ConvertToPensionFund_Delete_ToOracleTable(string ID)
        {
            try
            {
                var PensionFundQueryable = _sqlPensionFund.GetQueryable();
                var PensionFund = PensionFundQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Code = x.Code,
                        Title = x.Title,
                        Name = x.Title
                    })
                    .FirstOrDefault();
                var oldCode = _oracleCommon.OldColumnValue("HRS.TBCPENSION_FUND", "CODE", PensionFund.Code.ToString());
                var TBCPENSION_FUND_Queryable = _TBCPENSION_FUND.GetQueryable();
                var entity = TBCPENSION_FUND_Queryable
                    .Where(x => x.CODE == PensionFund.Code)
                    //.Where(x => x.CODE == int.Parse(oldCode))
                    .ToList()
                    .FirstOrDefault();

                _TBCPENSION_FUND.Delete(entity);
                _TBCPENSION_FUND.SaveChanges();
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
