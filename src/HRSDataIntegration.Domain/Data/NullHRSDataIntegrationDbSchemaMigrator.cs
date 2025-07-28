using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HRSDataIntegration.Data;

/* This is used if database provider does't define
 * IHRSDataIntegrationDbSchemaMigrator implementation.
 */
public class NullHRSDataIntegrationDbSchemaMigrator : IHRSDataIntegrationDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
