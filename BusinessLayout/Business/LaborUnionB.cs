using Data.Context;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLayout.Business
{
    public class LaborUnionB
    {
        #region CRUD
        public static List<Labor_unions> SelectLaborUnions()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Labor_unions.ToList();
            }
        }

        public static void InsertLaborUnion(Labor_unions laborUnion)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Labor_unions.Add(laborUnion);
                ent.SaveChanges();
            }
        }

        public static void UpdateLaborUnion(Labor_unions laborUnion)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Labor_unions.Attach(laborUnion);
                ent.Entry(laborUnion).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
            }
        }

        public static int DeleteLaborUnion(int laborUnionId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                int result = 0;
                try
                {
                    ent.Labor_unions.Remove(ent.Labor_unions.Find(laborUnionId));
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
