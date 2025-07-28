using System.Threading.Tasks;

namespace HRSDataIntegration.Data;

public interface IHRSDataIntegrationDbSchemaMigrator
{
    Task MigrateAsync();
}
