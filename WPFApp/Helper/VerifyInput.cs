using System.Windows.Controls;
using System.Windows.Input;

namespace WPFApp.Helper
{
    public class VerifyInput
    {
        public static void VerifyNumber(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Clear) || (e.Key == Key.Tab))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public static void VerifyDecimal(object sender, KeyEventArgs e, int limit)
        {
            if ((e.Key == Key.Tab) || (e.Key == Key.Clear))
            {
                e.Handled = false;
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                string[] num = ((TextBox) sender).Text.Split('.');

                if (((TextBox)sender).SelectionLength > 0)
                {
                    e.Handled = false;
                    return;
                }

                if ((num[0].Length == limit - 2) && !(((TextBox)sender).Text.Contains(".")))
                {
                    e.Handled = true;
                }
                else
                {
                    if (((TextBox)sender).Text.Contains("."))
                    {
                        if ((num[1].Length == 2) && (((TextBox)sender).SelectionStart > ((TextBox)sender).Text.IndexOf(".")))
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            e.Handled = false;
                        }
                    }
                }
            }
            else if ((e.Key == Key.Decimal) && (((TextBox)sender).Text.Length == 0))
            {
                e.Handled = true;
            }
            else if ((e.Key == Key.Decimal) && !(((TextBox)sender).Text.Contains(".")))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public static void VerifyDate(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Tab) || (e.Key == Key.Oem2))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public static void VerifyTime(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Clear) || (e.Key == Key.Tab) || (e.Key == Key.OemPeriod))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
