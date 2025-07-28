using Xunit;

namespace HRSDataIntegration.EntityFrameworkCore;

[CollectionDefinition(HRSDataIntegrationTestConsts.CollectionDefinitionName)]
public class HRSDataIntegrationEntityFrameworkCoreCollection : ICollectionFixture<HRSDataIntegrationEntityFrameworkCoreFixture>
{

}
