using HRSDataIntegration.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace HRSDataIntegration.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(HRSDataIntegrationEntityFrameworkCoreModule),
    typeof(HRSDataIntegrationApplicationContractsModule)
)]
public class HRSDataIntegrationDbMigratorModule : AbpModule
{
}
