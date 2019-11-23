using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class ProvinceB
    {
        #region CRUD
        public static List<Provinces> SelectProvinces()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Provinces.ToList();
            }
        }
        #endregion
    }
}
