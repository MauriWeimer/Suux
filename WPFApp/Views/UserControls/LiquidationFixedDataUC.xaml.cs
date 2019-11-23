using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para FixedDataUC.xaml
    /// </summary>
    public partial class LiquidationFixedDataUC : UserControl
    {
        public LiquidationFixedDataUC()
        {
            InitializeComponent();

            Period.DisplayDateStart = DateTime.Now;
            PeriodL.DisplayDateEnd = DateTime.Now.AddMonths(-1);
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

            ComboBox.SelectedValue =null;
            Period.Focus();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);

            Period.Focus();
        }

        private void ApplyOrCanel(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);

            PeriodL.DisplayDateEnd = DateTime.Now.AddMonths(-1);
        }

        private void DateVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDate(sender, e);
        }

        private void PeriodChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Period.SelectedDate != null)
            {
                PeriodL.SelectedDate = null;
                PeriodL.DisplayDateEnd = Period.SelectedDate.Value.AddMonths(-1);
            }            
        }

        private void PeriodLChanged(object sender, SelectionChangedEventArgs e)
        {
            DateL.SelectedDate = null;
            if (PeriodL.SelectedDate == null)
            {                
                DateL.IsEnabled = false;
            }
            else
            {
                DateTime date = PeriodL.SelectedDate.Value;
                DateL.DisplayDateStart = date;
                DateL.DisplayDateEnd = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - 1);

                DateL.IsEnabled = true;
            }
        }
    }
}
