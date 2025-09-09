namespace HRSDataIntegration.Permissions;

public static class HRSDataIntegrationPermissions
{
    public const string GroupName = "HRSDataIntegration";

    public static class Management
    {
        public const string ManagementGroup = GroupName + ".Management";
        public const string GetLogFileContent = ManagementGroup + ".GetLogFileContent";
    }


    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}
