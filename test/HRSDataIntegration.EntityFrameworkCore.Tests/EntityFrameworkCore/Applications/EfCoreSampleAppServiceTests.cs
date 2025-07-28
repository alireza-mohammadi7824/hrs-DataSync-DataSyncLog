using HRSDataIntegration.Samples;
using Xunit;

namespace HRSDataIntegration.EntityFrameworkCore.Applications;

[Collection(HRSDataIntegrationTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<HRSDataIntegrationEntityFrameworkCoreTestModule>
{

}
