using Data.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace BusinessLayout.Business
{
    public class CalendarB
    {
        #region CRUD
        public static List<Calendars> SelectCalendars(DateTime month)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Calendars
                    .Where(c => c.month == month)
                    .Include(c => c.Employees)
                    .Include(c => c.Employees.Fixed_schedules)
                    .ToList();
            }
        }       

        public static void InsertCalendars(ObservableCollection<Calendars> calendars)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                foreach (Calendars c in calendars)
                {
                    Calendars ca = new Calendars()
                    {
                        calendar_id = c.calendar_id,
                        file_n = c.Employees.file_n,
                        month = c.month,
                        Schedules = new ObservableCollection<Schedules>(c.Schedules.Where(s => s.day != 0)
                        .Select(s => new Schedules { calendar_id = c.calendar_id, day = s.day, turn_id = s.Turns?.turn_id, fixed_schedule_id = s.Fixed_schedules?.fixed_schedule_id, nwork_day = s.nwork_day }))
                    };

                    ent.Calendars.Add(ca);
                }
                ent.SaveChanges();
            }
        }

        public static void DeleteCalendars(List<int> calendarsId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                calendarsId.ForEach(cid => ent.Calendars.Remove(ent.Calendars.Single(c => c.calendar_id == cid)));
                ent.SaveChanges();
            }
        }
        #endregion

        #region ADD
        public static int SelectVacationsDays(int fileN, DateTime month)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Calendars
                    .Where(c => c.file_n == fileN && c.month == month)
                    .Select(c => c.Schedules.Where(s => s.nwork_day == "V").Count())
                    .FirstOrDefault();
            }
        }

        public static int GetNextId()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Calendars.Count() > 0 ? ent.Calendars.OrderByDescending(c => c.calendar_id).First().calendar_id : 0;
            }
        }
        #endregion
    }
}
