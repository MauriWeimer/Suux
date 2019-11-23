using GalaSoft.MvvmLight.Messaging;
using WPFApp.Views.Reports;
using System.Windows.Controls;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para ReceiptUC.xaml
    /// </summary>
    public partial class ReceiptUC : UserControl
    {
        public ReceiptUC()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, VisualizeReceipts);
        }

        private void VisualizeReceipts(NotificationMessage msg)
        {
            if (msg.Notification == "VisualizeReceipts")
            {
                new ReportV().Show();
            }
        }
    }
}
