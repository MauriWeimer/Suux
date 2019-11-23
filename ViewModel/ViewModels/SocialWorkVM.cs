using BusinessLayout.Business;
using Data.Context;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class SocialWorkVM : ObservableObject, IDataErrorInfo
    {
        #region ItemsSource
        public List<Provinces> Provinces => ProvinceB.SelectProvinces();

        private List<Social_works> allSocialWorks = SocialWorkB.SelectSocialWorks();
        public ObservableCollection<Social_works> SocialWorks { get; set; } = new ObservableCollection<Social_works>();
        #endregion

        #region Properties   
        private int? _socialWorkId;
        public int? SocialWorkId
        {
            get => _socialWorkId;
            set
            {
                Set(ref _socialWorkId, value);
                if (!searching)
                if (value != null && SocialWorks.Any(sw => sw.social_work_id == value))
                {
                    LoadSocialWork();
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

        private string _percentageRetention;
        public string PercentageRetention
        {
            get => _percentageRetention;
            set => Set(ref _percentageRetention, value);
        }

        private string _amountRetention;
        public string AmountRetention
        {
            get => _amountRetention;
            set => Set(ref _amountRetention, value);
        }
        #endregion

        #region Errors
        public string Error => null;
        public string this[string property]
        {
            get => GetValidationError(property, canCheckErrors);
        }

        private static readonly string[] validatedProperties = { "Name", "StreetN", "PostalCode", "PhoneN", "PercentageRetention", "AmountRetention" };
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_percentageRetention) && string.IsNullOrWhiteSpace(_amountRetention)) return false;

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
                            result = "El nombre de la obra social es requerido";
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
                SearchSocialWork();
            }
        }

        private void SearchSocialWork()
        {
            searching = true;
            if (string.IsNullOrWhiteSpace(_search))
            {
                allSocialWorks
                    .Where(sw => !SocialWorks.Any(swx => swx.social_work_id == sw.social_work_id))
                    .ToList()
                    .ForEach(sw => SocialWorks.Add(sw));
            }
            else
            {
                SearchSocialWorkByFilter();
            }

            searching = false;
            if (_socialWorkId == null) SocialWorkId = SocialWorks.FirstOrDefault()?.social_work_id;
        }
        private void SearchSocialWorkByFilter()
        {
            switch (Filter)
            {
                case 0:
                    allSocialWorks
                        .Where(sw => SocialWorks.Any(swx => swx.social_work_id == sw.social_work_id) && sw.social_work_id != int.Parse(_search))
                        .ToList()
                        .ForEach(sw => SocialWorks.Remove(sw));
                    allSocialWorks
                        .Where(sw => !SocialWorks.Any(swx => swx.social_work_id == sw.social_work_id)&& sw.social_work_id == int.Parse(_search))
                        .ToList()
                        .ForEach(sw => SocialWorks.Add(sw));
                    break;
                case 1:
                    allSocialWorks
                        .Where(sw => SocialWorks.Any(swx => swx.social_work_id == sw.social_work_id) && !sw.social_work.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(sw => SocialWorks.Remove(sw));
                    allSocialWorks
                        .Where(sw => !SocialWorks.Any(swx => swx.social_work_id == sw.social_work_id) && sw.social_work.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(sw => SocialWorks.Add(sw));
                    break;
            }
        }
        #endregion

        #region Methods
        private void ClearProperties()
        {
            Name = null;
            Street = null;
            StreetN = null;
            Floor = null;
            Departament = null;
            ProvinceId = null;
            City = null;
            PostalCode = null;
            PhoneN = null;
            PercentageRetention = null;
            AmountRetention = null;
        }

        private Social_works GetSocialWork()
        {
            return new Social_works
            {
                social_work_id = _socialWorkId ?? 0,
                social_work = _name.ToUpper(),
                street = _street?.ToUpper(),
                street_n = _streetN,
                floor = _floor,
                departament = _departament?.ToUpper(),
                province_id = _provinceId,
                city = _city?.ToUpper(),
                postal_code = _postalCode,
                phone_n = _phoneN,
                percentage_retention = decimal.TryParse(_percentageRetention?.Replace(".",","), out decimal resultp) ? resultp : (decimal?)null,
                amount_retention = decimal.TryParse(_amountRetention?.Replace(".", ","), out decimal resulta) ? resulta : (decimal?)null
            };
        }

        private void MoveSelection(int index, int n)
        {
            SocialWorkId = SocialWorks.OrderBy(sw => sw.social_work_id).ToList()[(index + n) == SocialWorks.Count ? 0 : (index + n) == -1 ? (SocialWorks.Count - 1) : index + n].social_work_id;
        }

        private void LoadSocialWork()
        {
            Social_works socialWork = SocialWorks.Single(sw => sw.social_work_id == _socialWorkId);

            Name = socialWork.social_work;
            Street = socialWork.street;
            StreetN = socialWork.street_n;
            Floor = socialWork.floor;
            Departament = socialWork.departament;
            ProvinceId = socialWork.province_id;
            City = socialWork.city;
            PostalCode = socialWork.postal_code;
            PhoneN = socialWork.phone_n;
            PercentageRetention = socialWork.percentage_retention?.ToString("F").Replace(",",".");
            AmountRetention = socialWork.amount_retention?.ToString("F").Replace(",", ".");
        }

        private void UpdateProperties(Social_works oldSW, Social_works newSW)
        {
            if (oldSW.social_work != newSW.social_work) oldSW.social_work = newSW.social_work;
            if (oldSW.street != newSW.street) oldSW.street = newSW.street;
            if (oldSW.street_n != newSW.street_n) oldSW.street_n = newSW.street_n;
            if (oldSW.floor != newSW.floor) oldSW.floor = newSW.floor;
            if (oldSW.departament != newSW.departament) oldSW.departament = newSW.departament;
            if (oldSW.province_id != newSW.province_id) oldSW.province_id = newSW.province_id;
            if (oldSW.city != newSW.city) oldSW.city = newSW.city;
            if (oldSW.postal_code != newSW.postal_code) oldSW.postal_code = newSW.postal_code;
            if (oldSW.phone_n != newSW.phone_n) oldSW.phone_n = newSW.phone_n;
            if (oldSW.percentage_retention != newSW.percentage_retention) oldSW.percentage_retention = newSW.percentage_retention;
            if (oldSW.amount_retention != newSW.amount_retention) oldSW.amount_retention = newSW.amount_retention;
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

        private bool _alertDialog;
        public bool AlertDialog
        {
            get => _alertDialog;
            set => Set(ref _alertDialog, value);
        }

        public string ConfirmDialogText => "¿Desea eliminar la obra social?";
        public string AlertDialogText => "¡No se puede eliminar, esta\nobra social está siendo utilizada!";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _socialWorkId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    if (await Task.Run(() => SocialWorkB.DeleteSocialWork((int)_socialWorkId)) == 547)
                    {
                        LoadingDialog = false;
                        AlertDialog = true;
                    }
                    else
                    {
                        allSocialWorks.Remove(allSocialWorks.Single(sw => sw.social_work_id == _socialWorkId));
                        SocialWorks.Remove(SocialWorks.Single(sw => sw.social_work_id == _socialWorkId));
                        LoadingDialog = false;
                    };
                });
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(SocialWorks.OrderBy(sw => sw.social_work_id).ToList().IndexOf(SocialWorks.Single(sw => sw.social_work_id == _socialWorkId)), -1);
                },
                () => { return SocialWorks.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(SocialWorks.OrderBy(sw => sw.social_work_id).ToList().IndexOf(SocialWorks.Single(sw => sw.social_work_id == _socialWorkId)), 1);
            },
            () => { return SocialWorks.Count > 1; }
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
                () => { return _socialWorkId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Social_works socialWork = GetSocialWork();
                canCheckErrors = false;

                if (socialWork.social_work_id == 0)
                {
                    await Task.Run(() => SocialWorkB.InsertSocialWork(socialWork));
                    allSocialWorks.Add(socialWork);
                    SearchSocialWork();                    
                }
                else
                {
                    await Task.Run(() => SocialWorkB.UpdateSocialWork(socialWork));
                    UpdateProperties(SocialWorks.Single(sw => sw.social_work_id == socialWork.social_work_id), socialWork);
                }

                SocialWorkId = socialWork.social_work_id;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_socialWorkId == null)
                {
                    SocialWorkId = SocialWorks.FirstOrDefault()?.social_work_id;
                }
                else
                {
                    LoadSocialWork();
                }
            }
            );
        #endregion            
    }
}
