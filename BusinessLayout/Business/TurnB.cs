using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class TurnB
    {
        #region CRUD
        public static List<Turns> SelectTurns()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Turns.ToList();
            }
        }

        public static void InsertTurn(Turns turn)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Turns.Add(turn);
                ent.SaveChanges();
            }
        }

        public static void UpdateTurn(Turns turn)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Turns.Attach(turn);
                ent.Entry(turn).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
            }
        }

        public static void DeleteTurn(int turnId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Turns.Remove(ent.Turns.Find(turnId));
                ent.SaveChanges();
            }
        }
        #endregion
    }
}
