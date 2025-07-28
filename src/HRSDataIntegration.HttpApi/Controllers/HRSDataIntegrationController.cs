using HRSDataIntegration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace HRSDataIntegration.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class HRSDataIntegrationController : AbpControllerBase
{
    protected HRSDataIntegrationController()
    {
        LocalizationResource = typeof(HRSDataIntegrationResource);
    }
}
