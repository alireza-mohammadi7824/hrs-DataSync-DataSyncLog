using HRSDataIntegration.DTOs.Personeli;
using HRSDataIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class EmployeeDependentStatusService : IEmployeeDependentStatusService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBNON_DEPENDENT_REASON> _TBNON_DEPENDENT_REASONRepository;
        private readonly IOracleRepository<TBNON_LIABILITY_REASON> _TBNON_LIABILITY_REASONRepository;
        private readonly ISqlRepository<EmployeeDependentStatus> _sqlRepositoryEmployeeDependentStatus;
        public EmployeeDependentStatusService(IOracleCommon oracleCommon,
            IOracleRepository<TBNON_DEPENDENT_REASON> TBNON_DEPENDENT_REASONRepository,
            IOracleRepository<TBNON_LIABILITY_REASON> TBNON_LIABILITY_REASONRepository,
            ISqlRepository<EmployeeDependentStatus> sqlRepositoryEmployeeDependentStatus)
        {
            _oracleCommon = oracleCommon;
            _TBNON_DEPENDENT_REASONRepository = TBNON_DEPENDENT_REASONRepository;
            _TBNON_LIABILITY_REASONRepository = TBNON_LIABILITY_REASONRepository;
            _sqlRepositoryEmployeeDependentStatus = sqlRepositoryEmployeeDependentStatus;
        }
        

        public void InsertEmployeeDependentStatusServiceFromSqlToOracle(string ID) //EmployeeDependentStatusId
        {
            try
            {
                var employeeDependentStatusQueryable = _sqlRepositoryEmployeeDependentStatus.GetQueryable();
                var employeeDependentStatus = employeeDependentStatusQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Title = x.Title,
                    })
                    .FirstOrDefault();

                var TBNON_DEPENDENT_REASON = new TBNON_DEPENDENT_REASON()
                {
                    ID = employeeDependentStatus.Id.ToString(),
                    CODE = employeeDependentStatus.Code,
                    NAME = employeeDependentStatus.Title
                };

                var TBNON_LIABILITY_REASON = new TBNON_LIABILITY_REASON()
                {
                    ID = employeeDependentStatus.Id.ToString(),
                    CODE = employeeDependentStatus.Code,
                    NAME = employeeDependentStatus.Title
                };

                _TBNON_DEPENDENT_REASONRepository.Create(TBNON_DEPENDENT_REASON);
                _TBNON_LIABILITY_REASONRepository.Create(TBNON_LIABILITY_REASON);
                _TBNON_DEPENDENT_REASONRepository.SaveChanges();
                _TBNON_LIABILITY_REASONRepository.SaveChanges();

                _oracleCommon.InsertInto_DataConverter_MappingId(employeeDependentStatus.Id.ToString(), ID, "HRS.TBNON_DEPENDENT_REASON", "ID", "Employee.EmployeeDependentStatus", "ID");
                _oracleCommon.InsertInto_DataConverter_MappingId(employeeDependentStatus.Id.ToString(), ID, "HRS.TBNON_LIABILITY_REASON", "ID", "Employee.EmployeeDependentStatus", "ID");
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

        public void UpdateEmployeeDependentStatusServiceFromSqlToOracle(string ID) //EmployeeDependentStatusId
        {
            try
            {
                var employeeDependentStatusQueryable = _sqlRepositoryEmployeeDependentStatus.GetQueryable();
                var employeeDependentStatus = employeeDependentStatusQueryable
                    .Where(x=>x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Title,
                    })
                    .FirstOrDefault();

                var oldId = _oracleCommon.OldColumnValue("HRS.TBNON_LIABILITY_REASON", "ID", employeeDependentStatus.Id.ToString());

                var tBNON_LIABILITY_REASON_Queryable = _TBNON_LIABILITY_REASONRepository.GetQueryable();
                var entity = tBNON_LIABILITY_REASON_Queryable
                    .Where(x=>x.ID.ToString() == employeeDependentStatus.Id.ToString()) 
                   // .Where(x=>x.ID.ToString() == oldId) 
                   .ToList()
                   .FirstOrDefault();

                entity.CODE = employeeDependentStatus.Code;
                entity.NAME = employeeDependentStatus.Name;

                _TBNON_LIABILITY_REASONRepository.SaveChanges();

                var tBNON_DEPENDENT_REASON_Queryable = _TBNON_DEPENDENT_REASONRepository.GetQueryable();
                var entityDependent = tBNON_DEPENDENT_REASON_Queryable
                    .Where (x=>x.ID.ToString() != employeeDependentStatus.Id.ToString())
                    .ToList()
                    .FirstOrDefault();

                entityDependent.CODE = employeeDependentStatus.Code;
                entityDependent.NAME = employeeDependentStatus.Name;

                _TBNON_DEPENDENT_REASONRepository.SaveChanges();
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

        public void DeleteEmployeeDependentStatusServiceFromSqlToOracle(string ID) //EmployeeDependentStatusId
        {
            try
            {
                var employeeDependentStatusQueryable = _sqlRepositoryEmployeeDependentStatus.GetQueryable();
                var employeeDependentStatus = employeeDependentStatusQueryable
                    .Where(x => x.Id.ToString() == ID)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Title,
                    })
                    .FirstOrDefault();

                var oldId = _oracleCommon.OldColumnValue("HRS.TBNON_LIABILITY_REASON", "ID", employeeDependentStatus.Id.ToString());

                var tBNON_LIABILITY_REASON_Queryable = _TBNON_LIABILITY_REASONRepository.GetQueryable();
                var entity = tBNON_LIABILITY_REASON_Queryable
                    .Where(x => x.ID.ToString() == employeeDependentStatus.Id.ToString())
                   // .Where(x=>x.ID.ToString() == oldId) 
                   .ToList()
                   .FirstOrDefault();

                _TBNON_LIABILITY_REASONRepository.Delete(entity);
                _TBNON_LIABILITY_REASONRepository.SaveChanges();

                var tBNON_DEPENDENT_REASON_Queryable = _TBNON_DEPENDENT_REASONRepository.GetQueryable();
                var entityDependent = tBNON_DEPENDENT_REASON_Queryable
                    .Where(x => x.ID.ToString() == employeeDependentStatus.Id.ToString())
                    .ToList()
                    .FirstOrDefault();

                _TBNON_DEPENDENT_REASONRepository.Delete(entityDependent);

                _TBNON_DEPENDENT_REASONRepository.SaveChanges();
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
    }
}
