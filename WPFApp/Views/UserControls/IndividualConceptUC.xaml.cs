using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para IndividualConceptUC.xaml
    /// </summary>
    public partial class IndividualConceptUC : UserControl
    {
        public IndividualConceptUC()
        {
            InitializeComponent();
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                UpdateBT.IsEnabled = true;
                DataGrid.ScrollIntoView(DataGrid.SelectedItem);
            }
            else
            {
                UpdateBT.IsEnabled = false;
            }
        }

        private void FilterChangedIndex(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox?.Text)) SearchBox.Text = null;
            SearchBox?.Focus();
        }

        private void ClearSearchBox(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox.Text)) SearchBox.Text = null;
            SearchBox.Focus();
        }

        private void SearchBoxVerify(object sender, KeyEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0) VerifyInput.VerifyNumber(sender, e);
        }

        private void SetBooleans(bool b)
        {
            Data.IsEnabled = b;
            Controls.IsEnabled = !b;
            Confirm.IsEnabled = b;
            Search.IsEnabled = !b;
        }       

        private void Update(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);
        }

        private void ApplyOrCanel(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);
        }

        private void ConceptListSelected(object sender, SelectionChangedEventArgs e)
        {
            IndividualList.UnselectAll();
        }

        private void IndividualListSelected(object sender, SelectionChangedEventArgs e)
        {
            ConceptList.UnselectAll();
        }                
    }
}
