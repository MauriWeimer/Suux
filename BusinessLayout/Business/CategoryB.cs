using Data.Context;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLayout.Business
{
    public class CategoryB
    {
        #region CRUD
        public static List<Categorys> SelectCategorys()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Categorys.ToList();
            }
        }

        public static void InsertCategory(Categorys category)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Categorys.Add(category);
                ent.SaveChanges();
            }
        }

        public static void UpdateCategory(Categorys category)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Categorys.Attach(category);
                ent.Entry(category).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
            }
        }

        public static int DeleteCategory(int categoryId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                int result = 0;
                try
                {
                    ent.Categorys.Remove(ent.Categorys.Find(categoryId));
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
