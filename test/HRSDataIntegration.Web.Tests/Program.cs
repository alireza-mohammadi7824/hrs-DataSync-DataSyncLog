using Microsoft.AspNetCore.Builder;
using HRSDataIntegration;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<HRSDataIntegrationWebTestModule>();

public partial class Program
{
}
