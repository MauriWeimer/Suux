using System.Windows;
using System.Windows.Controls;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para GlobalLiquidationUC.xaml
    /// </summary>
    public partial class GlobalLiquidationUC : UserControl
    {
        public GlobalLiquidationUC()
        {
            InitializeComponent();

            if (EmployeesList.SelectedItems.Count == 0) TabItem.IsEnabled = false;
        }

        private void EmployeesListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesList.SelectedItems.Count > 0)
            {
                TabItem.IsEnabled = true;
            }
            else
            {
                TabItem.IsEnabled = false;
            }
        }

        private void Accept(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;
        }

        private void TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedIndex == 0) LiquidationUC.SetBooleans(false);
        }
    }
}
