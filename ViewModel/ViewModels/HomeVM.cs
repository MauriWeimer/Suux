using BusinessLayout.Business;
using Data.Context;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class HomeVM : ObservableObject
    {
        public HomeVM(string user)
        {
            Users u = UserB.SelectUser(user);

            UserName = u.fullname;

            Messenger.Default.Register<NotificationMessage>(this, ReloadCompany);
        }

        #region Properties
        public string UserName { get; set; }
        public string CompanyName => CompanyB.SelectCompany().company;        

        private bool _loadingVisibility;
        public bool LoadingVisibility
        {
            get => _loadingVisibility;
            set => Set(ref _loadingVisibility, value);
        }

        private ObservableObject _currentControl;
        public ObservableObject CurrentControl
        {
            get => _currentControl;
            set => Set(ref _currentControl, value);
        }
        #endregion

        #region Methods
        private void ReloadCompany(NotificationMessage msg)
        {
            if (msg.Notification == "ReloadCompany")
            {
                RaisePropertyChanged("CompanyName");
            }
        }

        private async void SetCurrentControl(string control)
        {
            LoadingVisibility = true;
            await Task.Run(() => CurrentControl = GetControl(control));
            LoadingVisibility = false;
        }

        private ObservableObject GetControl(string control)
        {
            switch (control)
            {
                case "Company":
                    return new CompanyVM();
                case "SocialWork":
                    return new SocialWorkVM();
                case "LaborUnion":
                    return new LaborUnionVM();
                case "Bank":
                    return new BankVM();
                case "ART":
                    return new ARTVM();
                case "Category":
                    return new CategoryVM();
                case "Employee":
                    return new EmployeeVM();
                case "Turn":
                    return new TurnVM();
                case "Schedule":
                    return new ScheduleVM();
                case "Concept":
                    return new ConceptVM();
                case "IndividualConcept":
                    return new IndividualConceptVM();
                case "LiquidationFixedData":
                    return new LiquidationFixedDataVM();
                case "IndividualLiquidation":
                    return new IndividualLiquidationVM();
                case "GlobalLiquidation":
                    return new GlobalLiquidationVM();
                case "Receipt":
                    return new ReceiptVM();
                case "ReceiptBook":
                    return new ReceiptBookVM();
                default:
                    return null;
            }
        }
        #endregion

        #region Commands
        public ICommand LogOutCommand => new RelayCommand
           (
           () =>
           {
               SimpleIoc.Default.Register<LoginVM>();
               SimpleIoc.Default.Unregister<HomeVM>();

               Messenger.Default.Send(new NotificationMessage("ShowLogin"));
           });

        public ICommand CompanyCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("Company");
           });

        public ICommand SocialWorkCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("SocialWork");
           });

        public ICommand LaborUnionCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("LaborUnion");
           });

        public ICommand BankCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("Bank");
           });

        public ICommand ARTCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("ART");
           });

        public ICommand CategoryCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("Category");
           });

        public ICommand EmployeeCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("Employee");
           });

        public ICommand TurnCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("Turn");
           });

        public ICommand ScheduleCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("Schedule");
           });

        public ICommand ConceptCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("Concept");
           });

        public ICommand IndividualConceptCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("IndividualConcept");
           });

        public ICommand LiquidationFixedDataCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("LiquidationFixedData");
           });

        public ICommand IndividualLiquidationCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("IndividualLiquidation");
           });

        public ICommand GlobalLiquidationCommand => new RelayCommand
            (
            () =>
            {
                SetCurrentControl("GlobalLiquidation");
            });

        public ICommand ReceiptCommand => new RelayCommand
            (
            () =>
            {
                SetCurrentControl("Receipt");
            });

        public ICommand ReceiptBookCommand => new RelayCommand
           (
           () =>
           {
               SetCurrentControl("ReceiptBook");
           });
        #endregion   
    }
}
