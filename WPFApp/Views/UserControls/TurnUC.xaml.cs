using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para TurnUC.xaml
    /// </summary>
    public partial class TurnUC : UserControl
    {
        public TurnUC()
        {
            InitializeComponent();
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null) DataGrid.ScrollIntoView(DataGrid.SelectedItem);
        }

        private void SetBooleans(bool b)
        {
            Data.IsEnabled = b;
            Controls.IsEnabled = !b;
            Confirm.IsEnabled = b;
            TurnCard.IsEnabled = !b;
        }

        private void New(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);

            DataGrid.UnselectAll();
            Morningd.Focus();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);

            Morningd.Focus();
        }

        private void ApplyOrCanel(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);
        }

        private void TimeVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyTime(sender, e);
        }

        private void DialogOpened(object sender, MaterialDesignThemes.Wpf.DialogOpenedEventArgs eventArgs)
        {
            TurnCard.IsEnabled = false;
        }

        private void DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            TurnCard.IsEnabled = true;
        }
    }
}
