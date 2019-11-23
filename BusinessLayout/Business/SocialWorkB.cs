using Data.Context;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLayout.Business
{
    public class SocialWorkB
    {
        #region CRUD
        public static List<Social_works> SelectSocialWorks()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Social_works.ToList();
            }
        }

        public static void InsertSocialWork(Social_works socialWork)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Social_works.Add(socialWork);
                ent.SaveChanges();
            }
        }

        public static void UpdateSocialWork(Social_works socialWork)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Social_works.Attach(socialWork);
                ent.Entry(socialWork).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
            }
        }

        public static int DeleteSocialWork(int socialWorkId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                int result = 0;
                try
                {
                    ent.Social_works.Remove(ent.Social_works.Find(socialWorkId));
                    ent.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.GetBaseException() is SqlException sqlException) result = sqlException.Number;
                }
                return result;
            }
        }
        #endregion
    }
}
