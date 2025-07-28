using Volo.Abp.Settings;

namespace HRSDataIntegration.Settings;

public class HRSDataIntegrationSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(HRSDataIntegrationSettings.MySetting1));
    }
}
