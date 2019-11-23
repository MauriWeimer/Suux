using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using ViewModel.ViewModels;
using ViewModel.ViewModels.Reports;

namespace ViewModel.Locator
{
    public class Locator
    {
        public Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<LoginVM>();
        }

        public LoginVM LoginVM
        {
            get { return SimpleIoc.Default.GetInstance<LoginVM>(); }
        }

        public HomeVM HomeVM
        {
            get { return SimpleIoc.Default.GetInstance<HomeVM>(); }
        }

        public ReportVM ReportVM
        {
            get { return SimpleIoc.Default.GetInstance<ReportVM>(); }
        }
    }
}
