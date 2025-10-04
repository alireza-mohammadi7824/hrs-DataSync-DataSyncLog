using HRSDataIntegration.DTOs; // اگر مستقیم DTO می‌آوری
using HRSDataIntegration.Services.DataSyncLogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRSDataIntegration.Web.Pages
{
    public class DataSyncLogModel : PageModel
    {
        private readonly IDataSyncLogAppService _dataSyncLogAppService;
        public List<DataSyncLog> Logs { get; set; } = new();

        // -------- Search & Filters (GET) --------
        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsDoneFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }  // فیلد تاریخ از
        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }    // فیلد تاریخ تا (شامل همین روز)

        // -------- Sorting (GET) --------
        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        // -------- Paging (GET) --------
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
            var data = await _dataSyncLogAppService.LoadLog();
            Logs = data ?? new();

            // -------- FILTERS --------
            if (!string.IsNullOrWhiteSpace(Query))
            {
                var q = Query.Trim();
                bool isGuid = Guid.TryParse(q, out Guid qGuid);

                Logs = Logs.Where(x =>
                       (!string.IsNullOrEmpty(x.Type) && x.Type.Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(x.ExceptionMessage) && x.ExceptionMessage.Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (isGuid && (
                           x.Id == qGuid
                        || (x.CreatorId.HasValue && x.CreatorId.Value == qGuid)
                        || (x.LastModifierId.HasValue && x.LastModifierId.Value == qGuid)
                        || (x.DeleterId.HasValue && x.DeleterId.Value == qGuid)
                    ))
                    // اختیاری: جست‌وجوی بخشی روی Guid‌ها
                    || x.Id.ToString().Contains(q, StringComparison.OrdinalIgnoreCase)
                    || (x.CreatorId.HasValue && x.CreatorId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (x.LastModifierId.HasValue && x.LastModifierId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (x.DeleterId.HasValue && x.DeleterId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (IsDoneFilter.HasValue)
                Logs = Logs.Where(x => x.IsDone == IsDoneFilter.Value).ToList();

            if (FromDate.HasValue)
                Logs = Logs.Where(x => x.CreationTime >= FromDate.Value).ToList();

            if (ToDate.HasValue)
            {
                // شامل کل روز ToDate
                var to = ToDate.Value.Date.AddDays(1).AddTicks(-1);
                Logs = Logs.Where(x => x.CreationTime <= to).ToList();
            }

            // -------- SORT --------
            Logs = SortOrder switch
            {
                "CreationTime" => Logs.OrderBy(l => l.CreationTime).ToList(),
                "CreationTime_desc" => Logs.OrderByDescending(l => l.CreationTime).ToList(),

                "LastModificationTime" => Logs.OrderBy(l => l.LastModificationTime).ToList(),
                "LastModificationTime_desc" => Logs.OrderByDescending(l => l.LastModificationTime).ToList(),

                "Type" => Logs.OrderBy(l => l.Type).ToList(),
                "Type_desc" => Logs.OrderByDescending(l => l.Type).ToList(),

                "IsDone" => Logs.OrderBy(l => l.IsDone).ToList(),
                "IsDone_desc" => Logs.OrderByDescending(l => l.IsDone).ToList(),

                _ => Logs.OrderByDescending(l => l.CreationTime).ToList()
            };

            // -------- PAGING --------
            TotalCount = Logs.Count;
            if (Page < 1) Page = 1;
            if (PageSize < 1) PageSize = 15;

            Logs = Logs
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public string GetSortOrder(string column)
            => SortOrder == column ? column + "_desc" : column;

        public string SortIcon(string column)
        {
            if (SortOrder == column) return "▲";            // ascending
            if (SortOrder == column + "_desc") return "▼";  // descending
            return "";                                      // no sort
        }
    }
}
