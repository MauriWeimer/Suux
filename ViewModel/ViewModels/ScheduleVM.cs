using BusinessLayout.Business;
using Data.Context;
using Excels.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class ScheduleVM : ObservableObject
    {
        public ScheduleVM()
        {
            Month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString();
        }

        #region ItemsSource
        private List<Employees> allEmployees = EmployeeB.SelectEmployeesIncludeFixedSchedules();
        public List<Turns> Turns => TurnB.SelectTurns();
        public ObservableCollection<Holidays> Holidays { get; set; } = new ObservableCollection<Holidays>();

        public ObservableCollection<Calendars> Calendars { get; set; } = new ObservableCollection<Calendars>();
        #endregion

        #region Properties   
        public int? HolidayDay { get; set; }

        private int? _day;
        public int? Day
        {
            get => _day;
            set => Set(ref _day, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private int? _turnId;
        public int? TurnId
        {
            get => _turnId;
            set
            {
                Set(ref _turnId, value);
                if (value != null) SetTurn();
            }
        }

        public Dictionary<object, List<int>> SelectedDays { get; set; }

        private DateTime _monthHoliday;
        private DateTime _month;
        public string Month
        {
            get => _month.ToString("MMMM/yyyy").ToUpper();
            set
            {
                Set(ref _month, DateTime.Parse(value));
                _monthHoliday = new DateTime(1001, _month.Month, 1);
                LoadCalendarsAndHolidays();
            }
        }

        private Dictionary<int, List<Schedules>> originalSchedules = new Dictionary<int, List<Schedules>>();

        public bool ExportHF { get; set; }
        #endregion

        #region Methods
        private async void LoadCalendarsAndHolidays()
        {
            TextLoadingDialog = "Cargando";
            if (!_loadingDialog) LoadingDialog = true;          

            List<Holidays> holidays = new List<Holidays>();
            List<Calendars> calendars = new List<Calendars>();

            await Task.Run(() =>
            {
                HolidayB.SelectHolidays(_monthHoliday).ForEach(h => holidays.Add(h));
                CalendarB.SelectCalendars(_month).ForEach(c => calendars.Add(c));
            });
            
            Calendars.Clear();                        
            calendars.ForEach(c => Calendars.Add(c));
            Holidays.Clear();
            holidays.ForEach(h => Holidays.Add(h));

            await Task.Run(() => ChargeSchedules());

            LoadingDialog = false;
        }

        private void ChargeSchedules()
        {
            foreach (Calendars c in Calendars)
            {
                List<Schedules> schedules = ScheduleB.SelectSchedule(c.calendar_id);
                for (int x = 1; x <= DateTime.DaysInMonth(_month.Year, _month.Month); x++)
                {
                    if (schedules.Any(s => s.day == x))
                    {
                        c.Schedules.Add(schedules.Single(s => s.day == x));
                    }
                    else
                    {
                        c.Schedules.Add(new Schedules());
                    }
                }
                Holidays.ToList().ForEach(h => c.Schedules[h.day - 1].holiday = true);
            }           
        }

        private async void SetTurn()
        {
            TextLoadingDialog = "Aplicando\nturnos";
            LoadingDialog = true;

            if (SelectedDays?.Count > 0)
            {
                await Task.Run(() =>
                {
                    Calendars.Where(c => c.Employees.Fixed_schedules != null).ToList().ForEach(c => SelectedDays.Remove(c));
                    foreach (KeyValuePair<object, List<int>> days in SelectedDays)
                    {
                        int fileN = ((Calendars)days.Key).Employees.file_n;
                        foreach (int day in days.Value)
                        {
                            Calendars.Single(c => c.Employees.file_n == fileN).Schedules[day] = new Schedules()
                            {
                                calendar_id = Calendars.Single(c => c.Employees.file_n == fileN).calendar_id,
                                day = day + 1,
                                Turns = Turns.Single(t => t.turn_id == _turnId),
                                holiday = Holidays.Any(h => h.month == _monthHoliday && h.day == day + 1)
                            };
                        }
                    }
                });
            }

            Messenger.Default.Send(new NotificationMessage("UnselectDataGrid"));
            TurnId = null;
            LoadingDialog = false;
        }

        private async void CreateSchedules()
        {
            TextLoadingDialog = "Creando";
            LoadingDialog = true;

            List<Calendars> calendars = new List<Calendars>();
            int calendarId = CalendarB.GetNextId();

            await Task.Run(() =>
            {
                foreach (Employees e in allEmployees)
                {
                    calendarId++;
                    Calendars c = new Calendars() { calendar_id = calendarId, Employees = e, month = _month };
                    Fixed_schedules fs = e.Fixed_schedules;

                    for (int x = 1; x <= DateTime.DaysInMonth(_month.Year, _month.Month); x++)
                    {
                        if (e.birthdate.Day == x && e.birthdate.Month == _month.Month)
                        {
                            c.Schedules.Add(new Schedules() { calendar_id = c.calendar_id, day = x, nwork_day = "C" });
                            continue;
                        }

                        if (fs == null)
                        {
                            c.Schedules.Add(new Schedules());
                        }
                        else
                        {
                            c.Schedules.Add(NewFixedSchedule(new DateTime(_month.Year, _month.Month, x), fs, calendarId));
                        }
                    }

                    Holidays.ToList().ForEach(h => c.Schedules[h.day - 1].holiday = true);
                    calendars.Add(c);
                }
            });

            Calendars.Clear();
            calendars.ForEach(c => Calendars.Add(c));

            LoadingDialog = false;
        }

        private Schedules NewFixedSchedule(DateTime dayOfWeek, Fixed_schedules fs, int calendarId)
        {
            Schedules s = new Schedules();

            switch (dayOfWeek.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (fs.morning_lu != " - " || fs.late_lu != " - ")
                    {
                        s.calendar_id = calendarId;
                        s.day = dayOfWeek.Day;
                        s.Fixed_schedules = fs;
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (fs.morning_ma != " - " || fs.late_ma != " - ")
                    {
                        s.calendar_id = calendarId;
                        s.day = dayOfWeek.Day;
                        s.Fixed_schedules = fs;
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (fs.morning_mi != " - " || fs.late_mi != " - ")
                    {
                        s.calendar_id = calendarId;
                        s.day = dayOfWeek.Day;
                        s.Fixed_schedules = fs;
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (fs.morning_ju != " - " || fs.late_ju != " - ")
                    {
                        s.calendar_id = calendarId;
                        s.day = dayOfWeek.Day;
                        s.Fixed_schedules = fs;
                    }
                    break;
                case DayOfWeek.Friday:
                    if (fs.morning_vi != " - " || fs.late_vi != " - ")
                    {
                        s.calendar_id = calendarId;
                        s.day = dayOfWeek.Day;
                        s.Fixed_schedules = fs;
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (fs.morning_sa != " - " || fs.late_sa != " - ")
                    {
                        s.calendar_id = calendarId;
                        s.day = dayOfWeek.Day;
                        s.Fixed_schedules = fs;
                    }
                    break;
                case DayOfWeek.Sunday:
                    if (fs.morning_do != " - " || fs.late_do != " - ")
                    {
                        s.calendar_id = calendarId;
                        s.day = dayOfWeek.Day;
                        s.Fixed_schedules = fs;
                    }
                    break;
            }

            s.holiday = Holidays.Any(h => h.month == _monthHoliday && h.day == dayOfWeek.Day);
            return s;
        }        
        #endregion

        #region Dialog  
        private string _textLoadingDialog;
        public string TextLoadingDialog
        {
            get => _textLoadingDialog;
            set => Set(ref _textLoadingDialog, value);
        }

        private bool _loadingDialog;
        public bool LoadingDialog
        {
            get => _loadingDialog;
            set => Set(ref _loadingDialog, value);
        }

        private bool _confirmDialog;
        public bool ConfirmDialog
        {
            get => _confirmDialog;
            set => Set(ref _confirmDialog, value);
        }

        private bool _exportDialog;
        public bool ExportDialog
        {
            get => _exportDialog;
            set => Set(ref _exportDialog, value);
        }

        private bool _alertDialog;
        public bool AlertDialog
        {
            get => _alertDialog;
            set => Set(ref _alertDialog, value);
        }

        public string ConfirmDialogText => "¿Desea eliminar los horarios del mes?";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return Calendars.Count > 0; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    await Task.Run(() => CalendarB.DeleteCalendars(Calendars.Select(c => c.calendar_id).ToList()));
                    Calendars.Clear();

                    LoadingDialog = false;
                });

        public ICommand OpenExportDialog => new RelayCommand
               (
               () => { ExportDialog = true; },
               () => { return Calendars.Count > 0; }
               );
        #endregion

        #region Commands

        public ICommand NewHoliday => new RelayCommand
           (
           () =>
           {
               Holidays holidays = new Holidays() { month = _monthHoliday, day = (int)_day, description = _description?.ToUpper() };

               HolidayB.InsertHoliday(holidays);
               Holidays.Add(holidays);

               Calendars.ToList().ForEach(c => c.Schedules[(int)_day - 1].holiday = true);

               Day = null;
               Description = null;
           },
           () => { return _day != null && !Holidays.Any(h => h.month == _monthHoliday && h.day == _day) && _day <= DateTime.DaysInMonth(_month.Year, _month.Month); }
           );

        public ICommand DeleteHoliday => new RelayCommand
           (
           () =>
           {
               HolidayB.DeleteHoliday(_monthHoliday, (int)HolidayDay);
               Calendars.ToList().ForEach(c => c.Schedules[(int)HolidayDay - 1].holiday = false);
               Holidays.Remove(Holidays.Single(h => h.month == _monthHoliday && h.day == HolidayDay));
           },
           () => { return HolidayDay != null; }
           );

        public ICommand Clear => new RelayCommand
           (
           async () =>
           {
               TextLoadingDialog = "Limpiando";
               LoadingDialog = true;
               await Task.Run(() =>
               {
                   foreach (KeyValuePair<object, List<int>> days in SelectedDays)
                   {
                       int fileN = ((Calendars)days.Key).Employees.file_n;
                       foreach (int day in days.Value)
                       {
                           Calendars.Single(c => c.Employees.file_n == fileN).Schedules[day] = new Schedules()
                           {
                               holiday = Holidays.Any(h => h.month == _monthHoliday && h.day == day + 1)
                           };
                       }
                   }
               });

               Messenger.Default.Send(new NotificationMessage("UnselectDataGrid"));
               LoadingDialog = false;
           },
           () => { return SelectedDays?.Count > 0; }
           );

        public ICommand FixedSchedule => new RelayCommand
           (
           async () =>
           {
               TextLoadingDialog = "Aplicando\nhorarios";
               LoadingDialog = true;

               await Task.Run(() =>
               {
                   Calendars.Where(c => c.Employees.Fixed_schedules == null).ToList().ForEach(c => SelectedDays.Remove(c));
                   foreach (KeyValuePair<object, List<int>> days in SelectedDays)
                   {
                       int fileN = ((Calendars)days.Key).Employees.file_n;
                       foreach (int day in days.Value)
                       {
                           Schedules s = NewFixedSchedule(new DateTime(_month.Year, _month.Month, day + 1),
                               allEmployees.Single(e => e.file_n == fileN).Fixed_schedules,
                               Calendars.Single(c => c.Employees.file_n == fileN).calendar_id);
                           if (s.Fixed_schedules != null) Calendars.Single(c => c.Employees.file_n == fileN).Schedules[day] = s;
                       }
                   }
               });

               Messenger.Default.Send(new NotificationMessage("UnselectDataGrid"));
               LoadingDialog = false;
           },
           () => { return SelectedDays?.Count > 0; }
           );

        public ICommand Vacations => new RelayCommand
          (
          async () =>
          {
              TextLoadingDialog = "Aplicando\nvacaciones";
              LoadingDialog = true;

              await Task.Run(() =>
              {
                  foreach (KeyValuePair<object, List<int>> days in SelectedDays)
                  {
                      int fileN = ((Calendars)days.Key).Employees.file_n;
                      foreach (int day in days.Value)
                      {
                          Calendars.Single(c => c.Employees.file_n == fileN).Schedules[day] = new Schedules()
                          {
                              calendar_id = Calendars.Single(c => c.Employees.file_n == fileN).calendar_id,
                              day = day + 1,
                              nwork_day = "V",
                              holiday = Holidays.Any(h => h.month == _monthHoliday && h.day == day + 1)
                          };
                      }
                  }
              });

              Messenger.Default.Send(new NotificationMessage("UnselectDataGrid"));
              LoadingDialog = false;
          },
          () => { return SelectedDays?.Count > 0; }
          );

        public ICommand Previous => new RelayCommand
           (
           () =>
           {
               Month = _month.AddMonths(-1).ToString();
           }
           );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                Month = _month.AddMonths(1).ToString();
            }
            );

        public ICommand New => new RelayCommand
            (
            () =>
            {
                TextLoadingDialog = "Agregando";
                CreateSchedules();
            },
            () => { return Calendars.Count == 0 && allEmployees.Count > 0; }
            );

        public ICommand Update => new RelayCommand
            (
            async () =>
            {
                TextLoadingDialog = "Actualizando";
                await Task.Run(() => Calendars.ToList().ForEach(c => originalSchedules.Add(c.calendar_id, c.Schedules.ToList())));
            },
            () => { return Calendars.Count > 0; }
            );

        public ICommand Apply => new RelayCommand
           (
           async () =>
           {
               LoadingDialog = true;

               if (originalSchedules.Count == 0)
               {
                   await Task.Run(() => CalendarB.InsertCalendars(Calendars));
               }
               else
               {
                   await Task.Run(() => ScheduleB.UpdateSchedules(Calendars));
                   originalSchedules.Clear();
               }

               LoadingDialog = false;
           }
           );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                TextLoadingDialog = "Cancelando";
                LoadingDialog = true;

                if (originalSchedules.Count > 0)
                {
                    foreach (KeyValuePair<int, List<Schedules>> c in originalSchedules)
                    {
                        for (int x = 0; x < DateTime.DaysInMonth(_month.Year, _month.Month); x++)
                        {
                            if (!c.Value[x].Equals(Calendars.Single(ca => ca.calendar_id == c.Key).Schedules[x]))
                            {
                                Calendars.Single(ca => ca.calendar_id == c.Key).Schedules[x] = c.Value[x];
                            }
                        }
                    }
                    originalSchedules.Clear();
                }
                else
                {
                    Calendars.Clear();
                }

                LoadingDialog = false;
            }
            );

        public ICommand Print => new RelayCommand
           (
           async () =>
           {
               ExportDialog = false;
               TextLoadingDialog = "Imprimiendo";
               LoadingDialog = true;

               await ExportExcel.ExportSchedules(_month,
                   ExportHF ? Calendars.ToList() : Calendars.Where(c => c.Employees.fixed_schedule_id == null).ToList(),
                   Calendars.ToList(), true, Holidays.Select(h => h.day).ToList());

               LoadingDialog = false;
               AlertDialog = true;
           }
           );

        public ICommand Export => new RelayCommand
           (
           async () =>
           {
               ExportDialog = false;
               TextLoadingDialog = "Exportando";
               LoadingDialog = true;

               await ExportExcel.ExportSchedules(_month,
                   ExportHF ? Calendars.ToList() : Calendars.Where(c => c.Employees.fixed_schedule_id == null).ToList(),
                   Calendars.ToList(), false, Holidays.Select(h => h.day).ToList());

               LoadingDialog = false;
               AlertDialog = true;
           }
           );
        #endregion
    }
}
