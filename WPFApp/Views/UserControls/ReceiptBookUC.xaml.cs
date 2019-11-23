using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.Helper;
using WPFApp.Views.Reports;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para ReceiptBookUC.xaml
    /// </summary>
    public partial class ReceiptBookUC : UserControl
    {
        public ReceiptBookUC()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, VisualizeReceiptsBook);
        }

        private void VisualizeReceiptsBook(NotificationMessage msg)
        {
            if (msg.Notification == "VisualizeReceiptsBook")
            {
                new ReportV().Show();
            }
        }

        private void NumberVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyNumber(sender, e);
        }
    }
}
