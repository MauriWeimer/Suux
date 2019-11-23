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
    public class LiquidationFixedDataVM : ObservableObject, IDataErrorInfo
    {
        public LiquidationFixedDataVM()
        {
            LiquidationTypes = new ObservableCollection<Liquidation_types>(allLiquidationTypes);
            LiquidationFixedDataId = LiquidationFixedDatas
                    .OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .FirstOrDefault()?.liquidation_fixed_data_id;
        }

        #region ItemsSource
        public List<Banks> Banks => BankB.SelectBanks();

        private List<Liquidation_types> allLiquidationTypes = LiquidationTypeB.SelectLiquidationTypes();
        public ObservableCollection<Liquidation_types> LiquidationTypes { get; set; }

        public ObservableCollection<Liquidation_fixed_datas> LiquidationFixedDatas { get; set; } =
            new ObservableCollection<Liquidation_fixed_datas>(LiquidationFixedDataB.SelectLiquidationFixedDatasIncludeTypesAndEmployeesLiquidated());
        #endregion

        #region Properties   
        private int? _liquidationFixedDataId;
        public int? LiquidationFixedDataId
        {
            get => _liquidationFixedDataId;
            set
            {
                Set(ref _liquidationFixedDataId, value);
                if (value != null)
                {
                    LoadLiquidationFixedData();
                }
                else
                {
                    ClearProperties();
                }
            }
        }

        private string _dateS;
        public string DateS
        {
            get => _dateS;
            set => Set(ref _dateS, value);
        }

        private string _periodS;
        public string PeriodS
        {
            get => _periodS;
            set => Set(ref _periodS, value);
        }

        private string _liquidationTypeS;
        public string LiquidationTypeS
        {
            get => _liquidationTypeS;
            set => Set(ref _liquidationTypeS, value);
        }

        private DateTime? _period;
        public DateTime? Period
        {
            get => _period;
            set
            {
                Set(ref _period, value);
                LoadLiquidationTypes();
            }
        }

        private int? _liquidationTypeId;
        public int? LiquidationTypeId
        {
            get => _liquidationTypeId;
            set => Set(ref _liquidationTypeId, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private DateTime? _depositedPeriod;
        public DateTime? DepositedPeriod
        {
            get => _depositedPeriod;
            set => Set(ref _depositedPeriod, value);
        }

        private int? _bankId;
        public int? BankId
        {
            get => _bankId;
            set => Set(ref _bankId, value);
        }

        private DateTime? _depositedDate;
        public DateTime? DepositedDate
        {
            get => _depositedDate;
            set => Set(ref _depositedDate, value);
        }

        private bool _liquidationTypeEnabled;
        public bool LiquidationTypeEnabled
        {
            get => _liquidationTypeEnabled;
            set => Set(ref _liquidationTypeEnabled, value);
        }
        #endregion

        #region Errors
        public string Error => null;
        public string this[string property]
        {
            get => GetValidationError(property, canCheckErrors);
        }

        private static readonly string[] validatedProperties = { "Description", "Period", "LiquidationTypeId", "DepositedPeriod", "DepositedDate" };
        public bool IsValid
        {
            get
            {
                if (_liquidationTypeId == null) return false;

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
                    case "Description":
                        if (string.IsNullOrWhiteSpace(_description))
                        {
                            result = "La descripción es requerida";
                        }
                        break;
                    case "Period":
                        if (_period == null)
                        {
                            result = "El periodo de liquidación es requerido";
                        }
                        break;                    
                    case "LiquidationTypeId":
                        if (_liquidationTypeId == null)
                        {
                            result = "No hay tipos de liquidación disponibles con el periodo";
                        }
                        break;
                    case "DepositedPeriod":
                        if (_depositedPeriod != null && _depositedPeriod.Value >= _period.Value)
                        {                            
                            result = "El periodo depositado no puede ser igual o mayor al periodo de liquidación";
                        }
                        break;
                    case "DepositedDate":
                        if (_depositedDate != null && (_depositedDate.Value.Month != _depositedPeriod.Value.Month || _depositedDate.Value.Year != _depositedPeriod.Value.Year))
                        {
                            result = "La fecha depositada no corresponde al periodo depositado";
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

        #region Methods
        private void ClearProperties()
        {
            DateS = null;
            PeriodS = null;
            LiquidationTypeS = null;

            Period = null;
            Description = null;
            LiquidationTypeId = null;
            DepositedPeriod = null;
            BankId = null;
            DepositedDate = null;
        }

        private Liquidation_fixed_datas GetLiquidationFixedData()
        {
            return new Liquidation_fixed_datas
            {
                liquidation_fixed_data_id = _liquidationFixedDataId ?? 0,
                date = _liquidationFixedDataId == null ? DateTime.Now : LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId).date,
                period = (DateTime)_period,
                liquidation_type_id = (int)_liquidationTypeId,
                description = _description.ToUpper(),
                deposited_period = _depositedPeriod,
                bank_id = _bankId,
                deposited_date = _depositedDate
            };
        }

        private void MoveSelection(int index, int n)
        {
            LiquidationFixedDataId =
                LiquidationFixedDatas.OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .ToList()[(index + n) == LiquidationFixedDatas.Count ? 0 : (index + n) == -1 ? (LiquidationFixedDatas.Count - 1) : index + n]
                .liquidation_fixed_data_id;
        }

        private void LoadLiquidationFixedData()
        {
            Liquidation_fixed_datas liquidationFixedData = LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId);

            DateS = liquidationFixedData.date.ToString("dddd").ToUpper() + ", " +
                liquidationFixedData.date.ToString("dd") + " de " +
                liquidationFixedData.date.ToString("MMMM").ToUpper() + " de " +
                liquidationFixedData.date.ToString("yyyy");
            PeriodS = liquidationFixedData.period.ToString("MMMM").ToUpper() + " de " + liquidationFixedData.period.ToString("yyyy");
            LiquidationTypeS = liquidationFixedData.Liquidation_types.fullliquidationtype;

            Period = liquidationFixedData.period;
            Description = liquidationFixedData.description;
            LiquidationTypeId = liquidationFixedData.liquidation_type_id;
            DepositedDate = liquidationFixedData.deposited_date;
            BankId = liquidationFixedData.bank_id;
            DepositedPeriod = liquidationFixedData.deposited_period;
        }

        private void LoadLiquidationTypes()
        {
            List<Liquidation_types> liquidationTypes = LiquidationFixedDatas
                .Where(lfd => lfd.period == _period && lfd.liquidation_fixed_data_id != (_liquidationFixedDataId == null ? 0 : _liquidationFixedDataId))
                .Select(lfd => lfd.Liquidation_types)
                .ToList();

            allLiquidationTypes
                .Where(lt => liquidationTypes.Any(ltx => ltx.liquidation_type_id == lt.liquidation_type_id))
                .ToList()
                .ForEach(lt => LiquidationTypes.Remove(lt));
            allLiquidationTypes
                .Where(lt => !liquidationTypes.Any(ltx => ltx.liquidation_type_id == lt.liquidation_type_id) &&
                !LiquidationTypes.Any(ltx => ltx.liquidation_type_id == lt.liquidation_type_id))
                .ToList()
                .ForEach(lt => LiquidationTypes.Add(lt));

            if (_liquidationTypeId == null) LiquidationTypeId = LiquidationTypes.OrderBy(lt => lt.liquidation_type_id).FirstOrDefault()?.liquidation_type_id;
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
                    MoveSelection(LiquidationFixedDatas.OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .ToList().IndexOf(LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId)), -1);
                },
                () => { return LiquidationFixedDatas.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(LiquidationFixedDatas.OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .ToList().IndexOf(LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId)), 1);
            },
            () => { return LiquidationFixedDatas.Count > 1; }
            );

        public ICommand New => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Agregando";
                    LiquidationTypeEnabled = true;
                    canCheckErrors = true;
                }
                );

        public ICommand Update => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Actualizando";
                    if (LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId).Employees_liquidated.Count > 0)
                    {
                        LiquidationTypeEnabled = false;
                    }
                    else
                    {
                        LiquidationTypeEnabled = true;
                    }
                    canCheckErrors = true;
                },
                () => { return _liquidationFixedDataId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Liquidation_fixed_datas liquidationFixedData = GetLiquidationFixedData();
                canCheckErrors = false;

                if (liquidationFixedData.liquidation_fixed_data_id == 0)
                {
                    await Task.Run(() => LiquidationFixedDataB.InsertLiquidationFixedData(liquidationFixedData));
                    liquidationFixedData.Liquidation_types = LiquidationTypes.Single(lt => lt.liquidation_type_id == liquidationFixedData.liquidation_type_id);
                    LiquidationFixedDatas.Insert(0, liquidationFixedData);
                }
                else
                {
                    await Task.Run(() => LiquidationFixedDataB.UpdateLiquidationFixedData(liquidationFixedData));
                    liquidationFixedData.Liquidation_types = LiquidationTypes.Single(lt => lt.liquidation_type_id == liquidationFixedData.liquidation_type_id);
                    LiquidationFixedDatas[LiquidationFixedDatas
                        .IndexOf(LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == liquidationFixedData.liquidation_fixed_data_id))] = liquidationFixedData;
                }

                LiquidationFixedDataId = liquidationFixedData.liquidation_fixed_data_id;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_liquidationFixedDataId == null)
                {
                    LiquidationFixedDataId = LiquidationFixedDatas.OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id).FirstOrDefault()?.liquidation_fixed_data_id;
                }
                else
                {
                    LoadLiquidationFixedData();
                }
            }
            );
        #endregion      
    }
}
