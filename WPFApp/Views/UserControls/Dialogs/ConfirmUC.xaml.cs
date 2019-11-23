using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp.Views.UserControls.Dialogs
{
    /// <summary>
    /// Lógica de interacción para ConfirmUC.xaml
    /// </summary>
    public partial class ConfirmUC : UserControl
    {
        public ConfirmUC()
        {
            InitializeComponent();
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
