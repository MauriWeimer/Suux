using BusinessLayout.Business;
using Data.Context;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class EmployeeVM : ObservableObject, IDataErrorInfo
    {
        #region ItemsSource
        public List<Sexs> Sexs => SexB.SelectSexs();
        public List<Provinces> Provinces => ProvinceB.SelectProvinces();
        public List<Categorys> Categorys => CategoryB.SelectCategorys();
        public List<Social_works> SocialWorks => SocialWorkB.SelectSocialWorks();
        public List<Labor_unions> LaborUnions => LaborUnionB.SelectLaborUnions();
        public List<ART> ART => ARTB.SelectART();

        public ObservableCollection<Fixed_schedules> FixedSchedules { get; set; } = new ObservableCollection<Fixed_schedules>(FixedScheduleB.SelectFixedSchedules());

        private List<Employees> allEmployees = EmployeeB.SelectEmployees();
        public ObservableCollection<Employees> Employees { get; set; } = new ObservableCollection<Employees>();
        #endregion

        #region Properties   
        private int? _fileN;
        public int? FileN
        {
            get => _fileN;
            set
            {
                Set(ref _fileN, value);
                if (!searching)
                if (value != null && Employees.Any(e => e.file_n == value))
                {
                    LoadEmployee();
                }
                else
                {
                    ClearProperties();
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value);
        }

        private DateTime? _birthDate;
        public DateTime? BirthDate
        {
            get => _birthDate;
            set => Set(ref _birthDate, value);
        }

        private int? _sexId;
        public int? SexId
        {
            get => _sexId;
            set => Set(ref _sexId, value);
        }

        private long? _documentN;
        public long? DocumentN
        {
            get => _documentN;
            set => Set(ref _documentN, value);
        }

        private long? _cuilN;
        public long? CuilN
        {
            get => _cuilN;
            set => Set(ref _cuilN, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private string _street;
        public string Street
        {
            get => _street;
            set => Set(ref _street, value);
        }

        private int? _streetN;
        public int? StreetN
        {
            get => _streetN;
            set => Set(ref _streetN, value);
        }

        private int? _floor;
        public int? Floor
        {
            get => _floor;
            set => Set(ref _floor, value);
        }

        private string _departament;
        public string Departament
        {
            get => _departament;
            set => Set(ref _departament, value);
        }

        private int? _provinceId;
        public int? ProvinceId
        {
            get => _provinceId;
            set => Set(ref _provinceId, value);
        }

        private string _city;
        public string City
        {
            get => _city;
            set => Set(ref _city, value);
        }

        private int? _postalCode;
        public int? PostalCode
        {
            get => _postalCode;
            set => Set(ref _postalCode, value);
        }

        private long? _phoneN;
        public long? PhoneN
        {
            get => _phoneN;
            set => Set(ref _phoneN, value);
        }

        private string _profCalification;
        public string ProfCalification
        {
            get => _profCalification;
            set => Set(ref _profCalification, value);
        }

        private string _basicSalary;
        public string BasicSalary
        {
            get => _basicSalary;
            set => Set(ref _basicSalary, value);
        }

        private bool _manually;
        public bool Manually
        {
            get => _manually;
            set => Set(ref _manually, value);
        }

        private DateTime? _entryDate;
        public DateTime? EntryDate
        {
            get => _entryDate;
            set => Set(ref _entryDate, value);
        }

        private int _categoryId;
        public int CategoryId
        {
            get => _categoryId;
            set => Set(ref _categoryId, value);
        }

        private int _socialWorkId;
        public int SocialWorkId
        {
            get => _socialWorkId;
            set => Set(ref _socialWorkId, value);
        }

        private int _laborUnionId;
        public int LaborUnionId
        {
            get => _laborUnionId;
            set => Set(ref _laborUnionId, value);
        }

        private int _artId;
        public int ARTId
        {
            get => _artId;
            set => Set(ref _artId, value);
        }

        private DateTime? _lowDate;
        public DateTime? LowDate
        {
            get => _lowDate;
            set => Set(ref _lowDate, value);
        }

        private int _state;
        public int State
        {
            get => _state;
            set => Set(ref _state, value);
        }

        private int _scheduleIndex;
        public int ScheduleIndex
        {
            get => _scheduleIndex;
            set => Set(ref _scheduleIndex, value);
        }

        private int? _fixedScheduleId;
        public int? FixedScheduleId
        {
            get => _fixedScheduleId;
            set
            {
                Set(ref _fixedScheduleId, value);
                if (value != null)
                {
                    LoadFixedSchedule();
                }
                else
                {
                    ClearFixedSchedulesProperties();
                }
            }
        }

        private TimeSpan? _lu_m_d;
        public TimeSpan? Lu_m_d
        {
            get => _lu_m_d;
            set => Set(ref _lu_m_d, value);
        }

        private TimeSpan? _lu_m_h;
        public TimeSpan? Lu_m_h
        {
            get => _lu_m_h;
            set => Set(ref _lu_m_h, value);
        }

        private TimeSpan? _lu_l_d;
        public TimeSpan? Lu_l_d
        {
            get => _lu_l_d;
            set => Set(ref _lu_l_d, value);
        }

        private TimeSpan? _lu_l_h;
        public TimeSpan? Lu_l_h
        {
            get => _lu_l_h;
            set => Set(ref _lu_l_h, value);
        }

        private TimeSpan? _ma_m_d;
        public TimeSpan? Ma_m_d
        {
            get => _ma_m_d;
            set => Set(ref _ma_m_d, value);
        }

        private TimeSpan? _ma_m_h;
        public TimeSpan? Ma_m_h
        {
            get => _ma_m_h;
            set => Set(ref _ma_m_h, value);
        }

        private TimeSpan? _ma_l_d;
        public TimeSpan? Ma_l_d
        {
            get => _ma_l_d;
            set => Set(ref _ma_l_d, value);
        }

        private TimeSpan? _ma_l_h;
        public TimeSpan? Ma_l_h
        {
            get => _ma_l_h;
            set => Set(ref _ma_l_h, value);
        }

        private TimeSpan? _mi_m_d;
        public TimeSpan? Mi_m_d
        {
            get => _mi_m_d;
            set => Set(ref _mi_m_d, value);
        }

        private TimeSpan? _mi_m_h;
        public TimeSpan? Mi_m_h
        {
            get => _mi_m_h;
            set => Set(ref _mi_m_h, value);
        }

        private TimeSpan? _mi_l_d;
        public TimeSpan? Mi_l_d
        {
            get => _mi_l_d;
            set => Set(ref _mi_l_d, value);
        }

        private TimeSpan? _mi_l_h;
        public TimeSpan? Mi_l_h
        {
            get => _mi_l_h;
            set => Set(ref _mi_l_h, value);
        }

        private TimeSpan? _ju_m_d;
        public TimeSpan? Ju_m_d
        {
            get => _ju_m_d;
            set => Set(ref _ju_m_d, value);
        }

        private TimeSpan? _ju_m_h;
        public TimeSpan? Ju_m_h
        {
            get => _ju_m_h;
            set => Set(ref _ju_m_h, value);
        }

        private TimeSpan? _ju_l_d;
        public TimeSpan? Ju_l_d
        {
            get => _ju_l_d;
            set => Set(ref _ju_l_d, value);
        }

        private TimeSpan? _ju_l_h;
        public TimeSpan? Ju_l_h
        {
            get => _ju_l_h;
            set => Set(ref _ju_l_h, value);
        }

        private TimeSpan? _vi_m_d;
        public TimeSpan? Vi_m_d
        {
            get => _vi_m_d;
            set => Set(ref _vi_m_d, value);
        }

        private TimeSpan? _vi_m_h;
        public TimeSpan? Vi_m_h
        {
            get => _vi_m_h;
            set => Set(ref _vi_m_h, value);
        }

        private TimeSpan? _vi_l_d;
        public TimeSpan? Vi_l_d
        {
            get => _vi_l_d;
            set => Set(ref _vi_l_d, value);
        }

        private TimeSpan? _vi_l_h;
        public TimeSpan? Vi_l_h
        {
            get => _vi_l_h;
            set => Set(ref _vi_l_h, value);
        }

        private TimeSpan? _sa_m_d;
        public TimeSpan? Sa_m_d
        {
            get => _sa_m_d;
            set => Set(ref _sa_m_d, value);
        }

        private TimeSpan? _sa_m_h;
        public TimeSpan? Sa_m_h
        {
            get => _sa_m_h;
            set => Set(ref _sa_m_h, value);
        }

        private TimeSpan? _sa_l_d;
        public TimeSpan? Sa_l_d
        {
            get => _sa_l_d;
            set => Set(ref _sa_l_d, value);
        }

        private TimeSpan? _sa_l_h;
        public TimeSpan? Sa_l_h
        {
            get => _sa_l_h;
            set => Set(ref _sa_l_h, value);
        }

        private TimeSpan? _do_m_d;
        public TimeSpan? Do_m_d
        {
            get => _do_m_d;
            set => Set(ref _do_m_d, value);
        }

        private TimeSpan? _do_m_h;
        public TimeSpan? Do_m_h
        {
            get => _do_m_h;
            set => Set(ref _do_m_h, value);
        }

        private TimeSpan? _do_l_d;
        public TimeSpan? Do_l_d
        {
            get => _do_l_d;
            set => Set(ref _do_l_d, value);
        }

        private TimeSpan? _do_l_h;
        public TimeSpan? Do_l_h
        {
            get => _do_l_h;
            set => Set(ref _do_l_h, value);
        }
        #endregion

        #region Errors
        public string Error => null;
        public string this[string property]
        {
            get => GetValidationError(property, canCheckErrors);
        }

        private static readonly string[] validatedProperties = { "Name", "LastName", "DocumentN", "BirthDate", "CuilN", "Email",
            "StreetN", "PostalCode", "PhoneN", "BasicSalary", "EntryDate" };
        public bool IsValid
        {
            get
            {
                if (_categoryId == 0 || _socialWorkId == 0 || _laborUnionId == 0 || _artId == 0) return false;

                foreach (string property in validatedProperties)
                    if (!string.IsNullOrEmpty(GetValidationError(property, true))) return false;
                return true;
            }
        }

        private bool canCheckErrors;
        private string GetValidationError(string property, bool canCheck)
        {
            if (canCheck)
            {
                string result = null;
                switch (property)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(_name))
                        {
                            result = "El nombre del empleado es requerido";
                        }
                        break;
                    case "LastName":
                        if (string.IsNullOrWhiteSpace(_lastName))
                        {
                            result = "El apellido del empleado es requerido";
                        }
                        break;
                    case "DocumentN":
                        if (_documentN == null)
                        {
                            result = "El n° de documento del empleado es requerido";
                        }
                        else
                        {
                            if (_documentN < 9999999)
                            {
                                result = "El n° de documento debe contener 8 dígitos";
                            }
                            else if (!_cuilN.ToString().Contains(_documentN.ToString()) && _cuilN != null)
                            {
                                result = "El n° de documento no coincide con el n° de cuil";
                            }
                        }
                        break;
                    case "CuilN":
                        if (_cuilN == null)
                        {
                            result = "El n° de cuil del empleado es requerido";
                        }
                        else if (_cuilN < 9999999999)
                        {
                            result = "El n° de cuil debe contener 11 dígitos";
                        }
                        else if (!_cuilN.ToString().Substring(2, 8).Contains(_documentN.ToString()))
                        {
                            result = "El n° de cuil no coincide con el n° de documento";
                        }
                        break;
                    case "BirthDate":
                        if (_birthDate == null)
                        {
                            result = "La fecha de nacimiento del empleado es requerida";
                        }
                        else if (_birthDate > new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            result = "El empleado es menor de edad";
                        }
                        break;
                    case "StreetN":
                        if (_streetN != null && _streetN < 999)
                        {
                            result = "El n° de calle debe contener 4 dígitos";
                        }
                        break;
                    case "PostalCode":
                        if (_postalCode != null && _postalCode < 999)
                        {
                            result = "El código postal debe contener 4 dígitos";
                        }
                        break;
                    case "PhoneN":
                        if (_phoneN != null && _phoneN < 9999999)
                        {
                            result = "El n° teléfonico debe contener más de 7 dígitos";
                        }
                        break;
                    case "Email":
                        if (!string.IsNullOrWhiteSpace(_email) && !_email.Contains('@'))
                        {
                            result = "Email incorrecto";
                        }
                        break;
                    case "EntryDate":
                        if (_entryDate == null)
                        {
                            result = "La fecha de ingreso del empleado es requerida";
                        }
                        break;
                    case "BasicSalary":
                        if (string.IsNullOrWhiteSpace(_basicSalary) && _manually)
                        {
                            result = "Ingrese un monto";
                        }
                        break;
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Search
        private bool searching;
        public int Filter { get; set; }

        private string _search;
        public string Search
        {
            set
            {
                Set(ref _search, value);
                SearchEmployee();
            }
        }

        private void SearchEmployee()
        {
            searching = true;
            if (string.IsNullOrWhiteSpace(_search))
            {
                allEmployees
                    .Where(e => !Employees.Any(ex => ex.file_n == e.file_n))
                    .ToList()
                    .ForEach(e => Employees.Add(e));
            }
            else
            {
                SearchEmployeeByFilter();
            }

            searching = false;
            if (_fileN == null) FileN = Employees.FirstOrDefault()?.file_n;
        }
        private void SearchEmployeeByFilter()
        {
            switch (Filter)
            {
                case 0:
                    allEmployees
                        .Where(e => Employees.Any(ex => ex.file_n == e.file_n) && e.file_n != int.Parse(_search))
                        .ToList()
                        .ForEach(e => Employees.Remove(e));
                    allEmployees
                        .Where(e => !Employees.Any(ex => ex.file_n == e.file_n) && e.file_n == int.Parse(_search))
                        .ToList()
                        .ForEach(e => Employees.Add(e));
                    break;
                case 1:
                    allEmployees
                        .Where(e => Employees.Any(ex => ex.fullname == e.fullname) && !e.fullname.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(e => Employees.Remove(e));
                    allEmployees
                        .Where(e => !Employees.Any(ex => ex.fullname == e.fullname) && e.fullname.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(e => Employees.Add(e));
                    break;
            }
        }
        #endregion

        #region Methods
        private void ClearProperties()
        {
            Name = null;
            LastName = null;
            BirthDate = null;
            SexId = null;
            DocumentN = null;
            CuilN = null;
            Email = null;
            Street = null;
            StreetN = null;
            Floor = null;
            Departament = null;
            ProvinceId = null;
            City = null;
            PostalCode = null;
            PhoneN = null;
            ProfCalification = null;
            Manually = false;
            EntryDate = null;
            CategoryId = 0;
            SocialWorkId = 0;
            LaborUnionId = 0;
            ARTId = 0;
            LowDate = null;
            State = 1;

            ScheduleIndex = 0;
            FixedScheduleId = null;
        }

        private void ClearFixedSchedulesProperties()
        {
            Lu_m_d = null;
            Lu_m_h = null;
            Lu_l_d = null;
            Lu_l_h = null;

            Ma_m_d = null;
            Ma_m_h = null;
            Ma_l_d = null;
            Ma_l_h = null;

            Mi_m_d = null;
            Mi_m_h = null;
            Mi_l_d = null;
            Mi_l_h = null;

            Ju_m_d = null;
            Ju_m_h = null;
            Ju_l_d = null;
            Ju_l_h = null;

            Vi_m_d = null;
            Vi_m_h = null;
            Vi_l_d = null;
            Vi_l_h = null;

            Sa_m_d = null;
            Sa_m_h = null;
            Sa_l_d = null;
            Sa_l_h = null;

            Do_m_d = null;
            Do_m_h = null;
            Do_l_d = null;
            Do_l_h = null;
        }

        private Employees GetEmployee(int? fixedSecheduleId)
        {
            return new Employees
            {
                file_n = _fileN ?? 0,
                name = _name.ToUpper(),
                last_name = _lastName.ToUpper(),
                birthdate = (DateTime)_birthDate,
                sex_id = _sexId,
                document_n = (long)_documentN,
                cuil_n = (long)_cuilN,
                email = _email,
                street = _street?.ToUpper(),
                street_n = _streetN,
                floor = _floor,
                departament = _departament?.ToUpper(),
                province_id = _provinceId,
                city = _city?.ToUpper(),
                postal_code = _postalCode,
                phone_n = _phoneN,
                prof_calification = _profCalification?.ToUpper(),
                basic_salary = _manually == true ? decimal.Parse(_basicSalary?.Replace(".", ",")) : (decimal?)null,
                entry_date = (DateTime)_entryDate,
                category_id = _categoryId,
                social_work_id = _socialWorkId,
                labor_union_id = _laborUnionId,
                art_id = _artId,
                low_date = _lowDate,
                state = _state == 1 ? true : false,

                fixed_schedule_id = fixedSecheduleId
            };
        }

        private Fixed_schedules GetFixedSechedule()
        {
            return new Fixed_schedules
            {
                lu_m_d = _lu_m_d,
                lu_m_h = _lu_m_h,
                lu_l_d = _lu_l_d,
                lu_l_h = _lu_l_h,

                ma_m_d = _ma_m_d,
                ma_m_h = _ma_m_h,
                ma_l_d = _ma_l_d,
                ma_l_h = _ma_l_h,

                mi_m_d = _mi_m_d,
                mi_m_h = _mi_m_h,
                mi_l_d = _mi_l_d,
                mi_l_h = _mi_l_h,

                ju_m_d = _ju_m_d,
                ju_m_h = _ju_m_h,
                ju_l_d = _ju_l_d,
                ju_l_h = _ju_l_h,

                vi_m_d = _vi_m_d,
                vi_m_h = _vi_m_h,
                vi_l_d = _vi_l_d,
                vi_l_h = _vi_l_h,

                sa_m_d = _sa_m_d,
                sa_m_h = _sa_m_h,
                sa_l_d = _sa_l_d,
                sa_l_h = _sa_l_h,

                do_m_d = _do_m_d,
                do_m_h = _do_m_h,
                do_l_d = _do_l_d,
                do_l_h = _do_l_h
            };
        }

        private async Task<Employees> GetEmployeeWithFixedSchedule()
        {
            if (ScheduleIndex == 1)
            {
                Fixed_schedules fixedSchedule = GetFixedSechedule();
                if (FixedSchedules.Any(fs =>
                fs.lu_m_d == fixedSchedule.lu_m_d && fs.lu_m_h == fixedSchedule.lu_m_h
                && fs.lu_l_d == fixedSchedule.lu_l_d && fs.lu_l_h == fixedSchedule.lu_l_h
                && fs.ma_m_d == fixedSchedule.ma_m_d && fs.ma_m_h == fixedSchedule.ma_m_h
                && fs.ma_l_d == fixedSchedule.ma_l_d && fs.ma_l_h == fixedSchedule.ma_l_h
                && fs.mi_m_d == fixedSchedule.mi_m_d && fs.mi_m_h == fixedSchedule.mi_m_h
                && fs.mi_l_d == fixedSchedule.mi_l_d && fs.mi_l_h == fixedSchedule.mi_l_h
                && fs.ju_m_d == fixedSchedule.ju_m_d && fs.ju_m_h == fixedSchedule.ju_m_h
                && fs.ju_l_d == fixedSchedule.ju_l_d && fs.ju_l_h == fixedSchedule.ju_l_h
                && fs.vi_m_d == fixedSchedule.vi_m_d && fs.vi_m_h == fixedSchedule.vi_m_h
                && fs.vi_l_d == fixedSchedule.vi_l_d && fs.vi_l_h == fixedSchedule.vi_l_h
                && fs.sa_m_d == fixedSchedule.sa_m_d && fs.sa_m_h == fixedSchedule.sa_m_h
                && fs.sa_l_d == fixedSchedule.sa_l_d && fs.sa_l_h == fixedSchedule.sa_l_h
                && fs.do_m_d == fixedSchedule.do_m_d && fs.do_m_h == fixedSchedule.do_m_h
                && fs.do_l_d == fixedSchedule.do_l_d && fs.do_l_h == fixedSchedule.do_l_h))
                {
                    fixedSchedule.fixed_schedule_id = FixedSchedules.Single(fs =>
                    fs.lu_m_d == fixedSchedule.lu_m_d && fs.lu_m_h == fixedSchedule.lu_m_h
                    && fs.lu_l_d == fixedSchedule.lu_l_d && fs.lu_l_h == fixedSchedule.lu_l_h
                    && fs.ma_m_d == fixedSchedule.ma_m_d && fs.ma_m_h == fixedSchedule.ma_m_h
                    && fs.ma_l_d == fixedSchedule.ma_l_d && fs.ma_l_h == fixedSchedule.ma_l_h
                    && fs.mi_m_d == fixedSchedule.mi_m_d && fs.mi_m_h == fixedSchedule.mi_m_h
                    && fs.mi_l_d == fixedSchedule.mi_l_d && fs.mi_l_h == fixedSchedule.mi_l_h
                    && fs.ju_m_d == fixedSchedule.ju_m_d && fs.ju_m_h == fixedSchedule.ju_m_h
                    && fs.ju_l_d == fixedSchedule.ju_l_d && fs.ju_l_h == fixedSchedule.ju_l_h
                    && fs.vi_m_d == fixedSchedule.vi_m_d && fs.vi_m_h == fixedSchedule.vi_m_h
                    && fs.vi_l_d == fixedSchedule.vi_l_d && fs.vi_l_h == fixedSchedule.vi_l_h
                    && fs.sa_m_d == fixedSchedule.sa_m_d && fs.sa_m_h == fixedSchedule.sa_m_h
                    && fs.sa_l_d == fixedSchedule.sa_l_d && fs.sa_l_h == fixedSchedule.sa_l_h
                    && fs.do_m_d == fixedSchedule.do_m_d && fs.do_m_h == fixedSchedule.do_m_h
                    && fs.do_l_d == fixedSchedule.do_l_d && fs.do_l_h == fixedSchedule.do_l_h).fixed_schedule_id;
                }
                else
                {
                    await Task.Run(() => FixedScheduleB.InsertFixedSchedule(fixedSchedule));
                    FixedSchedules.Add(fixedSchedule);
                }

                return GetEmployee(fixedSchedule.fixed_schedule_id);
            }
            else
            {
                return GetEmployee(null);
            }
        }

        private void MoveSelection(int index, int n)
        {
            FileN = Employees.OrderBy(e => e.file_n).ToList()[(index + n) == Employees.Count ? 0 : (index + n) == -1 ? (Employees.Count - 1) : index + n].file_n;
        }

        private void LoadEmployee()
        {
            Employees employee = Employees.Single(e => e.file_n == _fileN);

            Name = employee.name;
            LastName = employee.last_name;
            BirthDate = employee.birthdate;
            SexId = employee.sex_id;
            DocumentN = employee.document_n;
            CuilN = employee.cuil_n;
            Email = employee.email;
            Street = employee.street;
            StreetN = employee.street_n;
            Floor = employee.floor;
            Departament = employee.departament;
            ProvinceId = employee.province_id;
            City = employee.city;
            PostalCode = employee.postal_code;
            PhoneN = employee.phone_n;
            ProfCalification = employee.prof_calification;
            BasicSalary = employee.basic_salary?.ToString("F").Replace(",", ".");
            Manually = _basicSalary == null ? false : true;
            EntryDate = employee.entry_date;
            CategoryId = employee.category_id;
            SocialWorkId = employee.social_work_id;
            LaborUnionId = employee.labor_union_id;
            ARTId = employee.art_id;
            LowDate = employee.low_date;
            State = employee.state ? 1 : 0;

            ScheduleIndex = employee.fixed_schedule_id == null ? 0 : 1;
            FixedScheduleId = employee.fixed_schedule_id;
        }

        private void LoadFixedSchedule()
        {
            Fixed_schedules fixedSchedules = FixedSchedules.Single(fs => fs.fixed_schedule_id == _fixedScheduleId);

            Lu_m_d = fixedSchedules.lu_m_d;
            Lu_m_h = fixedSchedules.lu_m_h;
            Lu_l_d = fixedSchedules.lu_l_d;
            Lu_l_h = fixedSchedules.lu_l_h;

            Ma_m_d = fixedSchedules.ma_m_d;
            Ma_m_h = fixedSchedules.ma_m_h;
            Ma_l_d = fixedSchedules.ma_l_d;
            Ma_l_h = fixedSchedules.ma_l_h;

            Mi_m_d = fixedSchedules.mi_m_d;
            Mi_m_h = fixedSchedules.mi_m_h;
            Mi_l_d = fixedSchedules.mi_l_d;
            Mi_l_h = fixedSchedules.mi_l_h;

            Ju_m_d = fixedSchedules.ju_m_d;
            Ju_m_h = fixedSchedules.ju_m_h;
            Ju_l_d = fixedSchedules.ju_l_d;
            Ju_l_h = fixedSchedules.ju_l_h;

            Vi_m_d = fixedSchedules.vi_m_d;
            Vi_m_h = fixedSchedules.vi_m_h;
            Vi_l_d = fixedSchedules.vi_l_d;
            Vi_l_h = fixedSchedules.vi_l_h;

            Sa_m_d = fixedSchedules.sa_m_d;
            Sa_m_h = fixedSchedules.sa_m_h;
            Sa_l_d = fixedSchedules.sa_l_d;
            Sa_l_h = fixedSchedules.sa_l_h;

            Do_m_d = fixedSchedules.do_m_d;
            Do_m_h = fixedSchedules.do_m_h;
            Do_l_d = fixedSchedules.do_l_d;
            Do_l_h = fixedSchedules.do_l_h;
        }

        private void UpdateProperties(Employees oldE, Employees newE)
        {
            if (oldE.name != newE.name) oldE.name = newE.name;
            if (oldE.last_name != newE.last_name) oldE.last_name = newE.last_name;
            if (oldE.birthdate != newE.birthdate) oldE.birthdate = newE.birthdate;
            if (oldE.sex_id != newE.sex_id) oldE.sex_id = newE.sex_id;
            if (oldE.document_n != newE.document_n) oldE.document_n = newE.document_n;
            if (oldE.cuil_n != newE.cuil_n) oldE.cuil_n = newE.cuil_n;
            if (oldE.email != newE.email) oldE.email = newE.email;
            if (oldE.street != newE.street) oldE.street = newE.street;
            if (oldE.street_n != newE.street_n) oldE.street_n = newE.street_n;
            if (oldE.floor != newE.floor) oldE.floor = newE.floor;
            if (oldE.departament != newE.departament) oldE.departament = newE.departament;
            if (oldE.province_id != newE.province_id) oldE.province_id = newE.province_id;
            if (oldE.city != newE.city) oldE.city = newE.city;
            if (oldE.postal_code != newE.postal_code) oldE.postal_code = newE.postal_code;
            if (oldE.phone_n != newE.phone_n) oldE.phone_n = newE.phone_n;
            if (oldE.basic_salary != newE.basic_salary) oldE.basic_salary = newE.basic_salary;
            if (oldE.prof_calification != newE.prof_calification) oldE.prof_calification = newE.prof_calification;
            if (oldE.entry_date != newE.entry_date) oldE.entry_date = newE.entry_date;
            if (oldE.category_id != newE.category_id) oldE.category_id = newE.category_id;
            if (oldE.social_work_id != newE.social_work_id) oldE.social_work_id = newE.social_work_id;
            if (oldE.labor_union_id != newE.labor_union_id) oldE.labor_union_id = newE.labor_union_id;
            if (oldE.art_id != newE.art_id) oldE.art_id = newE.art_id;
            if (oldE.low_date != newE.low_date) oldE.low_date = newE.low_date;
            if (oldE.state != newE.state) oldE.state = newE.state;
            if (oldE.fixed_schedule_id != newE.fixed_schedule_id) oldE.fixed_schedule_id = newE.fixed_schedule_id;
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
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(Employees.OrderBy(e => e.file_n).ToList().IndexOf(Employees.Single(e => e.file_n == _fileN)), -1);
                },
                () => { return Employees.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(Employees.OrderBy(e => e.file_n).ToList().IndexOf(Employees.Single(e => e.file_n == _fileN)), 1);
            },
            () => { return Employees.Count > 1; }
            );

        public ICommand New => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Agregando";
                    canCheckErrors = true;
                }
                );

        public ICommand Update => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Actualizando";
                    canCheckErrors = true;
                },
                () => { return _fileN != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Employees employee = await GetEmployeeWithFixedSchedule();
                canCheckErrors = false;

                if (employee.file_n == 0)
                {
                    await Task.Run(() => EmployeeB.InsertEmployee(employee));
                    allEmployees.Add(employee);
                    SearchEmployee();
                }
                else
                {
                    await Task.Run(() => EmployeeB.UpdateEmployee(employee));
                    UpdateProperties(Employees.Single(e => e.file_n == employee.file_n), employee);
                }

                FileN = employee.file_n;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_fileN == null)
                {
                    FileN = Employees.FirstOrDefault()?.file_n;
                }
                else
                {
                    LoadEmployee();
                }
            }
            );
        #endregion       
    }
}
