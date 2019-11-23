using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFApp.Helper;

namespace WPFApp.Views.UserControls
{
    /// <summary>
    /// Lógica de interacción para ConceptUC.xaml
    /// </summary>
    public partial class ConceptUC : UserControl
    {
        private string v;

        public ConceptUC()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, "Formula", InvalidVariableFormula);
            Messenger.Default.Register<NotificationMessage>(this, "Quantity", InvalidVariableQuantity);
        }

        private void InvalidVariableFormula(NotificationMessage msg)
        {
            if (!string.IsNullOrEmpty(msg.Notification))
            {
                FormulaBox.SelectionStart = FormulaBox.Text.IndexOf(msg.Notification);
                FormulaBox.SelectionLength = msg.Notification.Length;
                FormulaBox.Focus();
            }
        }

        private void InvalidVariableQuantity(NotificationMessage msg)
        {
            if (!string.IsNullOrEmpty(msg.Notification))
            {
                QuantityBox.SelectionStart = QuantityBox.Text.IndexOf(msg.Notification);
                QuantityBox.SelectionLength = msg.Notification.Length;
                QuantityBox.Focus();
            }            
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null) DataGrid.ScrollIntoView(DataGrid.SelectedItem);
        }

        private void FilterChangedIndex(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox?.Text)) SearchBox.Text = null;
            SearchBox?.Focus();
        }

        private void ClearConceptTypeBox(object sender, RoutedEventArgs e)
        {
            ConceptTypeBox.SelectedValue = null;
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

        private void New(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);

            DataGrid.UnselectAll();
            TypeBox.Focus();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            SetBooleans(true);

            TypeBox.Focus();
        }
        private void ApplyOrCancel(object sender, RoutedEventArgs e)
        {
            SetBooleans(false);
        }       

        private void ClearVariableBox(object sender, RoutedEventArgs e)
        {
            VariableBox.Text = null;
            VariableBox.Focus();
        }

        private void TypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeBox.SelectedIndex < 0)
            {
                IdGrid.IsEnabled = false;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(IdBox.Text)) IdBox.Text = null;
                IdGrid.IsEnabled = true;                            
            }
        }

        private void NumberVerify(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyNumber(sender, e);
        }

        private void DecimalVerify5(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDecimal(sender, e, 5);
        }

        private void DecimalVerify9(object sender, KeyEventArgs e)
        {
            VerifyInput.VerifyDecimal(sender, e, 9);
        }

        private void SetTB(object sender, RoutedEventArgs e)
        {
            if (!VariableList.IsEnabled) VariableList.IsEnabled = true;
            if (!Operators.IsEnabled) Operators.IsEnabled = true;
        }

        private void UnsetTB(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(v))
            {
                TextBox tb = sender as TextBox;
                int selectionStart = tb.SelectionStart;
                if (tb.SelectionLength > 0) tb.Text = tb.Text.Remove(selectionStart, tb.SelectionLength);

                tb.Text = tb.Text.Insert(selectionStart, v);

                v = null;
            }

            if (VariableList.IsEnabled) VariableList.IsEnabled = false;
            if (Operators.IsEnabled) Operators.IsEnabled = false;
        }

        private void VariableSelected(object sender, MouseButtonEventArgs e)
        {
            v = ((TextBlock)sender).Text;
        }

        private void OperatorClick(object sender, RoutedEventArgs e)
        {
            v = ((TextBlock)((Button)sender).Content).Text;
        }

        private void Min(object sender, RoutedEventArgs e)
        {
            v = "<";
        }

        private void Max(object sender, RoutedEventArgs e)
        {
            v = ">";
        }

        private void DialogOpened(object sender, MaterialDesignThemes.Wpf.DialogOpenedEventArgs eventArgs)
        {
            Search.IsEnabled = false;
        }

        private void DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            Search.IsEnabled = true;
        }
    }
}
