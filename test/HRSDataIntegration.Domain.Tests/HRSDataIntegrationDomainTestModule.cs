using Volo.Abp.Modularity;

namespace HRSDataIntegration;

[DependsOn(
    typeof(HRSDataIntegrationDomainModule),
    typeof(HRSDataIntegrationTestBaseModule)
)]
public class HRSDataIntegrationDomainTestModule : AbpModule
{

}
