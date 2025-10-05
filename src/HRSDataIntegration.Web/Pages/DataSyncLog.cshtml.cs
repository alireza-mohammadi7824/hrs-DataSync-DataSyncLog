using HRSDataIntegration.DTOs;                  
using HRSDataIntegration.Services.DataSyncLogs; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsDoneFilter { get; set; } 

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        [BindProperty(SupportsGet = true, Name = "p")]
        public int Page { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 15;

        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

       
        public IEnumerable<SelectListItem> StatusItems => new[]
        {
            new SelectListItem("All",     ""),      
            new SelectListItem("Done",    "true"),
            new SelectListItem("Not Done","false")
        };

        public IEnumerable<SelectListItem> PageSizeItems => new[]
        {
            new SelectListItem("15",  "15"),
            new SelectListItem("25",  "25"),
            new SelectListItem("50",  "50"),
            new SelectListItem("100", "100")
        };

        public DataSyncLogModel(IDataSyncLogAppService dataSyncLogAppService)
        {
            _dataSyncLogAppService = dataSyncLogAppService;
        }

        public async Task OnGetAsync()
        {
      
            var data = await _dataSyncLogAppService.LoadLog();
            var queryable = (data ?? new List<DataSyncLog>()).AsQueryable();

        
            if (!string.IsNullOrWhiteSpace(Query))
            {
                var q = Query.Trim();
                bool isGuid = Guid.TryParse(q, out Guid qGuid);

                queryable = queryable.Where(x =>
                
                    (!string.IsNullOrEmpty(x.Type) && x.Type.Contains(q, StringComparison.OrdinalIgnoreCase))
             
                    || (!string.IsNullOrEmpty(x.ExceptionMessage) && x.ExceptionMessage.Contains(q, StringComparison.OrdinalIgnoreCase))

                    || (isGuid && (
                           x.Id == qGuid
                        || (x.CreatorId.HasValue && x.CreatorId.Value == qGuid)
                        || (x.LastModifierId.HasValue && x.LastModifierId.Value == qGuid)
                        || (x.DeleterId.HasValue && x.DeleterId.Value == qGuid)
                    ))
                 
                    || x.Id.ToString().Contains(q, StringComparison.OrdinalIgnoreCase)
                    || (x.CreatorId.HasValue && x.CreatorId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (x.LastModifierId.HasValue && x.LastModifierId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (x.DeleterId.HasValue && x.DeleterId.Value.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                );
            }

      
            if (IsDoneFilter == true)
            {
                queryable = queryable.Where(x => x.IsDone == true);
            }
            else if (IsDoneFilter == false)
            {
                queryable = queryable.Where(x => x.IsDone == false);
            }
            if (FromDate.HasValue)
                queryable = queryable.Where(x => x.CreationTime >= FromDate.Value);

            if (ToDate.HasValue)
            {
                var toInclusive = ToDate.Value.Date.AddDays(1).AddTicks(-1);
                queryable = queryable.Where(x => x.CreationTime <= toInclusive);
            }

            queryable = SortOrder switch
            {
                "CreationTime" => queryable.OrderBy(l => l.CreationTime),
                "CreationTime_desc" => queryable.OrderByDescending(l => l.CreationTime),

                "LastModificationTime" => queryable.OrderBy(l => l.LastModificationTime ?? DateTime.MinValue),
                "LastModificationTime_desc" => queryable.OrderByDescending(l => l.LastModificationTime ?? DateTime.MinValue),

                "Type" => queryable.OrderBy(l => l.Type),
                "Type_desc" => queryable.OrderByDescending(l => l.Type),

                "IsDone" => queryable.OrderBy(l => (l.IsDone == true)),
                "IsDone_desc" => queryable.OrderByDescending(l => (l.IsDone == true)),

                _ => queryable.OrderByDescending(l => l.CreationTime)
            };

            TotalCount = queryable.Count();

            if (PageSize < 1) PageSize = 15;
            if (Page < 1) Page = 1;
            if (TotalPages > 0 && Page > TotalPages) Page = TotalPages;

            Logs = queryable
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public string GetSortOrder(string column)
            => SortOrder == column ? column + "_desc" : column;

        public string SortIcon(string column)
        {
            if (SortOrder == column) return "🔼";            
            if (SortOrder == column + "_desc") return "🔽"; 
            return "";                                       
        }
    }
}
