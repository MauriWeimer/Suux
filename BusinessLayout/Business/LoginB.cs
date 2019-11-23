using Data.Context;
using System.Linq;

namespace BusinessLayout.Business
{
    public class LoginB
    {
        public static int VerifyUser(string us, string pw)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                if (string.IsNullOrWhiteSpace(us))
                {
                    //Wrong access
                    return 11;
                }
                else if (string.IsNullOrEmpty(pw))
                {
                    //Wrong access
                    return 12;
                }

                Users u = ent.Users.FirstOrDefault(ux => ux.user_name == us.ToUpper());

                if (u != null)
                {
                    if (u.user_password == pw)
                    {     
                        //Good access
                        return 1;
                    }
                    else
                    {
                        //Wrong access
                        return 13;
                    }
                }
                else
                {
                    //Wrong access                    
                    return 14;
                }
            }
        }
    }
}
