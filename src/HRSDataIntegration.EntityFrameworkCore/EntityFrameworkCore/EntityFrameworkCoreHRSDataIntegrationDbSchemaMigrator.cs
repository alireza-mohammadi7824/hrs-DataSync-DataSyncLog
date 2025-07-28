using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HRSDataIntegration.Data;
using Volo.Abp.DependencyInjection;

namespace HRSDataIntegration.EntityFrameworkCore;

public class EntityFrameworkCoreHRSDataIntegrationDbSchemaMigrator
    : IHRSDataIntegrationDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreHRSDataIntegrationDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the HRSDataIntegrationDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<HRSDataIntegrationDbContext>()
            .Database
            .MigrateAsync();
    }
}
