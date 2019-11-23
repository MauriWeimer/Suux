using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para CompanyUC.xaml
    /// </summary>
    public partial class CompanyUC : UserControl
    {
        public CompanyUC()
        {
            InitializeComponent();
        }

        private void SetBooleans(bool b)
        {
            Data.IsEnabled = b;
            Controls.IsEnabled = !b;
            Confirm.IsEnabled = b;
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
    }
}
