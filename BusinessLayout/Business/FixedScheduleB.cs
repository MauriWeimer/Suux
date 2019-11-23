using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class FixedScheduleB
    {
        #region CRUD
        public static List<Fixed_schedules> SelectFixedSchedules()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Fixed_schedules.ToList();
            }
        }

        public static void InsertFixedSchedule(Fixed_schedules fixedSchedule)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Fixed_schedules.Add(fixedSchedule);
                ent.SaveChanges();
            }
        }
        #endregion
    }
}
