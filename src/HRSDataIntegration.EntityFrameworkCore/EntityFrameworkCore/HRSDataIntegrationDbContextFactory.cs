using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HRSDataIntegration.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class HRSDataIntegrationDbContextFactory : IDesignTimeDbContextFactory<HRSDataIntegrationDbContext>
{
      public HRSDataIntegrationDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        HRSDataIntegrationEfCoreEntityExtensionMappings.Configure();

        // تنظیم DbContextOptionsBuilder برای SQL Server
        var sqlServerBuilder = new DbContextOptionsBuilder<HRSDataIntegrationDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"),
            x=> x.UseHierarchyId());

        // تنظیم DbContextOptionsBuilder برای Oracle
        var oracleBuilder = new DbContextOptionsBuilder<HRSDataIntegrationDbContext>()
            .UseOracle(configuration.GetConnectionString("OracleDatabase"));

        // انتخاب پایگاه داده بر اساس آرگومان ورودی یا تنظیمات دیگر
        var useOracle = args.Contains("--use-oracle"); // یک فلگ ورودی به عنوان نمونه

        var options = useOracle ? oracleBuilder.Options : sqlServerBuilder.Options;

        return new HRSDataIntegrationDbContext(options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../HRSDataIntegration.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
