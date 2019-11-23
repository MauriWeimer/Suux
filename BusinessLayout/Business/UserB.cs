using Data.Context;
using System.Linq;

namespace BusinessLayout.Business
{
    public class UserB
    {
        public static Users SelectUser(string user)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Users.Where(u => u.user_name == user.ToUpper()).First();
            }
        }
    }
}
