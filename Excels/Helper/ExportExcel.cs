using Data.Context;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Excels.Helper
{
    public class ExportExcel
    {
        public static async Task<bool> ExportSchedules(DateTime month, List<Calendars> calendarsH, List<Calendars> calendarsA, bool print, List<int> holidays)
        {
            Application xlApp = null;
            Workbook xlWorkBook = null;

            await Task.Run(() => xlApp = new Application());

            Nullable<Boolean> printed = null;
            Nullable<Boolean> exported = null;

            try
            {
                if (print)
                {
                    PrintDialog printDialog = new PrintDialog();
                    printed = printDialog.ShowDialog();

                    if (printed == true)
                    {
                        xlWorkBook = await GetExcel.GetSchedules(month, calendarsH, calendarsA, xlApp, holidays);
                        xlWorkBook.PrintOutEx();
                    }
                }
                else
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "HORARIOS_ASISTENCIAS_" + month.ToString("MMMM-yyyy").ToUpper();
                    saveFileDialog.Filter = "Documento Excel (*.xlsx)|*.xlsx";
                    exported = saveFileDialog.ShowDialog();

                    if (exported == true)
                    {
                        xlWorkBook = await GetExcel.GetSchedules(month, calendarsH, calendarsA, xlApp, holidays);
                        xlWorkBook.SaveAs(saveFileDialog.FileName);
                    }
                }
            }
            catch { }
            finally
            {
                xlWorkBook?.Close();
                xlApp.Quit();                
            }

            return (bool)(printed ?? exported);
        }        
    }
}
