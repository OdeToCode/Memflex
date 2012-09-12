using WebMatrix.WebData;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(LogMeIn.App_Start.MembershipConfig), "Initialize")]

namespace LogMeIn.App_Start
{
    public class MembershipConfig
    {
        public static void Initialize()
        {
            WebSecurity.InitializeDatabaseConnection(
                connectionStringName:"DefaultConnection", 
                userTableName:"UserProfileManager", 
                userIdColumn:"UserId", 
                userNameColumn:"UserName", 
                autoCreateTables: true);
        } 
    }
}