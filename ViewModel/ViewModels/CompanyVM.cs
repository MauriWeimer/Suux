using BusinessLayout.Business;
using Data.Context;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class CompanyVM : ObservableObject, IDataErrorInfo
    {
        public CompanyVM()
        {
            LoadCompany();
        }

        #region ItemsSource
        public List<Provinces> Provinces => ProvinceB.SelectProvinces();
        #endregion

        #region Properties
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

        private long? _cuitN;
        public long? CuitN
        {
            get => _cuitN;
            set => Set(ref _cuitN, value);
        }
        #endregion

        #region Errors
        public string Error => null;
        public string this[string property]
        {
            get => GetValidationError(property);
        }

        private static readonly string[] validatedProperties = { "Name", "Street", "StreetN", "ProvinceId", "City", "PostalCode", "PhoneN", "CuitN" };
        public bool IsValid
        {
            get
            {
                foreach (string property in validatedProperties)
                    if (!string.IsNullOrEmpty(GetValidationError(property))) return false;
                return true;
            }
        }

        private string GetValidationError(string property)
        {
            string result = null;
            switch (property)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace(_name))
                    {
                        result = "El nombre de la empresa es requerido";
                    }
                    break;
                case "Street":
                    if (string.IsNullOrWhiteSpace(_street))
                    {
                        result = "La calle es requerida";
                    }
                    break;
                case "StreetN":
                    if (_streetN != null && _streetN < 999)
                    {
                        result = "El n° de calle debe contener 4 dígitos";
                    }
                    break;
                case "ProvinceId":
                    if (_provinceId == null)
                    {
                        result = "La provincia es requerida";
                    }
                    break;
                case "City":
                    if (string.IsNullOrWhiteSpace(_city))
                    {
                        result = "La ciudad es requerida";
                    }
                    break;
                case "PostalCode":
                    if (_postalCode == null)
                    {
                        result = "El código postal es requerido";
                    }
                    else if (_postalCode < 999)
                    {
                        result = "El código postal debe contener 4 dígitos";
                    }
                    break;
                case "PhoneN":
                    if (_phoneN == null)
                    {
                        result = "El n° teléfonico es requerido";
                    }
                    else if (_phoneN < 9999999)
                    {
                        result = "El n° teléfonico debe contener más de 7 dígitos";
                    }
                    break;
                case "CuitN":
                    if (_cuitN == null)
                    {
                        result = "El n° de cuit es requerido";
                    }
                    else if (_cuitN < 9999999999)
                    {
                        result = "El n° de cuit debe contener 11 dígitos";
                    }
                    break;
            }
            return result;
        }
        #endregion

        #region Methods
        private void LoadCompany()
        {
            Companys company = CompanyB.SelectCompany();

            Name = company.company;
            Street = company.street;
            StreetN = company.street_n;
            ProvinceId = company.province_id;
            City = company.city;
            PostalCode = company.postal_code;
            PhoneN = company.phone_n;
            CuitN = company.cuit_n;
        }
        #endregion

        #region Dialog  
        public string TextLoadingDialog => "Actualizando";

        private bool _loadingDialog;
        public bool LoadingDialog
        {
            get => _loadingDialog;
            set => Set(ref _loadingDialog, value);
        }
        #endregion

        #region Commands
        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                await Task.Run(() => CompanyB.UpdateCompany(_name, _street, (int)_streetN, (int)_provinceId, _city, (int)_postalCode, (long)_phoneN, (long)_cuitN));
                Messenger.Default.Send(new NotificationMessage("ReloadCompany"));

                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                LoadCompany();
            }
            );
        #endregion
    }
}
