using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para LaborUnionUC.xaml
    /// </summary>
    public partial class LaborUnionUC : UserControl
    {
        public LaborUnionUC()
        {
            InitializeComponent();
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null) DataGrid.ScrollIntoView(DataGrid.SelectedItem);
        }

        private void FilterChangedIndex(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox?.Text)) SearchBox.Text = null;
            SearchBox?.Focus();
        }

        private void ClearSearchBox(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox.Text)) SearchBox.Text = null;
            SearchBox.Focus();
        }

        private void SearchBoxVerify(object sender, KeyEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0) VerifyInput.VerifyNumber(sender, e);
        }

        private void SetBooleans(bool b)
        {
            Data.IsEnabled = b;
            Controls.IsEnabled = !b;
            Confirm.IsEnabled = b;
            Search.IsEnabled = !b;
        }

        private void New(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);

            DataGrid.UnselectAll();
            NameBox.Focus();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);

            NameBox.SelectionStart = NameBox.Text.Length;
            NameBox.Focus();
        }
        
        private void ApplyOrCanel(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);
        }

        private void NumberVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyNumber(sender, e);
        }

        private void DecimalVerify5(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDecimal(sender, e, 5);
        }

        private void DecimalVerify9(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDecimal(sender, e, 9);
        }

        private void DialogOpened(object sender, MaterialDesignThemes.Wpf.DialogOpenedEventArgs eventArgs)
        {
            Search.IsEnabled = false;
        }

        private void DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            Search.IsEnabled = true;
        }
    }
}
