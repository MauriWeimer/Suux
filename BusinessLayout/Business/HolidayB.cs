using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class HolidayB
    {
        #region CRUD
        public static List<Holidays> SelectHolidays(DateTime month)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Holidays
                    .Where(h => h.month == month)
                    .ToList();
            }
        }

        public static void InsertHoliday(Holidays holiday)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Holidays.Add(holiday);
                ent.SaveChanges();
            }
        }

        public static void DeleteHoliday(DateTime month, int day)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Holidays.Remove(ent.Holidays.Where(h => h.month == month && h.day == day).First());
                ent.SaveChanges();
            }
        }
        #endregion
    }
}
