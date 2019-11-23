using Data.Context;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLayout.Business
{
    public class ARTB
    {
        #region CRUD
        public static List<ART> SelectART()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.ART.ToList();
            }
        }

        public static void InsertART(ART art)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.ART.Add(art);
                ent.SaveChanges();
            }
        }

        public static void UpdateART(ART art)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.ART.Attach(art);
                ent.Entry(art).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
            }
        }

        public static int DeleteART(int artId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                int result = 0;
                try
                {
                    ent.ART.Remove(ent.ART.Find(artId));
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
