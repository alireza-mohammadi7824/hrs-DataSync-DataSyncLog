using HRSDataIntegration.Interfaces.ManagementService;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services.ManagementService
{
    public class HRSManagementService : IHRSManagementService
    {
        private readonly IFileProvider _fileProvider;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<HRSManagementService> _logger;

        public HRSManagementService(IHostEnvironment hostEnvironment, ILogger<HRSManagementService> logger)
        {
            _hostEnvironment = hostEnvironment;
            _fileProvider = hostEnvironment.ContentRootFileProvider;
            _logger = logger;
        }

        //[Authorize(HRSDataIntegrationPermissions.Management.GetLogFileContent)]
        public async Task<string> GetLogContentAsync()
        {
            var fileInfo = _fileProvider.GetFileInfo("Logs/logs.txt");

            if (!fileInfo.Exists)
                throw new FileNotFoundException("Log file not found.");

            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
