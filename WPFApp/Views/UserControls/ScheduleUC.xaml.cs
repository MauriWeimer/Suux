using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para ScheduleUC.xaml
    /// </summary>
    public partial class ScheduleUC : UserControl
    {
        private Style headerStyle;

        public ScheduleUC()
        {
            InitializeComponent();

            headerStyle = new Style(typeof(DataGridColumnHeader)) { BasedOn = FindResource("MaterialDesignDataGridColumnHeader") as Style };
            headerStyle.Setters.Add(new Setter(DataGridColumnHeader.HorizontalAlignmentProperty, HorizontalAlignment.Center));

            Messenger.Default.Register<NotificationMessage>(this, UnselectDataGrid);

            CreateSchedule(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        }

        private void UnselectDataGrid(NotificationMessage msg)
        {
            if (msg.Notification == "UnselectDataGrid") DataGrid.UnselectAllCells();
        }

        private void CreateSchedule(DateTime date)
        {
            DataGrid.Columns.Clear();
            for (int x = 0; x < DateTime.DaysInMonth(date.Year, date.Month); x++)
            {
                DateTime dayOfWeek = new DateTime(date.Year, date.Month, x + 1);

                Style cellStyle = new Style(typeof(DataGridCell)) { BasedOn = FindResource("CellStyle") as Style };
                cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new Binding("Schedules[" + x + "].color")));
                cellStyle.Setters.Add(new Setter(CustomProperties.VisibilityProperty, new Binding("Schedules[" + x + "].holiday") { Converter = new BooleanToVisibilityConverter() }));

                DataGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = dayOfWeek.ToString("ddd dd").ToUpper(),
                    Width = 80,
                    HeaderStyle = headerStyle,
                    Binding = new Binding("Schedules[" + x + "]." + dayOfWeek.DayOfWeek) { NotifyOnTargetUpdated = true },
                    CellStyle = cellStyle
                });
            }
        }

        private void SetBooleans(bool b)
        {
            DataGrid.IsEnabled = b;
            DataGrid.UnselectAllCells();
            Controls.IsEnabled = !b;
            Export.IsEnabled = !b;
            Confirm.IsEnabled = b;            
        }

        private void ClearChecks(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            EHR.IsChecked = false;
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            CreateSchedule(DateTime.Parse(Month.Text).AddMonths(-1));
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            CreateSchedule(DateTime.Parse(Month.Text).AddMonths(1));
        }

        private void NewOrUpdate(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);
        }

        private void Apply(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);
        }

        private void NumberVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyNumber(sender, e);
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
