using System.Linq;
using LogMeIn.Models;

namespace LogMeIn
{
    class UserProfileManager : IUserProfileManager
    {
        public bool Exists(string userName)
        {
            using (UsersContext db = new UsersContext())
            {
                return db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower()) != null;
            }
        }

        public void Add(string userName)
        {
            using (UsersContext db = new UsersContext())
            {                
                db.UserProfiles.Add(new UserProfile {UserName = userName});
                db.SaveChanges();
            }
        }
    }
}