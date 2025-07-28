using HRSDataIntegration.Localization;
using Volo.Abp.Application.Services;

namespace HRSDataIntegration;

/* Inherit your application services from this class.
 */
public abstract class HRSDataIntegrationAppService : ApplicationService
{
    protected HRSDataIntegrationAppService()
    {
        LocalizationResource = typeof(HRSDataIntegrationResource);
    }
}
