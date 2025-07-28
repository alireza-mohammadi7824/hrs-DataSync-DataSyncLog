using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using HRSDataIntegration.Localization;

namespace HRSDataIntegration.Web;

[Dependency(ReplaceServices = true)]
public class HRSDataIntegrationBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<HRSDataIntegrationResource> _localizer;

    public HRSDataIntegrationBrandingProvider(IStringLocalizer<HRSDataIntegrationResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
