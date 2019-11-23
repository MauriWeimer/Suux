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
    public class ARTVM : ObservableObject, IDataErrorInfo
    {
        #region ItemsSource
        public List<Provinces> Provinces => ProvinceB.SelectProvinces();

        private List<ART> allART = ARTB.SelectART();
        public ObservableCollection<ART> ART { get; set; } = new ObservableCollection<ART>();
        #endregion

        #region Properties   
        private int? _artId;
        public int? ARTId
        {
            get => _artId;
            set
            {
                Set(ref _artId, value);
                if (!searching)
                    if (value != null && ART.Any(a => a.art_id == value))
                    {
                        LoadART();
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
        #endregion

        #region Errors
        public string Error => null;
        public string this[string property]
        {
            get => GetValidationError(property, canCheckErrors);
        }

        private static readonly string[] validatedProperties = { "Name", "StreetN", "PostalCode", "PhoneN" };
        public bool IsValid
        {
            get
            {
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
                            result = "El nombre del banco es requerido";
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
                SearchBank();
            }
        }

        private void SearchBank()
        {
            searching = true;
            if (string.IsNullOrWhiteSpace(_search))
            {
                allART
                    .Where(a => !ART.Any(ax => ax.art_id == a.art_id))
                    .ToList()
                    .ForEach(a => ART.Add(a));
            }
            else
            {
                SearchBankByFilter();
            }

            searching = false;
            if (_artId == null) ARTId = ART.FirstOrDefault()?.art_id;
        }
        private void SearchBankByFilter()
        {
            switch (Filter)
            {
                case 0:
                    allART
                        .Where(a => ART.Any(ax => ax.art_id == a.art_id) && a.art_id != int.Parse(_search))
                        .ToList()
                        .ForEach(a => ART.Remove(a));
                    allART
                        .Where(a => !ART.Any(ax => ax.art_id == a.art_id) && a.art_id == int.Parse(_search))
                        .ToList()
                        .ForEach(a => ART.Add(a));
                    break;
                case 1:
                    allART
                        .Where(a => ART.Any(ax => ax.art_id == a.art_id) && !a.art1.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(a => ART.Remove(a));
                    allART
                        .Where(a => !ART.Any(ax => ax.art_id == a.art_id) && a.art1.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(a => ART.Add(a));
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
        }

        private ART GetART()
        {
            return new ART
            {
                art_id = _artId ?? 0,
                art1 = _name.ToUpper(),
                street = _street?.ToUpper(),
                street_n = _streetN,
                floor = _floor,
                departament = _departament?.ToUpper(),
                province_id = _provinceId,
                city = _city?.ToUpper(),
                postal_code = _postalCode,
                phone_n = _phoneN
            };
        }

        private void MoveSelection(int index, int n)
        {
            ARTId = ART.OrderBy(a => a.art_id).ToList()[(index + n) == ART.Count ? 0 : (index + n) == -1 ? (ART.Count - 1) : index + n].art_id;
        }

        private void LoadART()
        {
            ART art = ART.Single(a => a.art_id == _artId);

            Name = art.art1;
            Street = art.street;
            StreetN = art.street_n;
            Floor = art.floor;
            Departament = art.departament;
            ProvinceId = art.province_id;
            City = art.city;
            PostalCode = art.postal_code;
            PhoneN = art.phone_n;
        }

        private void UpdateProperties(ART oldA, ART newA)
        {
            if (oldA.art1 != newA.art1) oldA.art1 = newA.art1;
            if (oldA.street != newA.street) oldA.street = newA.street;
            if (oldA.street_n != newA.street_n) oldA.street_n = newA.street_n;
            if (oldA.floor != newA.floor) oldA.floor = newA.floor;
            if (oldA.departament != newA.departament) oldA.departament = newA.departament;
            if (oldA.province_id != newA.province_id) oldA.province_id = newA.province_id;
            if (oldA.city != newA.city) oldA.city = newA.city;
            if (oldA.postal_code != newA.postal_code) oldA.postal_code = newA.postal_code;
            if (oldA.phone_n != newA.phone_n) oldA.phone_n = newA.phone_n;
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

        public string ConfirmDialogText => "¿Desea eliminar la ART?";
        public string AlertDialogText => "¡No se puede eliminar, esta\nART está siendo utilizada!";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _artId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    if (await Task.Run(() => ARTB.DeleteART((int)_artId)) == 547)
                    {
                        LoadingDialog = false;
                        AlertDialog = true;
                    }
                    else
                    {
                        allART.Remove(allART.Single(a => a.art_id == _artId));
                        ART.Remove(ART.Single(a => a.art_id == _artId));
                        LoadingDialog = false;
                    };
                });
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(ART.OrderBy(a => a.art_id).ToList().IndexOf(ART.Single(a => a.art_id == _artId)), -1);
                },
                () => { return ART.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(ART.OrderBy(a => a.art_id).ToList().IndexOf(ART.Single(a => a.art_id == _artId)), 1);
            },
            () => { return ART.Count > 1; }
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
                () => { return _artId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                ART art = GetART();
                canCheckErrors = false;

                if (art.art_id == 0)
                {
                    await Task.Run(() => ARTB.InsertART(art));
                    allART.Add(art);
                    SearchBank();
                }
                else
                {
                    await Task.Run(() => ARTB.UpdateART(art));
                    UpdateProperties(ART.Single(a => a.art_id == art.art_id), art);
                }

                ARTId = art.art_id;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_artId == null)
                {
                    ARTId = ART.FirstOrDefault()?.art_id;
                }
                else
                {
                    LoadART();
                }
            }
            );
        #endregion    
    }
}
