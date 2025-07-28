using Volo.Abp.Modularity;

namespace HRSDataIntegration;

/* Inherit from this class for your domain layer tests. */
public abstract class HRSDataIntegrationDomainTestBase<TStartupModule> : HRSDataIntegrationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
