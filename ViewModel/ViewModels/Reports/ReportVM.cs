namespace ViewModel.ViewModels.Reports
{
    public class ReportVM
    {
        public ReportVM(object report, string title)
        {
            Report = report;
            Title = title;
        }

        #region Properties        
        public object Report { get; set; }
        public string Title { get; set; }
        #endregion
    }
}
