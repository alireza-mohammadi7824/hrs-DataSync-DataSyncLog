using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace HRSDataIntegration.Interfaces.ManagementService
{
    public interface IHRSManagementService : IApplicationService, ITransientDependency
    {
        Task<string> GetLogContentAsync();
        //Task ClearLogContent();
        //Task ClearLogContentWithSize(long? maxSizeInBytes = null);
    }
}
