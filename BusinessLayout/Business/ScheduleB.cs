using Data.Context;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace BusinessLayout.Business
{
    public class ScheduleB
    {
        #region CRUD
        public static List<Schedules> SelectSchedule(int calendarId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Schedules
                    .Where(s => s.calendar_id == calendarId)
                    .Include(s => s.Turns)
                    .Include(s => s.Fixed_schedules)
                    .ToList();
            }
        }

        public static void UpdateSchedules(ObservableCollection<Calendars> calendars)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                foreach (Calendars c in calendars)
                {                    
                    List<Schedules> oldSchedules = ent.Schedules.Where(s => s.calendar_id == c.calendar_id).ToList();

                    List<Schedules> updateSchedules = oldSchedules
                        .Where(s => c.Schedules.Select(sx => sx.day).Contains(s.day) &&
                        !c.Schedules.Any(x => x.calendar_id == s.calendar_id && x.day == s.day && x.turn_id == s.turn_id && x.fixed_schedule_id == s.fixed_schedule_id && x.nwork_day == s.nwork_day)).ToList();

                    List<Schedules> addedSchedules = new List<Schedules>();
                    var a = c.Schedules.Where(s => s.day != 0 && !oldSchedules.Select(sx => sx.day).ToList().Contains(s.day)).ToList();
                    c.Schedules.Where(s => s.day != 0 && !oldSchedules.Select(sx => sx.day).ToList().Contains(s.day)).ToList()
                        .ForEach(s => addedSchedules.Add(new Schedules() { calendar_id = s.calendar_id, day = s.day, turn_id = s.Turns?.turn_id, fixed_schedule_id = s.Fixed_schedules?.fixed_schedule_id, nwork_day = s.nwork_day }));

                    List<Schedules> deletedSchedules = oldSchedules.Where(s => !c.Schedules.Select(sx => sx.day).Contains(s.day)).ToList();

                    foreach (Schedules schedule in updateSchedules)
                    {
                        Schedules newS = c.Schedules.Single(s => s.calendar_id == schedule.calendar_id && s.day == schedule.day);

                        schedule.turn_id = newS.turn_id;
                        schedule.fixed_schedule_id = newS.fixed_schedule_id;
                        schedule.nwork_day = newS.nwork_day;
                    }
                    deletedSchedules.ForEach(s => ent.Schedules.Remove(s));
                    addedSchedules.ForEach(s => ent.Schedules.Add(s));
                }
                ent.SaveChanges();
            }
        }
        #endregion
    }
}
