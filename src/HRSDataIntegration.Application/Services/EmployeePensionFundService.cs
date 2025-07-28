using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class EmployeePensionFundService : IEmployeePensionFundService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBPERSONNEL_PENSION_FUND> _TBPERSONNEL_PENSION_FUNDRepository;
        private readonly ISqlRepository<EmployeePensionFund> _sqlRepositoryEmployeePensionFund;

        public EmployeePensionFundService(IOracleCommon oracleCommon, 
            IOracleRepository<TBPERSONNEL_PENSION_FUND> TBPERSONNEL_PENSION_FUNDRepository,
            ISqlRepository<EmployeePensionFund> sqlRepositoryEmployeePensionFund)
        {
            _oracleCommon = oracleCommon;
            _TBPERSONNEL_PENSION_FUNDRepository = TBPERSONNEL_PENSION_FUNDRepository;
            _sqlRepositoryEmployeePensionFund = sqlRepositoryEmployeePensionFund;
        }

       

        public void ConvertToEmployeePensionFund_Insert_ToOracleTable(string ID) // EmployeeId
        {
            var employeePensionFundQueryable = _sqlRepositoryEmployeePensionFund.GetQueryable();
            var employeePensionFund = employeePensionFundQueryable
                .Where(x=>x.EmployeeId.ToString() == ID)    
                .Select(x=>new
                {
                   ID = x.Id,
                   PERSONEL_ID = x.EmployeeId,
                   DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)).FirstOrDefault().ToString()),
                   RECORD_ACTIVE =  _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)).FirstOrDefault().ToString()),
                   RECORD_TYOE_CODE = 1,
                   PENSION_FUND_CODE = x.PensionFundId,
                   FROM_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                   TO_DATE = x.EffectiveDateTo == 0 ? null: _oracleCommon.ToStringDateTime(x.EffectiveDateTo),
                   MEMBER_CODE = x.MembershipCode,
                   IS_MEMBER_REFAH_FUND = 1,
                   IS_MEMBER_HEALTH = 1,
                   IS_MEMBER_SAVING_CASH = 1,
                   STATE_CODE =3,
                   DESCRIPTION = x.Description,
                })
                .FirstOrDefault();
            var oldPensionFundId = _oracleCommon.OldColumnValue("HRS.TBCPENSION_FUND", "CODE" , employeePensionFund.PENSION_FUND_CODE.ToString());
            var oldPersonelId = _oracleCommon.OldColumnValue("HRS.TBPERSONNEL","Id", employeePensionFund.PERSONEL_ID.ToString());
            var TBPERSONNEL_PENSION_FUND = new TBPERSONNEL_PENSION_FUND()
            {
                ID = employeePensionFund.ID.ToString(),
                PERSONNEL_ID = oldPersonelId,
                DOMAIN_CODE = long.Parse(employeePensionFund.DOMAIN_CODE),
                RECORD_ACTIVE = long.Parse(employeePensionFund.RECORD_ACTIVE),
                RECORD_TYPE_CODE = employeePensionFund.RECORD_TYOE_CODE,
                PENSION_FUND_CODE = int.Parse(oldPensionFundId),
                FROM_DATE = employeePensionFund.FROM_DATE,
                TO_DATE =employeePensionFund.TO_DATE,
                MEMBER_CODE = employeePensionFund.MEMBER_CODE,
                IS_MEMBER_REFAH_FUND =employeePensionFund.IS_MEMBER_REFAH_FUND,
                IS_MEMBER_HEALTH = employeePensionFund.IS_MEMBER_HEALTH,
                IS_MEMBER_SAVING_CASH = employeePensionFund.IS_MEMBER_SAVING_CASH,
                STATE_CODE = 3,
                DESCRIPTION = employeePensionFund.DESCRIPTION
            };
            _TBPERSONNEL_PENSION_FUNDRepository.Create(TBPERSONNEL_PENSION_FUND);
            _TBPERSONNEL_PENSION_FUNDRepository.SaveChanges();

            _oracleCommon.InsertInto_DataConverter_MappingId(TBPERSONNEL_PENSION_FUND.ID.ToString(), ID, "HRS.TBPERSONNEL_PENSION_FUND", "Id", "employeePensionFund", "Id");
            _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", ID , 74955 , 8589934592);
        }

        public void ConvertToEmployeePensionFund_Update_ToOracleTable(string ID)
        {
            var employeePensionFundQueryable = _sqlRepositoryEmployeePensionFund.GetQueryable();
            var employeePensionFund = employeePensionFundQueryable
                .Where(x => x.EmployeeId.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    PERSONEL_ID = x.EmployeeId,
                    DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)).FirstOrDefault().ToString()),
                    RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)).FirstOrDefault().ToString()),
                    RECORD_TYOE_CODE = 1,
                    PENSION_FUND_CODE = x.PensionFundId,
                    FROM_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                    TO_DATE = x.EffectiveDateTo == 0 ? "null" : _oracleCommon.ToStringDateTime(x.EffectiveDateTo),
                    MEMBER_CODE = x.MembershipCode,
                    IS_MEMBER_REFAH_FUND = 1,
                    IS_MEMBER_HEALTH = 1,
                    IS_MEMBER_SAVING_CASH = 1,
                    STATE_CODE = 3,
                    DESCRIPTION = x.Description,
                })
                .FirstOrDefault();

            var tbPersonnelPensionFundQueryable = _TBPERSONNEL_PENSION_FUNDRepository.GetQueryable();
            var tbPersonnelPension = tbPersonnelPensionFundQueryable
                .Where(x => x.ID == employeePensionFund.ID.ToString())
                .ToList()
                .FirstOrDefault();

            tbPersonnelPension.MEMBER_CODE = employeePensionFund.MEMBER_CODE;
            _TBPERSONNEL_PENSION_FUNDRepository.SaveChanges();
        }
        public void ConvertToEmployeePensionFund_Delete_ToOracleTable(string ID)
        {
            var EmployeePensionFundQueryable = _sqlRepositoryEmployeePensionFund.GetQueryable();
            var EmployeePensionFund = EmployeePensionFundQueryable
                .Where(x => x.EmployeeId.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    PERSONEL_ID = x.EmployeeId,
                    DOMAIN_CODE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)).FirstOrDefault().ToString()),
                    RECORD_ACTIVE = _oracleCommon.OldColumnValue("HRS.TBPROVINCE", "DOMAIN_CODE", x.Employee.EmployeeAppointmentUnits.SelectMany(x => x.Unit.UnitDetails.Select(x => x.ProvinceId)).FirstOrDefault().ToString()),
                    RECORD_TYOE_CODE = 1,
                    PENSION_FUND_CODE = x.PensionFundId,
                    FROM_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                    TO_DATE = x.EffectiveDateTo == 0 ? null : _oracleCommon.ToStringDateTime(x.EffectiveDateTo),
                    MEMBER_CODE = x.MembershipCode,
                    IS_MEMBER_REFAH_FUND = 1,
                    IS_MEMBER_HEALTH = 1,
                    IS_MEMBER_SAVING_CASH = 1,
                    STATE_CODE = 3,
                    DESCRIPTION = x.Description,
                })
                .FirstOrDefault();

            var tbPersonnelPensionFundQueryable = _TBPERSONNEL_PENSION_FUNDRepository.GetQueryable();
            var tbPersonnelPension = tbPersonnelPensionFundQueryable
                .Where(x=>x.ID == EmployeePensionFund.ID.ToString())
                .ToList()
                .FirstOrDefault();

            _TBPERSONNEL_PENSION_FUNDRepository.Delete(tbPersonnelPension);
            _TBPERSONNEL_PENSION_FUNDRepository.SaveChanges();
        }
    }
}
