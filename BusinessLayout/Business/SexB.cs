using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class SexB
    {
        #region CRUD
        public static List<Sexs> SelectSexs()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Sexs.ToList();
            }
        }
        #endregion
    }
}
