using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls.Liquidation
{
    /// <summary>
    /// Lógica de interacción para LiquidationUC.xaml
    /// </summary>
    public partial class LiquidationUC : UserControl
    {
        public LiquidationUC()
        {
            InitializeComponent();
        }

        public void SetBooleans(bool b)
        {
            Liquidation.IsEnabled = !b;
            Data.IsEnabled = !b;
            Controls.IsEnabled = !b;
            Confirm.IsEnabled = b;
        }

        private void AddOrUpdate(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);
        }

        private void ApplyOrCancel(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);
        }

        private void DecimalVerify5(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDecimal(sender, e, 5);
        }

        private void DecimalVerify9(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDecimal(sender, e, 9);
        }

        private void DGLoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid.ScrollIntoView(DataGrid.Items.GetItemAt(DataGrid.Items.Count - 1));
            if (DataGrid.Items.Count > 7)
            {
                Thickness margin = new Thickness(0, -2, 18, 0);
                if (Border.Margin != margin) Border.Margin = margin;
            }
        }

        private void DGUnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (DataGrid.Items.Count < 8)
            {
                Thickness margin = new Thickness(0, -2, 0, 0);
                if (Border.Margin != margin) Border.Margin = margin;
            }
        }

        private void DGPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ((ScrollViewer)sender).ScrollToVerticalOffset(((ScrollViewer)sender).VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
