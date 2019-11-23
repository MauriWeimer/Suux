using BusinessLayout.Business;
using Data.Context;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Reports.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class ReceiptVM : ObservableObject
    {
        public ReceiptVM()
        {
            allLiquidationFixedDatas.Select(lfd => lfd.period).Distinct().ToList().ForEach(p => Periods.Add(p, p.ToString("MMMM/yyyy").ToUpper()));
            allLiquidationFixedDatas.ForEach(lfd => 
            {
                if (lfd.Employees_liquidated.Where(el => el.issue).Count() == lfd.Employees_liquidated.Count())
                {
                    lfd.emited = true;
                }
                else
                {
                    if (lfd.Employees_liquidated.Where(el => el.issue).Count() > 0) lfd.generated = true;
                }
            });

            LiquidationFixedDatas = new ObservableCollection<Liquidation_fixed_datas>(allLiquidationFixedDatas);
            if (Periods.Count > 0 ) Period = Periods.First().Key;
        }

        #region ItemsSource
        public Dictionary<DateTime, string> Periods { get; set; } = new Dictionary<DateTime, string>();

        private List<Liquidation_fixed_datas> allLiquidationFixedDatas =
            LiquidationFixedDataB.SelectLiquidationFixedDatasIncludeAll().Where(lfd => lfd.Employees_liquidated.Count > 0).ToList();
        public ObservableCollection<Liquidation_fixed_datas> LiquidationFixedDatas { get; set; }

        public ObservableCollection<Employees_liquidated> EmployeesLiquidated { get; set; } = new ObservableCollection<Employees_liquidated>();
        #endregion

        #region Properties   
        private DateTime _period;
        public DateTime Period
        {
            get => _period;
            set
            {
                Set (ref _period, value);
                SearchLiquidations();
                SearchGenerated();
                SearchEmited();
            }
        }

        private int? _liquidationFixedDataId;
        public int? LiquidationFixedDataId
        {
            get => _liquidationFixedDataId;
            set
            {
                Set(ref _liquidationFixedDataId, value);
                EmployeesLiquidated.Clear();                
                if (value != null) LoadLiquidationFixedDatas();
                CheckAllEmployees = true;
            }
        }

        private bool _generated;
        public bool Generated
        {
            set
            {
                _generated = value;
                SearchGenerated();
            }
        }

        private bool _emited;
        public bool Emited
        {
            set
            {
                _emited = value;
                SearchEmited();
            }
        }

        private bool _emitedEmployees;
        public bool EmitedEmployees
        {
            set
            {
                _emitedEmployees = value;
                LoadEmployees();
            }
        }

        private bool _checkAllEmployees;
        public bool CheckAllEmployees
        {
            get => _checkAllEmployees;
            set
            {
                Set (ref _checkAllEmployees, value);
                EmployeesLiquidated.ToList().ForEach(el => el.selected = value);
            }
        }

        public bool SendDigitalReceipt { get; set; }
        #endregion

        #region Search
        private void SearchLiquidations()
        {
            allLiquidationFixedDatas
                .Where(lfd => LiquidationFixedDatas.Any(lfdx => lfdx.liquidation_fixed_data_id == lfd.liquidation_fixed_data_id) &&
                (_period == null ? true : lfd.period != _period))
                .ToList()
                .ForEach(lfd => LiquidationFixedDatas.Remove(lfd));
            allLiquidationFixedDatas
                .Where(lfd => !LiquidationFixedDatas.Any(lfdx => lfdx.liquidation_fixed_data_id == lfd.liquidation_fixed_data_id) &&
                (_period == null ? true : lfd.period == _period))
                .ToList()
                .ForEach(lfd => LiquidationFixedDatas.Add(lfd));
        }

        private void SearchGenerated()
        {
            allLiquidationFixedDatas
                .Where(lfd => LiquidationFixedDatas.Any(lfdx => lfdx.liquidation_fixed_data_id == lfd.liquidation_fixed_data_id) &&
                (_generated ? false : lfd.generated))
                .ToList()
                .ForEach(lfd => LiquidationFixedDatas.Remove(lfd));
            allLiquidationFixedDatas
                .Where(lfd => !LiquidationFixedDatas.Any(lfdx => lfdx.liquidation_fixed_data_id == lfd.liquidation_fixed_data_id) &&
                (_generated ? true : !lfd.generated) &&
                !lfd.emited &&
                (_period == null ? true : lfd.period == _period))
                .ToList()
                .ForEach(lfd => LiquidationFixedDatas.Add(lfd));

            if (_liquidationFixedDataId == null) LiquidationFixedDataId = LiquidationFixedDatas
                    .OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .FirstOrDefault()?.liquidation_fixed_data_id;
        }

        private void SearchEmited()
        {
            allLiquidationFixedDatas
                .Where(lfd => LiquidationFixedDatas.Any(lfdx => lfdx.liquidation_fixed_data_id == lfd.liquidation_fixed_data_id) &&
                (_emited ? false : lfd.emited))
                .ToList()
                .ForEach(lfd => LiquidationFixedDatas.Remove(lfd));
            allLiquidationFixedDatas
                .Where(lfd => !LiquidationFixedDatas.Any(lfdx => lfdx.liquidation_fixed_data_id == lfd.liquidation_fixed_data_id) &&
                (_emited ? true : !lfd.emited) &&
                !lfd.generated &&
                (_period == null ? true : lfd.period == _period))
                .ToList()
                .ForEach(lfd => LiquidationFixedDatas.Add(lfd));

            if (_liquidationFixedDataId == null) LiquidationFixedDataId = LiquidationFixedDatas
                    .OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .FirstOrDefault()?.liquidation_fixed_data_id;
        }
        #endregion

        #region Methods
        private void MoveSelection(int index, int n)
        {
            LiquidationFixedDataId =
                LiquidationFixedDatas.OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .ToList()[(index + n) == LiquidationFixedDatas.Count ? 0 : (index + n) == -1 ? (LiquidationFixedDatas.Count - 1) : index + n]
                .liquidation_fixed_data_id;
        }

        private void LoadLiquidationFixedDatas()
        {
            LiquidationFixedDatas
                .Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId)
                .Employees_liquidated
                .ToList()
                .ForEach(el => EmployeesLiquidated.Add(el));            

            LoadEmployees();
        }

        private void LoadEmployees()
        {
            if (LiquidationFixedDatas
                .SingleOrDefault(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId) == null) return;

            List<Employees_liquidated> EmployeesLiquidated = LiquidationFixedDatas
                .Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId)
                .Employees_liquidated.ToList();

            EmployeesLiquidated
                .Where(el => this.EmployeesLiquidated.Any(elx => elx.employee_liquidated_id == el.employee_liquidated_id) && 
                (_emitedEmployees ? false : el.issue))
                .ToList()
                .ForEach(el => this.EmployeesLiquidated.Remove(el));
            EmployeesLiquidated
                .Where(el => !this.EmployeesLiquidated.Any(elx => elx.employee_liquidated_id == el.employee_liquidated_id) && 
                (_emitedEmployees ? true : !el.issue))
                .ToList()
                .ForEach(el => { el.selected = _checkAllEmployees; this.EmployeesLiquidated.Add(el); });
        }

        private async Task PrintOrExport(bool print)
        {
            Liquidation_fixed_datas liquidationFixedData = LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId);
            if (await ExportReports.PrintOrExportReceipts(EmployeesLiquidated.Where(el => el.selected).Select(el => el.file_n).ToArray(),
                (int)_liquidationFixedDataId,
                liquidationFixedData.Liquidation_types.liquidation_type_initials,
                _period.ToString("MMMM-yyyy").ToUpper(), print))
            {
                TextLoadingDialog = "Realizando\nbackup";
                await ExportReports.BackupReceipts(EmployeesLiquidated.Where(el => el.selected).Select(el => el.file_n).ToArray(),
                    (int)_liquidationFixedDataId,
                    liquidationFixedData.Liquidation_types.liquidation_type_initials,
                    _period.ToString("MMMM-yyyy").ToUpper());
                await Update();
            }
        }

        private async Task Update()
        {
            await Task.Run(() => EmployeeLiquidatedB.UpdateEmployeeLiquidated(EmployeesLiquidated.Where(el => el.selected).Select(el => el.employee_liquidated_id).ToList()));

            Liquidation_fixed_datas allLiquidationFixedData = allLiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId);
            Liquidation_fixed_datas liquidationFixedData = LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId);
            EmployeesLiquidated.Where(el => el.selected).ToList().ForEach(el => 
            {
                el.issue = true;
                allLiquidationFixedData.Employees_liquidated.Single(elx => elx.employee_liquidated_id == el.employee_liquidated_id).issue = true;
                liquidationFixedData.Employees_liquidated.Single(elx => elx.employee_liquidated_id == el.employee_liquidated_id).issue = true;
            });

            if (liquidationFixedData.Employees_liquidated.Where(el => el.issue).Count() == liquidationFixedData.Employees_liquidated.Count())
            {
                liquidationFixedData.generated = false;
                liquidationFixedData.emited = true;
            }
            else
            {
                if (liquidationFixedData.Employees_liquidated.Where(el => el.issue).Count() > 0) liquidationFixedData.generated = true;
            }

            SearchGenerated();
            SearchEmited();
            LoadEmployees();
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

        public string ConfirmDialogText => "¿Desea eliminar la liquidación?\n Se eliminará el dato fijo";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _liquidationFixedDataId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    await Task.Run(() => LiquidationFixedDataB.DeleteLiquidationFixedData((int)_liquidationFixedDataId));

                    allLiquidationFixedDatas.Remove(allLiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId));
                    LiquidationFixedDatas.Remove(LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId));

                    LoadingDialog = false;
                });

        public ICommand OpenExportDialog => new RelayCommand
               (
               () => { ExportDialog = true; },
               () => { return EmployeesLiquidated.Any(el => el.selected); }
               );
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

        public ICommand Visualize => new RelayCommand
            (
            async () =>
            {
                ExportDialog = false;
                TextLoadingDialog = "Visualizando";
                LoadingDialog = true;

                var report = await GetReports.GetReceipts(EmployeesLiquidated.Where(el => el.selected).Select(el => el.file_n).ToArray(),
                    (int)_liquidationFixedDataId);
                SimpleIoc.Default.Register<Reports.ReportVM>(() => new Reports.ReportVM(report, "Suux - Recibos"));
                Messenger.Default.Send(new NotificationMessage("VisualizeReceipts"));
                SimpleIoc.Default.Unregister<Reports.ReportVM>();

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

               await PrintOrExport(true);

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

               await PrintOrExport(false);

               LoadingDialog = false;
               AlertDialog = true;
           }
           );
        #endregion
    }
}
