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
    public class LaborUnionVM : ObservableObject, IDataErrorInfo
    {
        #region ItemsSource
        public List<Provinces> Provinces => ProvinceB.SelectProvinces();

        private List<Labor_unions> allLaborUnions = LaborUnionB.SelectLaborUnions();
        public ObservableCollection<Labor_unions> LaborUnions { get; set; } = new ObservableCollection<Labor_unions>();
        #endregion

        #region Properties   
        private int? _laborUnionId;
        public int? LaborUnionId
        {
            get => _laborUnionId;
            set
            {
                Set(ref _laborUnionId, value);
                if (!searching)
                if (value != null && LaborUnions.Any(lu => lu.labor_union_id == value))
                {
                    LoadLaborUnion();
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
                            result = "El nombre del sindicato es requerido";
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
                SearchLaborUnion();
            }
        }

        private void SearchLaborUnion()
        {
            searching = true;
            if (string.IsNullOrWhiteSpace(_search))
            {
                allLaborUnions
                    .Where(lu => !LaborUnions.Any(lux => lux.labor_union_id == lu.labor_union_id))
                    .ToList()
                    .ForEach(lu => LaborUnions.Add(lu));
            }
            else
            {
                SearchLaborUnionByFilter();
            }

            searching = false;
            if (_laborUnionId == null) LaborUnionId = LaborUnions.FirstOrDefault()?.labor_union_id;
        }
        private void SearchLaborUnionByFilter()
        {
            switch (Filter)
            {
                case 0:
                    allLaborUnions
                        .Where(lu => LaborUnions.Any(lux => lux.labor_union_id == lu.labor_union_id) && lu.labor_union_id != int.Parse(_search))
                        .ToList()
                        .ForEach(lu => LaborUnions.Remove(lu));
                    allLaborUnions
                        .Where(lu => !LaborUnions.Any(lux => lux.labor_union_id == lu.labor_union_id) && lu.labor_union_id == int.Parse(_search))
                        .ToList()
                        .ForEach(lu => LaborUnions.Add(lu));
                    break;
                case 1:
                    allLaborUnions
                        .Where(lu => LaborUnions.Any(lux => lux.labor_union_id == lu.labor_union_id) && !lu.labor_union.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(lu => LaborUnions.Remove(lu));
                    allLaborUnions
                        .Where(lu => !LaborUnions.Any(lux => lux.labor_union_id == lu.labor_union_id) && lu.labor_union.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(lu => LaborUnions.Add(lu));
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

        private Labor_unions GetLaborUnion()
        {
            return new Labor_unions
            {
                labor_union_id = _laborUnionId ?? 0,
                labor_union = _name.ToUpper(),
                street = _street?.ToUpper(),
                street_n = _streetN,
                floor = _floor,
                departament = _departament?.ToUpper(),
                province_id = _provinceId,
                city = _city?.ToUpper(),
                postal_code = _postalCode,
                phone_n = _phoneN,
                percentage_retention = decimal.TryParse(_percentageRetention?.Replace(".", ","), out decimal resultp) ? resultp : (decimal?)null,
                amount_retention = decimal.TryParse(_amountRetention?.Replace(".", ","), out decimal resulta) ? resulta : (decimal?)null
            };
        }

        private void MoveSelection(int index, int n)
        {
            LaborUnionId = LaborUnions.OrderBy(lu => lu.labor_union_id).ToList()[(index + n) == LaborUnions.Count ? 0 : (index + n) == -1 ? (LaborUnions.Count - 1) : index + n].labor_union_id;
        }

        private void LoadLaborUnion()
        {
            Labor_unions laborUnion = LaborUnions.Single(lu => lu.labor_union_id == _laborUnionId);

            Name = laborUnion.labor_union;
            Street = laborUnion.street;
            StreetN = laborUnion.street_n;
            Floor = laborUnion.floor;
            Departament = laborUnion.departament;
            ProvinceId = laborUnion.province_id;
            City = laborUnion.city;
            PostalCode = laborUnion.postal_code;
            PhoneN = laborUnion.phone_n;
            PercentageRetention = laborUnion.percentage_retention?.ToString("F").Replace(",", ".");
            AmountRetention = laborUnion.amount_retention?.ToString("F").Replace(",", ".");
        }

        private void UpdateProperties(Labor_unions oldLU, Labor_unions newLU)
        {
            if (oldLU.labor_union != newLU.labor_union) oldLU.labor_union = newLU.labor_union;
            if (oldLU.street != newLU.street) oldLU.street = newLU.street;
            if (oldLU.street_n != newLU.street_n) oldLU.street_n = newLU.street_n;
            if (oldLU.floor != newLU.floor) oldLU.floor = newLU.floor;
            if (oldLU.departament != newLU.departament) oldLU.departament = newLU.departament;
            if (oldLU.province_id != newLU.province_id) oldLU.province_id = newLU.province_id;
            if (oldLU.city != newLU.city) oldLU.city = newLU.city;
            if (oldLU.postal_code != newLU.postal_code) oldLU.postal_code = newLU.postal_code;
            if (oldLU.phone_n != newLU.phone_n) oldLU.phone_n = newLU.phone_n;
            if (oldLU.percentage_retention != newLU.percentage_retention) oldLU.percentage_retention = newLU.percentage_retention;
            if (oldLU.amount_retention != newLU.amount_retention) oldLU.amount_retention = newLU.amount_retention;
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

        public string ConfirmDialogText => "¿Desea eliminar el sindicato?";
        public string AlertDialogText => "¡No se puede eliminar, este\nsindicato está siendo utilizado!";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _laborUnionId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    if (await Task.Run(() => LaborUnionB.DeleteLaborUnion((int)_laborUnionId)) == 547)
                    {
                        LoadingDialog = false;
                        AlertDialog = true;
                    }
                    else
                    {
                        allLaborUnions.Remove(allLaborUnions.Single(lu => lu.labor_union_id == _laborUnionId));
                        LaborUnions.Remove(LaborUnions.Single(lu => lu.labor_union_id == _laborUnionId));
                        LoadingDialog = false;
                    };
                });
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(LaborUnions.OrderBy(lu => lu.labor_union_id).ToList().IndexOf(LaborUnions.Single(lu => lu.labor_union_id == _laborUnionId)), -1);
                },
                () => { return LaborUnions.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(LaborUnions.OrderBy(lu => lu.labor_union_id).ToList().IndexOf(LaborUnions.Single(lu => lu.labor_union_id == _laborUnionId)), 1);
            },
            () => { return LaborUnions.Count > 1; }
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
                () => { return _laborUnionId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Labor_unions laborUnion = GetLaborUnion();
                canCheckErrors = false;

                if (laborUnion.labor_union_id == 0)
                {
                    await Task.Run(() => LaborUnionB.InsertLaborUnion(laborUnion));
                    allLaborUnions.Add(laborUnion);
                    SearchLaborUnion();                    
                }
                else
                {
                    await Task.Run(() => LaborUnionB.UpdateLaborUnion(laborUnion));
                    UpdateProperties(LaborUnions.Single(lu => lu.labor_union_id == laborUnion.labor_union_id), laborUnion);
                }

                LaborUnionId = laborUnion.labor_union_id;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_laborUnionId == null)
                {
                    LaborUnionId = LaborUnions.FirstOrDefault()?.labor_union_id;
                }
                else
                {
                    LoadLaborUnion();
                }
            }
            );
        #endregion      
    }
}
