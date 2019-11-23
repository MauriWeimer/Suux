using Data.Context;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLayout.Business
{
    public class BankB
    {
        #region CRUD
        public static List<Banks> SelectBanks()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Banks.ToList();
            }
        }

        public static void InsertBank(Banks bank)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Banks.Add(bank);
                ent.SaveChanges();
            }
        }

        public static void UpdateBank(Banks bank)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Banks.Attach(bank);
                ent.Entry(bank).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
            }            
        }

        public static int DeleteBank(int bankId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                int result = 0;
                try
                {
                    ent.Banks.Remove(ent.Banks.Find(bankId));
                    ent.SaveChanges();
                }
                catch(DbUpdateException ex)
                {
                    if (ex.GetBaseException() is SqlException sqlException) result = sqlException.Number;
                }
                return result;
            }
        }
        #endregion
    }
}
