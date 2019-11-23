using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;

namespace WPFApp.Views
{
    /// <summary>
    /// Lógica de interacción para LoginV.xaml
    /// </summary>
    public partial class LoginV : Window
    {
        public LoginV()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, ShowHome);

            if (string.IsNullOrEmpty(UserBox.Text)) UserBox.Focus();
        }

        private void ShowHome(NotificationMessage msg)
        {
            if (msg.Notification == "ShowHome")
            {
                new HomeV().Show();
                Close();
            }
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Loading.Visibility == Visibility.Visible)
            {
                LoginBT.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginBT.Visibility = Visibility.Visible;
            }
        }
    }
}
