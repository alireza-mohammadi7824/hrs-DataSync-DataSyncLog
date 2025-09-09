using HRSDataIntegration.EntityFrameworkCore;
using HRSDataIntegration.Interfaces;
using HRSDataIntegration.Services;
using HRSToHRDataConverter.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;


namespace HRSDataIntegration.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddTransient<IJobService, JobService>();
            builder.Services.AddTransient<IPostService, PostService>();
            builder.Services.AddTransient<IUnitService, UnitService>();
            builder.Services.AddTransient<IUnitLevelService, UnitLevelService>();
            builder.Services.AddTransient<IChartService, ChartService>();
            builder.Services.AddTransient<IProvinceService, ProvinceService>();
            builder.Services.AddTransient<INotFamilyRightReasonService, NotFamilyRightReasonService>();
            builder.Services.AddTransient<IStudyFieldService, StudyFieldService>();
            builder.Services.AddTransient<IPensionFundService, PensionFundService>();
            builder.Services.AddTransient<IUniversityService, UniversityService>();
            builder.Services.AddTransient<IPensionFundBranchService, PensionFundBranchService>();
            builder.Services.AddTransient<INotDependentReasonService, NotDependentReasonService>();
            builder.Services.AddTransient<IPersonEducationService, PersonEducationService>();
            builder.Services.AddTransient<IEmployeePensionFundService, EmployeePensionFundService>();
            builder.Services.AddTransient<IPersonDetailService, PersonDetailService>();
            builder.Services.AddTransient<ILanguageService, LanguageService>();
            builder.Services.AddTransient<IPersonContactService, PersonContactService>();
            builder.Services.AddTransient<IEmployeeDependentStatusService, EmployeeDependentStatusService>();
            builder.Services.AddScoped(typeof(IOracleRepository<>), typeof(RepositoryBaseOracle<>));
            builder.Services.AddScoped(typeof(ISqlRepository<>), typeof(RepositoryBaseSQL<>));
            builder.Services.AddScoped<IOracleCommon, OracleCommon>();

            var config = builder.Configuration;

            var logLevelString = config["Logging:LogLevel:Default"] ?? "Warning";
            var logLevel = Enum.TryParse<LogEventLevel>(logLevelString, true, out var level)
                ? level
                : LogEventLevel.Warning;

            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
#if DEBUG
                        .MinimumLevel.Is(logLevel)
#else
            .MinimumLevel.Information()
#endif
                        //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        //.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Async(c => c.File("Logs/logs.txt"))
                        .WriteTo.Async(c => c.Console())
                        .WriteTo.Async(c => c.AbpStudio(services));
                });
            await builder.AddApplicationAsync<HRSDataIntegrationWebModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
