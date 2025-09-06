
using HRSDataIntegration;
using HRSDataIntegration.DTOs;
using HRSDataIntegration.EntityFrameworkCore;
using HRSDataIntegration.EntityFrameworkCore.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace HRSToHRDataConverter.Common
{
    public class OracleCommon: IOracleCommon
    {
        private readonly OracleDbContext _oracleDbContext;
        private readonly HRSDataIntegrationDbContext _sQLDatabaseContext;
        public OracleCommon(OracleDbContext oracleDbContext, HRSDataIntegrationDbContext sQLDatabaseContext)
        {
            _oracleDbContext = oracleDbContext;
            _sQLDatabaseContext = sQLDatabaseContext;
        }
        //   برای اینسرت در جدول MappingId
        public void InsertInto_DataConverter_MappingId(string OracleId, string SqlID, string OldTableName, string OldColumnName, string newTableName, string newColumnName)
        {
            var mappingId = new MappingId()
            {
                Id =Guid.NewGuid(),
                ConvertDateTime = DateTime.Now,
                SubsystemCode = 1,
                OldTableName = OldTableName,
                OldColumnName = OldColumnName,
                NewTableName = newTableName,
                NewColumnName = newColumnName,
                OldColumnValue = OracleId.ToString(),
                NewColumnValue = SqlID.ToString(),
            };

            _sQLDatabaseContext.Add(mappingId);
            _sQLDatabaseContext.SaveChanges();
        }
        public string Get_Old_ColumnValue(string SqlID, string OldTableName, string OldColumnName, string newTableName, string newColumnName)
        {
            return _sQLDatabaseContext.MappingId.Where(x =>
            x.OldTableName == OldTableName &&
            x.OldColumnName == OldColumnName
            && x.NewTableName == newTableName
            && x.NewColumnName == newColumnName
            && x.NewColumnValue == SqlID
            ).FirstOrDefault().OldColumnValue;

        }
        public void Update_DataConverter_MappingId(string OracleId, string SqlID, string OldTableName, string OldColumnName, string newTableName, string newColumnName)
        {
            var entity = _sQLDatabaseContext.MappingId.Where(x =>
            x.OldTableName == OldTableName &&
            x.OldColumnName == OldColumnName
            && x.NewTableName == newTableName
            && x.NewColumnName == newColumnName
            && x.NewColumnValue == SqlID
            ).FirstOrDefault();

            if (entity != null)
            {
                entity.OldColumnValue = OracleId.ToString();
            };
            
        }
        //برای گرفتن مقدار Id مربوط به جدول پایگاه اوراکل
        public string OldColumnValue(string oldTableName, string oldColumnName, string newColumnValue)
        {
          return  _sQLDatabaseContext.MappingId
                            .Where(x => x.NewColumnValue == newColumnValue && x.OldTableName.Contains(oldTableName) && x.OldColumnName == oldColumnName)
                            .Select(x => x.OldColumnValue)
                            .FirstOrDefault();
        }
        public string DomainMappingCodeOldColumnValue(string oldTableName, string oldColumnName, string newColumnValue)
        {
            return _sQLDatabaseContext.MappingId
                .Where(x => x.NewColumnValue == newColumnValue && x.OldTableName.Contains(oldTableName) && x.OldColumnName == "MappingDomainCode")
                .Select(x => x.OldColumnValue)
                .FirstOrDefault();
        }

        //  برای اینسرت کردن در جدول لاگ
        public  void TBActivity_Log(string LogTableName, string ID, int Document_Code, Int64 Domain_Code)
        {
            // پیدا کردن نوع کلاس مرتبط با نام جدول
            // var tableType = Type.GetType($"HRSDataIntegration.DTOs.{LogTableName}");
            var tableType = AppDomain.CurrentDomain.GetAssemblies()
             .SelectMany(a => a.GetTypes())
             .FirstOrDefault(t => t.Name == LogTableName);
                if (tableType == null)
                {
                    throw new Exception("Table type not found!");
                }

            // ایجاد نمونه‌ای از کلاس لاگ تیبل نیم ورودی به صورت پویا
            var log = Activator.CreateInstance(tableType,
                Guid.NewGuid(),                                       // ID یا شناسه
                ID,                                                  // DOC_ID
                DateTime.Now,                                       // تاریخ ایجاد
                "بروز رسانی از سامانه جدید منابع انسانی",         // توضیحات--DESCRIPTION
                "sync_New_System",                                // USER_NAME
                Domain_Code,                                     // DOMAIN_CODE
                "d2c799d7-7046-43eb-9e77-c3c36e42cb94",         // شناسه ثابت--ACTIVITY_ID
                null,                                          // DOC_VALUE_ID
                Document_Code                                 // DOCUMENT_CODE
            );
            _oracleDbContext.Add(log);
            _oracleDbContext.SaveChanges();
        }
        //تبدیل تاریخ از تایپ  int  به  string
        public  string ToStringDateTime(int? dateInt)
        {
            string dateString = dateInt.ToString();
            var length = dateString.Length;

            // بررسی فرمت ورودی
            if (length != 8)
                throw new ArgumentException("Invalid date format. Expected YYYYMMDD.");

            // استخراج سال، ماه و روز
            int year = int.Parse(dateString.Substring(0, 4));
            int month = int.Parse(dateString.Substring(4, 2));
            int day = int.Parse(dateString.Substring(6, 2));

            

            return $"{year}/{month:D2}/{day:D2}";
        }
        public  int GetTableCount(string tableName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OracleDbContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            optionsBuilder.UseOracle(configuration.GetConnectionString("OracleDatabase"));
            using var context = new OracleDbContext(optionsBuilder.Options);
            var property = typeof(OracleDbContext).GetProperty(tableName);
            if (property == null) throw new ArgumentException($"جدولی با نام '{tableName}' پیدا نشد.");

            var dbSet = property.GetValue(context);
            var countMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "Count" && m.GetParameters().Length == 1)
                .MakeGenericMethod(property.PropertyType.GenericTypeArguments[0]);

            return (int)countMethod.Invoke(null, new object[] { dbSet });
        }

        public void UpdateDataSyncLog(Guid id, bool isSuccedded, string Exception = null)
        {
            var entity  = _sQLDatabaseContext.DataSyncLog
                                            .Where(x => x.RecordId == id 
                                                     && x.IsDone == null)
                                            .ToList()
                                            .FirstOrDefault();

            if (entity == null)
            {
                return;
            }
            else if (entity != null && isSuccedded)
            {
                entity.IsDone = true;
                entity.LastModificationTime = DateTime.Now.ToString();
            }
            else if (entity != null && !isSuccedded) {
                entity.IsDone = false;
                entity.ExceptionMessage = Exception;
                entity.LastModificationTime = DateTime.Now.ToString();
            }

            _sQLDatabaseContext.SaveChanges();
        }
    }
}
