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
    public class ReceiptBookVM : ObservableObject
    {
        public ReceiptBookVM()
        {
            allLiquidationFixedDatas.Select(lfd => lfd.period).Distinct().ToList().ForEach(p => Periods.Add(p, p.ToString("MMMM/yyyy").ToUpper()));
            if (Periods.Count > 0) Period = Periods.First().Key;
        }

        #region ItemsSource
        public Dictionary<DateTime, string> Periods { get; set; } = new Dictionary<DateTime, string>();

        private List<Liquidation_fixed_datas> allLiquidationFixedDatas =
            LiquidationFixedDataB.SelectLiquidationFixedDatasIncludeAll().Where(lfd => lfd.Employees_liquidated.Count > 0).ToList();
        public ObservableCollection<Liquidation_fixed_datas> LiquidationFixedDatas { get; set; } = new ObservableCollection<Liquidation_fixed_datas>();
        #endregion

        #region Properties   
        private DateTime _period;
        public DateTime Period
        {
            get => _period;
            set
            {
                Set(ref _period, value);                
                LoadLiquidationFixedDatas();
                CheckAllLiquidationFixedDatas = true;
            }
        }

        public bool HideHeader { get; set; }
        public bool ShowTotals { get; set; }
        public int? FolioN { get; set; }

        private bool _checkAllLiquidationFixedDatas;
        public bool CheckAllLiquidationFixedDatas
        {
            get => _checkAllLiquidationFixedDatas;
            set
            {
                Set(ref _checkAllLiquidationFixedDatas, value);
                LiquidationFixedDatas.ToList().ForEach(lfd => lfd.emited = value);
            }
        }
        #endregion

        #region Methods
        private void LoadLiquidationFixedDatas()
        {
            LiquidationFixedDatas.Clear();

            allLiquidationFixedDatas
                .Where(lfd => lfd.period == _period)
                .ToList()
                .ForEach(lfd => LiquidationFixedDatas.Add(lfd));
        }

        private async Task PrintOrExport(bool print)
        {
            if (await ExportReports.PrintOrExportReceiptsBook(LiquidationFixedDatas.Where(lfd => lfd.emited).Select(lfd => lfd.liquidation_fixed_data_id).ToArray(),
                HideHeader, ShowTotals, FolioN ?? 1, _period.ToString("MMMM-yyyy").ToUpper(), print))
            {
                TextLoadingDialog = "Realizando\nbackup";
                await ExportReports.BackupReceiptsBook(LiquidationFixedDatas.Where(lfd => lfd.emited).Select(lfd => lfd.liquidation_fixed_data_id).ToArray(),
                    HideHeader, ShowTotals, FolioN ?? 1, _period.ToString("MMMM-yyyy").ToUpper());
            }
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

        public ICommand OpenExportDialog => new RelayCommand
               (
               () => { ExportDialog = true; },
               () => { return LiquidationFixedDatas.Any(lfd => lfd.emited); }
               );
        #endregion

        #region Commands
        public ICommand Visualize => new RelayCommand
            (
            async () =>
            {
                ExportDialog = false;
                TextLoadingDialog = "Visualizando";
                LoadingDialog = true;

                var report = await GetReports.GetReceiptsBook(LiquidationFixedDatas.Where(lfd => lfd.emited).Select(lfd => lfd.liquidation_fixed_data_id).ToArray(),
                    HideHeader, ShowTotals, FolioN ?? 1);
                SimpleIoc.Default.Register<Reports.ReportVM>(() => new Reports.ReportVM(report, "Suux - Libro de sueldos"));
                Messenger.Default.Send(new NotificationMessage("VisualizeReceiptsBook"));
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
