using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para EmployeeUC.xaml
    /// </summary>
    public partial class EmployeeUC : UserControl
    {
        public EmployeeUC()
        {
            InitializeComponent();

            BirthDate.DisplayDateEnd = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);
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
            DataTab.IsEnabled = b;
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
            Tab.SelectedIndex = 0;
        }

        private void StateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StateBox.SelectedIndex == 0)
            {
                LowDateBox.IsEnabled = true;
                LowDateBox.Focus();
            }
            else
            {
                LowDateBox.Text = null;
                LowDateBox.IsEnabled = false;
            }
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            BasicSalary.IsEnabled = true;
            BasicSalary.Focus();
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            BasicSalary.Text = null;
            BasicSalary.IsEnabled = false;
        }

        private void NumberVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyNumber(sender, e);
        }

        private void DecimalVerify9(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDecimal(sender, e, 9);
        }

        private void DateVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDate(sender, e);
        }

        private void ScheduleChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScheduleBox.SelectedIndex == 0)
            {
                Schedule.Visibility = Visibility.Hidden;
            }
            else
            {
                Schedule.Visibility = Visibility.Visible;
            }
        }

        private void TimeVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyTime(sender, e);
        }
    }
}
