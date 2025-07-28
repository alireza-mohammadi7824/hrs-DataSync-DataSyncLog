using HRSDataIntegration.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace HRSDataIntegration.Web.Pages;

public abstract class HRSDataIntegrationPageModel : AbpPageModel
{
    protected HRSDataIntegrationPageModel()
    {
        LocalizationResourceType = typeof(HRSDataIntegrationResource);
    }
}
