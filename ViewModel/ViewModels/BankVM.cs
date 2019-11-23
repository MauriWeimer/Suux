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
    public class BankVM : ObservableObject, IDataErrorInfo
    {      
        #region ItemsSource
        public List<Provinces> Provinces => ProvinceB.SelectProvinces();

        private List<Banks> allBanks = BankB.SelectBanks();
        public ObservableCollection<Banks> Banks { get; set; } = new ObservableCollection<Banks>();            
        #endregion

        #region Properties   
        private int? _bankId;
        public int? BankId
        {
            get => _bankId;
            set
            {
                Set(ref _bankId, value);
                if (!searching)
                if (value != null && Banks.Any(b => b.bank_id == value))
                {
                    LoadBank();
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
                allBanks
                    .Where(b => !Banks.Any(bx => bx.bank_id == b.bank_id))
                    .ToList()
                    .ForEach(b => Banks.Add(b));
            }
            else
            {
                SearchBankByFilter();
            }

            searching = false;
            if (_bankId == null) BankId = Banks.FirstOrDefault()?.bank_id;
        }
        private void SearchBankByFilter()
        {
            switch (Filter)
            {
                case 0:
                    allBanks
                        .Where(b => Banks.Any(bx => bx.bank_id == b.bank_id) && b.bank_id != int.Parse(_search))
                        .ToList()
                        .ForEach(b => Banks.Remove(b));
                    allBanks
                        .Where(b => !Banks.Any(bx => bx.bank_id == b.bank_id) && b.bank_id == int.Parse(_search))
                        .ToList()
                        .ForEach(b => Banks.Add(b));
                    break;
                case 1:
                    allBanks
                        .Where(b => Banks.Any(bx => bx.bank_id == b.bank_id) && !b.bank.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(b => Banks.Remove(b));
                    allBanks
                        .Where(b => !Banks.Any(bx => bx.bank_id == b.bank_id) && b.bank.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(b => Banks.Add(b));
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

        private Banks GetBank()
        {
            return new Banks
            {
                bank_id = _bankId ?? 0,
                bank = _name.ToUpper(),
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
            BankId = Banks.OrderBy(b => b.bank_id).ToList()[(index + n) == Banks.Count ? 0 : (index + n) == - 1 ? (Banks.Count - 1) : index + n].bank_id;
        }

        private void LoadBank()
        {
            Banks bank = Banks.Single(b => b.bank_id == _bankId);

            Name = bank.bank;
            Street = bank.street;
            StreetN = bank.street_n;
            Floor = bank.floor;
            Departament = bank.departament;
            ProvinceId = bank.province_id;
            City = bank.city;
            PostalCode = bank.postal_code;
            PhoneN = bank.phone_n;
        }

        private void UpdateProperties(Banks oldB, Banks newB)
        {
            if (oldB.bank != newB.bank) oldB.bank = newB.bank;
            if (oldB.street != newB.street) oldB.street = newB.street;
            if (oldB.street_n != newB.street_n) oldB.street_n = newB.street_n;
            if (oldB.floor != newB.floor) oldB.floor = newB.floor;
            if (oldB.departament != newB.departament) oldB.departament = newB.departament;
            if (oldB.province_id != newB.province_id) oldB.province_id = newB.province_id;
            if (oldB.city != newB.city) oldB.city = newB.city;
            if (oldB.postal_code != newB.postal_code) oldB.postal_code = newB.postal_code;
            if (oldB.phone_n != newB.phone_n) oldB.phone_n = newB.phone_n;
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

        public string ConfirmDialogText => "¿Desea eliminar el banco?";
        public string AlertDialogText => "¡No se puede eliminar, este\nbanco está siendo utilizado!";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _bankId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    if (await Task.Run(() => BankB.DeleteBank((int)_bankId)) == 547)
                    {
                        LoadingDialog = false;
                        AlertDialog = true;
                    }
                    else
                    {
                        allBanks.Remove(allBanks.Single(b => b.bank_id == _bankId));
                        Banks.Remove(Banks.Single(b => b.bank_id == _bankId));
                        LoadingDialog = false;
                    };
                });
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(Banks.OrderBy(b => b.bank_id).ToList().IndexOf(Banks.Single(b => b.bank_id == _bankId)), -1);
                },
                () => { return Banks.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(Banks.OrderBy(b => b.bank_id).ToList().IndexOf(Banks.Single(b => b.bank_id == _bankId)), 1);
            },
            () => { return Banks.Count > 1; }
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
                () => { return _bankId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Banks bank = GetBank();
                canCheckErrors = false;

                if (bank.bank_id == 0)
                {
                    await Task.Run(() => BankB.InsertBank(bank));
                    allBanks.Add(bank);
                    SearchBank();                    
                }
                else
                {
                    await Task.Run(() => BankB.UpdateBank(bank));
                    UpdateProperties(Banks.Single(b => b.bank_id == bank.bank_id), bank);
                }

                BankId = bank.bank_id;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_bankId == null)
                {
                    BankId = Banks.FirstOrDefault()?.bank_id;
                }
                else
                {
                    LoadBank();
                }                
            }
            );
        #endregion    
    }
}