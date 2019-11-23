using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace WPFApp.Views
{
    /// <summary>
    /// Lógica de interacción para HomeV.xaml
    /// </summary>
    public partial class HomeV : Window
    {
        public HomeV()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, ShowLogin);
        }

        private void ShowLogin(NotificationMessage msg)
        {
            if (msg.Notification == "ShowLogin")
            {
                new LoginV().Show();
                Close();
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearUC(object sender, RoutedEventArgs e)
        {
            if (CurrentControl.Content != null) CurrentControl.Content = null;
        }

        private void Files(object sender, RoutedEventArgs e)
        {
            SchedulesExpander.IsExpanded = false;
            ConceptsExpander.IsExpanded = false;
            LiquidationsExpander.IsExpanded = false;
            ExportsExpander.IsExpanded = false;
        }

        private void Schedules(object sender, RoutedEventArgs e)
        {
            FilesExpander.IsExpanded = false;
            ConceptsExpander.IsExpanded = false;
            LiquidationsExpander.IsExpanded = false;
            ExportsExpander.IsExpanded = false;
        }

        private void Concepts(object sender, RoutedEventArgs e)
        {
            FilesExpander.IsExpanded = false;
            SchedulesExpander.IsExpanded = false;
            LiquidationsExpander.IsExpanded = false;
            ExportsExpander.IsExpanded = false;
        }

        private void Liquidations(object sender, RoutedEventArgs e)
        {
            FilesExpander.IsExpanded = false;
            SchedulesExpander.IsExpanded = false;
            ConceptsExpander.IsExpanded = false;
            ExportsExpander.IsExpanded = false;
        }

        private void Exports(object sender, RoutedEventArgs e)
        {
            FilesExpander.IsExpanded = false;
            SchedulesExpander.IsExpanded = false;
            ConceptsExpander.IsExpanded = false;
            LiquidationsExpander.IsExpanded = false;
        }
    }
}
