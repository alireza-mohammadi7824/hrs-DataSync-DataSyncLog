using HRSDataIntegration.DTOs;                  // خروجی LoadLog
using HRSDataIntegration.Services.DataSyncLogs; // سرویس اپلیکیشن
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRSDataIntegration.Web.Pages
{
    public class DataSyncLogModel : PageModel
    {
        private readonly IDataSyncLogAppService _dataSyncLogAppService;

        // لیست نمایش صفحه (بعد از فیلتر/سورت/صفحه‌بندی)
        public List<DataSyncLog> Logs { get; set; } = new();

        // ---------- فیلترها (GET) ----------
        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsDoneFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        // ---------- سورت (GET) ----------
        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        // ---------- صفحه‌بندی (GET) ----------
        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 15;

        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public DataSyncLogModel(IDataSyncLogAppService dataSyncLogAppService)
        {
            _dataSyncLogAppService = dataSyncLogAppService;
        }

        public async Task OnGetAsync()
        {
            // 1) دریافت کل داده‌ها
            var data = await _dataSyncLogAppService.LoadLog();
            var queryable = (data ?? new List<DataSyncLog>()).AsQueryable();

            // 2) فیلترهای متنی و GUID
            if (!string.IsNullOrWhiteSpace(Query))
            {
                var q = Query.Trim();
                bool isGuid = Guid.TryParse(q, out Guid qGuid);

                queryable = queryable.Where(x =>
                       (!string.IsNullOrEmpty(x.Type) && x.Type.Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(x.ExceptionMessage) && x.ExceptionMessage.Contains(q, StringComparison.OrdinalIgnoreCase))

                    // جست‌وجوی دقیق روی GUID اگر ورودی، GUID کامل بود
                    || (isGuid && (
                           x.Id == qGuid
                        || (x.CreatorId.HasValue && x.CreatorId.Value == qGuid)
                        || (x.LastModifierId.HasValue && x.LastModifierId.Value == qGuid)
                        || (x.DeleterId.HasValue && x.DeleterId.Value == qGuid)
                    ))

                    // جست‌وجوی بخشی روی GUID ها (اختیاری اما کاربردی)
                    || x.Id.ToString().Contains(q, StringComparison.OrdinalIgnoreCase)
                    || (x.CreatorId.HasValue && x.CreatorId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (x.LastModifierId.HasValue && x.LastModifierId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (x.DeleterId.HasValue && x.DeleterId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                );
            }

            // 3) فیلتر وضعیت
            if (IsDoneFilter.HasValue)
                queryable = queryable.Where(x => x.IsDone == IsDoneFilter.Value);

            // 4) فیلتر بازه تاریخ ایجاد
            if (FromDate.HasValue)
                queryable = queryable.Where(x => x.CreationTime >= FromDate.Value);

            if (ToDate.HasValue)
            {
                var toInclusive = ToDate.Value.Date.AddDays(1).AddTicks(-1);
                queryable = queryable.Where(x => x.CreationTime <= toInclusive);
            }

            // 5) سورت (با درنظرگرفتن nullable بودن LastModificationTime)
            queryable = SortOrder switch
            {
                "CreationTime" => queryable.OrderBy(l => l.CreationTime),
                "CreationTime_desc" => queryable.OrderByDescending(l => l.CreationTime),

                "LastModificationTime" => queryable.OrderBy(l => l.LastModificationTime ?? DateTime.MinValue),
                "LastModificationTime_desc" => queryable.OrderByDescending(l => l.LastModificationTime ?? DateTime.MinValue),

                "Type" => queryable.OrderBy(l => l.Type),
                "Type_desc" => queryable.OrderByDescending(l => l.Type),

                "IsDone" => queryable.OrderBy(l => l.IsDone),
                "IsDone_desc" => queryable.OrderByDescending(l => l.IsDone),

                _ => queryable.OrderByDescending(l => l.CreationTime)
            };

            // 6) محاسبه تعداد کل قبل از Paging
            TotalCount = queryable.Count();

            
            if (PageSize < 1) PageSize = 15;
            if (Page < 1) Page = 1;
            if (TotalPages > 0 && Page > TotalPages) Page = TotalPages; // اگر صفحه بزرگتر از آخرین صفحه بود، برگردون

            // 8) اعمال صفحه‌بندی
            Logs = queryable
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public string GetSortOrder(string column)
            => SortOrder == column ? column + "_desc" : column;

        // آیکون‌های تمیزتر برای سورت
        public string SortIcon(string column)
        {
            if (SortOrder == column) return "🔼";            // صعودی
            if (SortOrder == column + "_desc") return "🔽";  // نزولی
            return "";                                       // بدون سورت
        }
    }
}
