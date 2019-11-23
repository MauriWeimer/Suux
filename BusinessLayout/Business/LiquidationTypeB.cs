using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class LiquidationTypeB
    {

        #region CRUD
        public static List<Liquidation_types> SelectLiquidationTypes()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Liquidation_types.ToList();
            }
        }
        #endregion
    }
}
