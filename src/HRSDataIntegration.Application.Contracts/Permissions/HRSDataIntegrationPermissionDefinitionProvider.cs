using HRSDataIntegration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace HRSDataIntegration.Permissions;

public class HRSDataIntegrationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HRSDataIntegrationPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(HRSDataIntegrationPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HRSDataIntegrationResource>(name);
    }
}
