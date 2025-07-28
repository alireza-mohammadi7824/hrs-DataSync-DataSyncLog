using Volo.Abp.Modularity;

namespace HRSDataIntegration;

[DependsOn(
    typeof(HRSDataIntegrationApplicationModule),
    typeof(HRSDataIntegrationDomainTestModule)
)]
public class HRSDataIntegrationApplicationTestModule : AbpModule
{

}
