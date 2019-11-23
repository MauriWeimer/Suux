using BusinessLayout.Business;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class LoginVM : ObservableObject
    {
        public LoginVM()
        {
            User = Properties.UserSettings.Default.user;
            Password = Properties.UserSettings.Default.password;
            RememberUser = Properties.UserSettings.Default.remember;
        }

        #region Properties
        private bool _loadingVisibility;
        public bool LoadingVisibility
        {
            get => _loadingVisibility;
            set => Set(ref _loadingVisibility, value);
        }

        public string User { get; set; }
        public string Password { get; set; }
        public bool RememberUser { get; set; }

        private bool _errorVisibility;
        public bool ErrorVisibility
        {
            get => _errorVisibility;
            set => Set(ref _errorVisibility, value);
        }

        private string _errorMsg;
        public string ErrorMsg
        {
            get => _errorMsg;
            set => Set(ref _errorMsg, value);
        }
        #endregion

        #region Methods
        private void SetConfiguration()
        {
            if (RememberUser)
            {
                Properties.UserSettings.Default.user = User;
                Properties.UserSettings.Default.password = Password;
                Properties.UserSettings.Default.remember = RememberUser;
            }
            else
            {
                Properties.UserSettings.Default.user = null;
                Properties.UserSettings.Default.password = null;
                Properties.UserSettings.Default.remember = false;                
            }

            Properties.UserSettings.Default.Save();
        }
        #endregion

        #region Commands
        public ICommand Login => new RelayCommand
            (
            async () =>
            {
                ErrorVisibility = false;
                LoadingVisibility = true;

                switch (await Task.Run(() => LoginB.VerifyUser(User, Password)))
                {
                    case 1:
                        await Task.Run(() => SetConfiguration());

                        SimpleIoc.Default.Register<HomeVM>(() => new HomeVM(User));
                        SimpleIoc.Default.Unregister<LoginVM>();

                        Messenger.Default.Send(new NotificationMessage("ShowHome"));
                        break;
                    case 11:
                        ErrorMsg = "¡Ingrese un usuario!";
                        break;
                    case 12:
                        ErrorMsg = "¡Ingrese una contraseña!";
                        break;
                    case 13:
                        ErrorMsg = "¡Contraseña incorrecta!";
                        break;
                    case 14:
                        ErrorMsg = "¡Usuario incorrecto!";
                        break;
                }

                ErrorVisibility = true;
                LoadingVisibility = false;
            }
            );
        #endregion
    }
}
