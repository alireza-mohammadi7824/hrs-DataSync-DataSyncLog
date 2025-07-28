using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace HRSDataIntegration.Pages;

[Collection(HRSDataIntegrationTestConsts.CollectionDefinitionName)]
public class Index_Tests : HRSDataIntegrationWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
