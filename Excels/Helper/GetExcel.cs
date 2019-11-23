using Data.Context;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Excels.Helper
{
    public class GetExcel
    {
        public static async Task<Workbook> GetSchedules(DateTime month, List<Calendars> calendarsH, List<Calendars> calendarsA, Application xlApp, List<int> holidays)
        {
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            Workbook xlWorkBook = xlApp.Workbooks.Add();

            Worksheet xlWorkSheetA = xlWorkBook.Worksheets[1];    
            await Task.Run(() =>
            {
                //PAGE SETUP
                xlWorkSheetA.Name = "Asistencias";
                xlWorkSheetA.Rows[1].RowHeight = 25;
                xlWorkSheetA.Rows[2].RowHeight = 15;
                xlWorkSheetA.PageSetup.PaperSize = XlPaperSize.xlPaperA4;
                xlWorkSheetA.PageSetup.TopMargin = 30;
                xlWorkSheetA.PageSetup.BottomMargin = 30;
                xlWorkSheetA.PageSetup.LeftMargin = 30;
                xlWorkSheetA.PageSetup.RightMargin = 30;

                for (int x = 3; x <= daysInMonth + 2; x++)
                {
                    xlWorkSheetA.Rows[x].RowHeight = 24.5;
                }
            });
            await SetAssistance(xlWorkSheetA, month, calendarsA, daysInMonth, holidays);            

            Worksheet xlWorkSheetH = xlWorkBook.Worksheets.Add();
            await Task.Run(() =>
            {
                //PAGE SETUP
                xlWorkSheetH.Name = "Horarios";
                xlWorkSheetH.Rows[1].RowHeight = 25;
                xlWorkSheetH.Rows[2].RowHeight = 15;
                xlWorkSheetH.PageSetup.PaperSize = XlPaperSize.xlPaperA4;
                xlWorkSheetH.PageSetup.TopMargin = 30;
                xlWorkSheetH.PageSetup.BottomMargin = 30;
                xlWorkSheetH.PageSetup.LeftMargin = 30;
                xlWorkSheetH.PageSetup.RightMargin = 30;

                for (int x = 3; x <= daysInMonth + 2; x++)
                {
                    xlWorkSheetH.Rows[x].RowHeight = 24.5;
                }
            });
            await SetSchedules(xlWorkSheetH, month, calendarsH, daysInMonth, holidays);

            return xlWorkBook;
        }

        private static async Task SetSchedules(Worksheet xlWorkSheet, DateTime month, List<Calendars> calendars, int daysInMonth, List<int> holidays)
        {
            await Task.Run(() =>
            {
                int calendarIndex = 0;                

                for (int x = 1; x < calendars.Count * 2; x += 2)
                {
                    //EMPLOYEE NAME
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, x], xlWorkSheet.Cells[1, x + 1]].Merge();
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, x], xlWorkSheet.Cells[1, x + 1]].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[1, x] = calendars[calendarIndex].Employees.fullname;
                    xlWorkSheet.Cells[1, x].Font.Bold = true;
                    xlWorkSheet.Cells[1, x].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[1, x].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    //MORNING
                    xlWorkSheet.Cells[2, x] = "MAÑANA";
                    xlWorkSheet.Columns[x].ColumnWidth = 20;
                    xlWorkSheet.Cells[2, x].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[2, x].Font.Bold = true;
                    xlWorkSheet.Cells[2, x].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[2, x].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    //LATE
                    xlWorkSheet.Cells[2, x + 1] = "TARDE";
                    xlWorkSheet.Columns[x + 1].ColumnWidth = 20;
                    xlWorkSheet.Cells[2, x + 1].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[2, x + 1].Font.Bold = true;
                    xlWorkSheet.Cells[2, x + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[2, x + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    for (int y = 1; y <= daysInMonth; y++)
                    {
                        //SCHEDULES
                        string schedule = GetFixedSechedule(new DateTime(month.Year, month.Month, y), calendars, calendarIndex);
                        string[] schedules = schedule?.Split(new char[] { '\n' });
                        if (schedules.Length == 2)
                        {
                            if (!string.IsNullOrEmpty(schedules[0]))
                            {
                                xlWorkSheet.Cells[y + 2, x].Borders.LineStyle = XlLineStyle.xlContinuous;
                                xlWorkSheet.Cells[y + 2, x] = schedules[0];
                            }

                            if (!string.IsNullOrEmpty(schedules[1]))
                            {
                                xlWorkSheet.Cells[y + 2, x + 1].Borders.LineStyle = XlLineStyle.xlContinuous;
                                xlWorkSheet.Cells[y + 2, x + 1] = schedules[1];
                            }

                            if (!string.IsNullOrEmpty(schedules[0]) || !string.IsNullOrEmpty(schedules[1]))
                            {
                                xlWorkSheet.Rows[y + 2].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                                xlWorkSheet.Rows[y + 2].VerticalAlignment = XlVAlign.xlVAlignCenter;
                                xlWorkSheet.Cells[y + 2, x].Interior.Color =
                                System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(calendars[calendarIndex].Schedules[y - 1].color));
                            }
                            else
                            {
                                xlWorkSheet.Cells[y + 2, x + 1].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                                if (y == daysInMonth)
                                    xlWorkSheet.Range[xlWorkSheet.Cells[y + 2, x], xlWorkSheet.Cells[y + 2, x + 1]]
                                    .Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                            }
                        }
                        else
                        {
                            xlWorkSheet.Range[xlWorkSheet.Cells[y + 2, x], xlWorkSheet.Cells[y + 2, x + 1]].Merge();
                            xlWorkSheet.Range[xlWorkSheet.Cells[y + 2, x], xlWorkSheet.Cells[y + 2, x + 1]].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x] = schedule;

                            xlWorkSheet.Rows[y + 2].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                            xlWorkSheet.Rows[y + 2].VerticalAlignment = XlVAlign.xlVAlignCenter;
                            xlWorkSheet.Range[xlWorkSheet.Cells[y + 2, x], xlWorkSheet.Cells[y + 2, x + 1]].Interior.Color =
                            System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml(calendars[calendarIndex].Schedules[y - 1].color));
                        }
                    }                    

                    calendarIndex++;
                }

                for (int y = 1; y < (calendars.Count * 5) / 2; y += 5)
                {
                    Range xlRng = xlWorkSheet.Range[xlWorkSheet.Cells[1, y], xlWorkSheet.Cells[1, y]];
                    xlRng.EntireColumn.Insert(XlInsertShiftDirection.xlShiftToRight, XlInsertFormatOrigin.xlFormatFromRightOrBelow);

                    xlWorkSheet.Columns[y].ColumnWidth = 11;

                    //MONTH
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, y], xlWorkSheet.Cells[2, y]].Merge();
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, y], xlWorkSheet.Cells[2, y]].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[1, y] = month.ToString("MMMM").ToUpper() + "\r \n" + month.ToString("yyyy");
                    xlWorkSheet.Cells[1, y].Font.Bold = true;
                    xlWorkSheet.Cells[1, y].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[1, y].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    for (int z = 1; z <= daysInMonth; z++)
                    {
                        //DAYS
                        var date = new DateTime(month.Year, month.Month, z);
                        xlWorkSheet.Cells[z + 2, y] = GetDay(date) + date.Day.ToString();
                        xlWorkSheet.Cells[z + 2, y].Font.Bold = true;
                        xlWorkSheet.Cells[z + 2, y].Borders.LineStyle = XlLineStyle.xlContinuous;
                        xlWorkSheet.Cells[z + 2, y].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        xlWorkSheet.Cells[z + 2, y].VerticalAlignment = XlVAlign.xlVAlignCenter;                        

                        if (holidays.Contains(z))
                        {
                            xlWorkSheet.Cells[z + 2, y].Interior.Color =
                            System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#FFEF5350"));
                        }
                        else
                        {
                            xlWorkSheet.Cells[z + 2, y].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        }
                    }
                }
            });
        }

        private static async Task SetAssistance(Worksheet xlWorkSheet, DateTime month, List<Calendars> calendars, int daysInMonth, List<int> holidays)
        {
            await Task.Run(() =>
            {
                int calendarIndex = 0;

                for (int x = 1; x <= calendars.Count * 9; x += 9)
                {
                    xlWorkSheet.Columns[x].ColumnWidth = 11;
                    xlWorkSheet.Columns[x + 1].ColumnWidth = 6;
                    xlWorkSheet.Columns[x + 2].ColumnWidth = 13.29;
                    xlWorkSheet.Columns[x + 3].ColumnWidth = 6;
                    xlWorkSheet.Columns[x + 4].ColumnWidth = 13.29;
                    xlWorkSheet.Columns[x + 5].ColumnWidth = 6;
                    xlWorkSheet.Columns[x + 6].ColumnWidth = 13.29;
                    xlWorkSheet.Columns[x + 7].ColumnWidth = 6;
                    xlWorkSheet.Columns[x + 8].ColumnWidth = 13.29;

                    //MONTH
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, x], xlWorkSheet.Cells[2, x]].Merge();
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, x], xlWorkSheet.Cells[2, x]].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[1, x] = month.ToString("MMMM").ToUpper() + "\r \n" + month.ToString("yyyy");
                    xlWorkSheet.Cells[1, x].Font.Bold = true;
                    xlWorkSheet.Cells[1, x].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[1, x].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    //EMPLOYEE NAME
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, x + 1], xlWorkSheet.Cells[1, x + 8]].Merge();
                    xlWorkSheet.Range[xlWorkSheet.Cells[1, x + 1], xlWorkSheet.Cells[1, x + 8]].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[1, x + 1] = calendars[calendarIndex].Employees.fullname;
                    xlWorkSheet.Cells[1, x + 1].Font.Bold = true;
                    xlWorkSheet.Cells[1, x + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[1, x + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    //MORNING
                    xlWorkSheet.Range[xlWorkSheet.Cells[2, x + 1], xlWorkSheet.Cells[2, x + 4]].Merge();
                    xlWorkSheet.Range[xlWorkSheet.Cells[2, x + 1], xlWorkSheet.Cells[2, x + 4]].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[2, x + 1] = "MAÑANA";
                    xlWorkSheet.Cells[2, x + 1].Font.Bold = true;
                    xlWorkSheet.Cells[2, x + 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[2, x + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    //LATE
                    xlWorkSheet.Range[xlWorkSheet.Cells[2, x + 5], xlWorkSheet.Cells[2, x + 8]].Merge();
                    xlWorkSheet.Range[xlWorkSheet.Cells[2, x + 5], xlWorkSheet.Cells[2, x + 8]].Borders.LineStyle = XlLineStyle.xlContinuous;
                    xlWorkSheet.Cells[2, x + 5] = "TARDE";
                    xlWorkSheet.Cells[2, x + 5].Font.Bold = true;
                    xlWorkSheet.Cells[2, x + 5].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[2, x + 5].VerticalAlignment = XlVAlign.xlVAlignCenter;

                    for (int y = 1; y <= daysInMonth; y++)
                    {
                        //DAYS
                        var date = new DateTime(month.Year, month.Month, y);
                        xlWorkSheet.Cells[y + 2, x] = GetDay(date) + date.Day.ToString();
                        xlWorkSheet.Cells[y + 2, x].Font.Bold = true;
                        xlWorkSheet.Cells[y + 2, x].Borders.LineStyle = XlLineStyle.xlContinuous;
                        xlWorkSheet.Cells[y + 2, x].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        xlWorkSheet.Cells[y + 2, x].VerticalAlignment = XlVAlign.xlVAlignCenter;

                        if (holidays.Contains(y))
                        {
                            xlWorkSheet.Cells[y + 2, x].Interior.Color =
                            System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#FFEF5350"));
                        }

                        //SCHEDULES
                        string schedule = GetFixedSechedule(new DateTime(month.Year, month.Month, y), calendars, calendarIndex);
                        string[] schedules = schedule.Split(new char[] { '-', '\n' });
                        if (schedules.Length == 4)
                        {
                            xlWorkSheet.Cells[y + 2, x + 1] = schedules[0].Trim() == "" ? "-" : schedules[0].Trim();
                            xlWorkSheet.Cells[y + 2, x + 1].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x + 2].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x + 3] = schedules[1].Trim() == "" ? "-" : schedules[1].Trim();
                            xlWorkSheet.Cells[y + 2, x + 3].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x + 4].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x + 5] = schedules[2].Trim() == "" ? "-" : schedules[2].Trim();
                            xlWorkSheet.Cells[y + 2, x + 5].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x + 6].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x + 7] = schedules[3].Trim() == "" ? "-" : schedules[3].Trim();
                            xlWorkSheet.Cells[y + 2, x + 7].Borders.LineStyle = XlLineStyle.xlContinuous;
                            xlWorkSheet.Cells[y + 2, x + 8].Borders.LineStyle = XlLineStyle.xlContinuous;

                            xlWorkSheet.Rows[y + 2].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                            xlWorkSheet.Rows[y + 2].VerticalAlignment = XlVAlign.xlVAlignCenter;
                        }
                        else
                        {
                            xlWorkSheet.Cells[y + 2, x + 8].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                            if (y == daysInMonth)
                                xlWorkSheet.Range[xlWorkSheet.Cells[y + 2, x + 1], xlWorkSheet.Cells[y + 2, x + 8]]
                                .Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                        }                        
                    }

                    calendarIndex++;
                }
            });
        }

        private static string GetFixedSechedule(DateTime dayOfWeek, List<Calendars> calendars, int calendarIndex)
        {
            switch (dayOfWeek.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return calendars[calendarIndex].Schedules[dayOfWeek.Day - 1].Monday;
                case DayOfWeek.Tuesday:
                    return calendars[calendarIndex].Schedules[dayOfWeek.Day - 1].Tuesday;
                case DayOfWeek.Wednesday:
                    return calendars[calendarIndex].Schedules[dayOfWeek.Day - 1].Wednesday;
                case DayOfWeek.Thursday:
                    return calendars[calendarIndex].Schedules[dayOfWeek.Day - 1].Thursday;
                case DayOfWeek.Friday:
                    return calendars[calendarIndex].Schedules[dayOfWeek.Day - 1].Friday;
                case DayOfWeek.Saturday:
                    return calendars[calendarIndex].Schedules[dayOfWeek.Day - 1].Saturday;
                case DayOfWeek.Sunday:
                    return calendars[calendarIndex].Schedules[dayOfWeek.Day - 1].Sunday;
                default:
                    return null;
            }
        }

        private static string GetDay(DateTime day)
        {
            switch (day.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "LUN. ";
                case DayOfWeek.Tuesday:
                    return "MAR. ";
                case DayOfWeek.Wednesday:
                    return "MIÉ. ";
                case DayOfWeek.Thursday:
                    return "JUE. ";
                case DayOfWeek.Friday:
                    return "VIE. ";
                case DayOfWeek.Saturday:
                    return "SÁB. ";
                case DayOfWeek.Sunday:
                    return "DOM. ";
                default:
                    return null;
            }
        }
    }    
}
