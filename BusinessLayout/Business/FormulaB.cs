using Data.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayout.Business
{
    public class FormulaB
    {
        #region CRUD
        public static List<Formulas> SelectFormulas()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Formulas.ToList();
            }
        }

        public static void InsertFormula(Formulas formula)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Formulas.Add(formula);
                ent.SaveChanges();
            }
        }
        #endregion
    }
}
