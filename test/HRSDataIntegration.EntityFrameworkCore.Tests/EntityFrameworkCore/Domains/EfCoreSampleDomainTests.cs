using HRSDataIntegration.Samples;
using Xunit;

namespace HRSDataIntegration.EntityFrameworkCore.Domains;

[Collection(HRSDataIntegrationTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<HRSDataIntegrationEntityFrameworkCoreTestModule>
{

}
