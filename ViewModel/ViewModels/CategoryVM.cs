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
    public class CategoryVM : ObservableObject, IDataErrorInfo
    {
        #region ItemsSource
        public ObservableCollection<Fixed_schedules> FixedSchedules { get; set; } = new ObservableCollection<Fixed_schedules>(FixedScheduleB.SelectFixedSchedules());

        private List<Categorys> allCategorys = CategoryB.SelectCategorys();
        public ObservableCollection<Categorys> Categorys { get; set; } = new ObservableCollection<Categorys>();
        #endregion

        #region Properties   
        private int? _categoryId;
        public int? CategoryId
        {
            get => _categoryId;
            set
            {
                Set(ref _categoryId, value);
                if (!searching)
                if (value != null && Categorys.Any(c => c.category_id == value))
                {
                    LoadCategory();
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

        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private string _basicSalary;
        public string BasicSalary
        {
            get => _basicSalary;
            set => Set(ref _basicSalary, value);
        }

        private decimal? _amountExtra1;
        public decimal? AmountExtra1
        {
            get => _amountExtra1;
            set => Set(ref _amountExtra1, value);
        }

        private decimal? _amountExtra2;
        public decimal? AmountExtra2
        {
            get => _amountExtra2;
            set => Set(ref _amountExtra2, value);
        }

        private decimal? _amountExtra3;
        public decimal? AmountExtra3
        {
            get => _amountExtra3;
            set => Set(ref _amountExtra3, value);
        }

        private decimal? _amountExtra4;
        public decimal? AmountExtra4
        {
            get => _amountExtra4;
            set => Set(ref _amountExtra4, value);
        }
        #endregion

        #region Errors
        public string Error => null;
        public string this[string property]
        {
            get => GetValidationError(property, canCheckErrors);
        }

        private static readonly string[] validatedProperties = { "Name", "BasicSalary" };
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
                            result = "El nombre de la categoría es requerido";
                        }
                        break;
                    case "BasicSalary":
                        if (string.IsNullOrWhiteSpace(_basicSalary))
                        {
                            result = "El sueldo básico es requerido";
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
                SearchCategory();
            }
        }

        private void SearchCategory()
        {
            searching = true;
            if (string.IsNullOrWhiteSpace(_search))
            {
                allCategorys
                    .Where(c => !Categorys.Any(cx => cx.category_id == c.category_id))
                    .ToList()
                    .ForEach(c => Categorys.Add(c));
            }
            else
            {
                SearchCategoryByFilter();
            }

            searching = false;
            if (_categoryId == null) CategoryId = Categorys.FirstOrDefault()?.category_id;
        }
        private void SearchCategoryByFilter()
        {
            switch (Filter)
            {
                case 0:
                    allCategorys
                        .Where(c => Categorys.Any(cx => cx.category_id == c.category_id) && c.category_id != int.Parse(_search))
                        .ToList()
                        .ForEach(c => Categorys.Remove(c));
                    allCategorys
                        .Where(c => !Categorys.Any(cx => cx.category_id == c.category_id) && c.category_id == int.Parse(_search))
                        .ToList()
                        .ForEach(c => Categorys.Add(c));
                    break;
                case 1:
                    allCategorys
                        .Where(c => Categorys.Any(cx => cx.category_id == c.category_id) && !c.category.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(c => Categorys.Remove(c));
                    allCategorys
                        .Where(c => !Categorys.Any(cx => cx.category_id == c.category_id) && c.category.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(c => Categorys.Add(c));
                    break;
            }
        }
        #endregion

        #region Methods
        private void ClearProperties()
        {
            Name = null;
            Description = null;
            BasicSalary = null;
            AmountExtra1 = null;
            AmountExtra2 = null;
            AmountExtra3 = null;
            AmountExtra4 = null;
        }

        private Categorys GetCategory()
        {
            return new Categorys
            {
                category_id = _categoryId ?? 0,
                category = _name.ToUpper(),
                description = _description?.ToUpper(),
                basic_salary = decimal.Parse(_basicSalary.Replace(".", ",")),
                amount_extra_1 = _amountExtra1,
                amount_extra_2 = _amountExtra2,
                amount_extra_3 = _amountExtra3,
                amount_extra_4 = _amountExtra4
            };
        }

        private void MoveSelection(int index, int n)
        {
            CategoryId = Categorys.OrderBy(c => c.category_id).ToList()[(index + n) == Categorys.Count ? 0 : (index + n) == -1 ? (Categorys.Count - 1) : index + n].category_id;
        }

        private void LoadCategory()
        {
            Categorys category = Categorys.Single(c => c.category_id == _categoryId);

            Name = category.category;
            Description = category.description;
            BasicSalary = category.basic_salary.ToString("F").Replace(",", ".");
            AmountExtra1 = category.amount_extra_1;
            AmountExtra2 = category.amount_extra_2;
            AmountExtra3 = category.amount_extra_3;
            AmountExtra4 = category.amount_extra_4;
        }

        private void UpdateProperties(Categorys oldC, Categorys newC)
        {
            if (oldC.category != newC.category) oldC.category = newC.category;
            if (oldC.description != newC.description) oldC.description = newC.description;
            if (oldC.basic_salary != newC.basic_salary) oldC.basic_salary = newC.basic_salary;
            if (oldC.amount_extra_1 != newC.amount_extra_1) oldC.amount_extra_1 = newC.amount_extra_1;
            if (oldC.amount_extra_2 != newC.amount_extra_2) oldC.amount_extra_2 = newC.amount_extra_2;
            if (oldC.amount_extra_3 != newC.amount_extra_3) oldC.amount_extra_3 = newC.amount_extra_3;
            if (oldC.amount_extra_4 != newC.amount_extra_4) oldC.amount_extra_4 = newC.amount_extra_4;
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

        public string ConfirmDialogText => "¿Desea eliminar la categoría?";
        public string AlertDialogText => "¡No se puede eliminar, esta\ncategoría está siendo utilizada!";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _categoryId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    if (await Task.Run(() => CategoryB.DeleteCategory((int)_categoryId)) == 547)
                    {
                        LoadingDialog = false;
                        AlertDialog = true;
                    }
                    else
                    {
                        allCategorys.Remove(allCategorys.Single(c => c.category_id == _categoryId));
                        Categorys.Remove(Categorys.Single(c => c.category_id == _categoryId));
                        LoadingDialog = false;
                    };
                });
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(Categorys.OrderBy(c => c.category_id).ToList().IndexOf(Categorys.Single(c => c.category_id == _categoryId)), -1);
                },
                () => { return Categorys.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(Categorys.OrderBy(c => c.category_id).ToList().IndexOf(Categorys.Single(c => c.category_id == _categoryId)), 1);
            },
            () => { return Categorys.Count > 1; }
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
                () => { return _categoryId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;
                                
                Categorys category = GetCategory();
                canCheckErrors = false;

                if (category.category_id == 0)
                {
                    await Task.Run(() => CategoryB.InsertCategory(category));
                    allCategorys.Add(category);
                    SearchCategory();                    
                }
                else
                {
                    await Task.Run(() => CategoryB.UpdateCategory(category));
                    UpdateProperties(Categorys.Single(c => c.category_id == category.category_id), category);
                }

                CategoryId = category.category_id;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_categoryId == null)
                {
                    CategoryId = Categorys.FirstOrDefault()?.category_id;
                }
                else
                {
                    LoadCategory();
                }
            }
            );
        #endregion              
    }
}
