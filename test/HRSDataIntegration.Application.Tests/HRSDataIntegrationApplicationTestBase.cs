using Volo.Abp.Modularity;

namespace HRSDataIntegration;

public abstract class HRSDataIntegrationApplicationTestBase<TStartupModule> : HRSDataIntegrationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
