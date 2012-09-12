using System.Data.Entity.Infrastructure;
using LogMeIn.Models;
using WebMatrix.WebData;

//[assembly: WebActivator.PreApplicationStartMethod(
//    typeof(LogMeIn.App_Start.MembershipConfig), "Initialize")]

namespace LogMeIn.App_Start
{
    public class MembershipConfig
    {
        public static void Initialize()
        {
            // TODO: figure out database initialization
            using (var context = new UsersContext())
            {
                if (!context.Database.Exists())
                {                    
                    context.Database.Create();                    
                }
            }

            WebSecurity.InitializeDatabaseConnection(
                connectionStringName:"DefaultConnection", 
                userTableName:"UserProfileManager", 
                userIdColumn:"UserId", 
                userNameColumn:"UserName", 
                autoCreateTables: true);
        } 
    }
}