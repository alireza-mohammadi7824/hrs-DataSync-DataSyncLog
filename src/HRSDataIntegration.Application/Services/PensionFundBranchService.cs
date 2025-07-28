using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class PensionFundBranchService : IPensionFundBranchService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBCTAMIN_BRANCH> _TBCTAMIN_BRANCHRepository;
        private readonly ISqlRepository<PensionFundBranch> _SqlPensionFundBranchRepository;
        public PensionFundBranchService(IOracleCommon oracleCommon, 
            IOracleRepository<TBCTAMIN_BRANCH> TBCTAMIN_BRANCHRepository,
            ISqlRepository<PensionFundBranch> SqlPensionFundBranchRepository)
        {
            _oracleCommon = oracleCommon;
            _TBCTAMIN_BRANCHRepository = TBCTAMIN_BRANCHRepository;
            _SqlPensionFundBranchRepository = SqlPensionFundBranchRepository;
        }

        public void ConvertToPensionFundBranch_Insert_ToOracleTable(string ID)
        {
            var pnsionFundBranchQueryable = _SqlPensionFundBranchRepository.GetQueryable();
            var pensionFundBranch = pnsionFundBranchQueryable
                .Where(x=>x.Id.ToString() == ID)
                .Select(x => new
                {
                    Code = x.Code,
                    Title = x.Title,
                    Name = x.Title,
                    PoliticalProvinceCaption = x.CountryDivisionId
                })
                .FirstOrDefault();

            var oldPliticalProcinceCaption = _oracleCommon.OldColumnValue("HRS.TBCTAMIN_BRANCH", "ID", pensionFundBranch.PoliticalProvinceCaption.ToString());

            var TBCTAMIN_BRANCH = new TBCTAMIN_BRANCH()
            {
                CODE = pensionFundBranch.Code.ToString(),
                CAPTION = pensionFundBranch.Title.ToString(),
                NAME = pensionFundBranch.Name.ToString(),
                POLITICAL_PROVINCE_CAPTION = oldPliticalProcinceCaption
            };
            _TBCTAMIN_BRANCHRepository.Create(TBCTAMIN_BRANCH);
            _TBCTAMIN_BRANCHRepository.SaveChanges();
            _oracleCommon.InsertInto_DataConverter_MappingId(pensionFundBranch.Code.ToString(), ID, "HRS.TBCTAMIN_BRANCH", "CODE", "Employee.PensionFund", "ID");
        }
        public void ConvertToPensionFundBranch_Update_ToOracleTable(string ID)
        {
            var pensionFundBranchQueryable = _SqlPensionFundBranchRepository.GetQueryable();
            var pensionFundBranch = pensionFundBranchQueryable
                .Where(x=>x.Id.ToString() == ID)
                .Select(x=> new
                {
                    ID= x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Name = x.Title,
                    PoliticalProvinceCaption = x.CountryDivisionId
                })
                .FirstOrDefault();
            var oldCode = _oracleCommon.OldColumnValue("HRS.TBCTAMIN_BRANCH", "CODE", pensionFundBranch.ID.ToString());
            var oldPliticalProcinceCaption = _oracleCommon.OldColumnValue("HRS.TBCTAMIN_BRANCH", "ID", pensionFundBranch.PoliticalProvinceCaption.ToString());
            var TBCTAMIN_BRANCH_Queryable = _TBCTAMIN_BRANCHRepository.GetQueryable();
            var entity = TBCTAMIN_BRANCH_Queryable
                .Where(x => x.CODE == oldCode)
                .ToList()
                .FirstOrDefault();

            entity.CAPTION = pensionFundBranch.Title;
            entity.NAME = pensionFundBranch.Name;
            entity.POLITICAL_PROVINCE_CAPTION = oldPliticalProcinceCaption;
            _TBCTAMIN_BRANCHRepository.SaveChanges();
        }
        public void ConvertToPensionFundBranch_Delete_ToOracleTable(string ID)
        {
            var pensionFundBranchQueryable = _SqlPensionFundBranchRepository.GetQueryable();
            var pensionFundBranch = pensionFundBranchQueryable
                .Where(x => x.Id.ToString() == ID)
                .Select(x => new
                {
                    ID = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Name = x.Title,
                    PoliticalProvinceCaption = x.CountryDivisionId
                })
                .FirstOrDefault();
            var oldCode = _oracleCommon.OldColumnValue("HRS.TBCTAMIN_BRANCH", "CODE", pensionFundBranch.ID.ToString());
            var oldPliticalProcinceCaption = _oracleCommon.OldColumnValue("HRS.TBCTAMIN_BRANCH", "ID", pensionFundBranch.PoliticalProvinceCaption.ToString());
            var TBCTAMIN_BRANCH_Queryable = _TBCTAMIN_BRANCHRepository.GetQueryable();
            var entity = TBCTAMIN_BRANCH_Queryable
                .Where(x => x.CODE == oldCode)
                .ToList()
                .FirstOrDefault();

            _TBCTAMIN_BRANCHRepository.Delete(entity);
            _TBCTAMIN_BRANCHRepository.SaveChanges();
        }        
    }
}
