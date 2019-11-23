using BusinessLayout.Business;
using Data.Context;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.Win32.SafeHandles;

namespace BusinessLayout.Helper
{
    public class CalculateVariable : IDisposable
    {
        private decimal? number;
        public decimal _quantity { get; set; }
        public decimal? _r { get; set; }
        public decimal? _nr { get; set; }
        public decimal? _d { get; set; }
        private DataTable calculator = new DataTable();

        public decimal CalculateQuantity(string quantity, Employees e, Concepts c, DateTime period)
        {
            string[] variables = quantity.Split(new char[] { '+', '-', '*', '/', '(', ')', '?', '<', '>', '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            quantity = quantity.Replace("?", "IIF");

            foreach (string v in variables)
            {
                if (!decimal.TryParse(v, out decimal result))
                {
                    switch (v)
                    {
                        case "PORCE":
                            number = (c.percentage ?? 0) / 100;
                            break;
                        case "IMPOR":
                            number = c.amount;
                            break;
                        case "SUEBA":
                            number = e.basic_salary ?? e.Categorys.basic_salary;
                            break;
                        case "CATA1":
                            number = e.Categorys.amount_extra_1;
                            break;
                        case "CATA2":
                            number = e.Categorys.amount_extra_2;
                            break;
                        case "CATA3":
                            number = e.Categorys.amount_extra_3;
                            break;
                        case "CATA4":
                            number = e.Categorys.amount_extra_4;
                            break;
                        case "OBRRP":
                            number = (e.Social_works.percentage_retention ?? 0) / 100;
                            break;
                        case "OBRRI":
                            number = e.Social_works.amount_retention;
                            break;
                        case "SINRP":
                            number = (e.Labor_unions.percentage_retention ?? 0) / 100;
                            break;
                        case "SINRI":
                            number = e.Labor_unions.amount_retention;
                            break;
                        case "ANTIA":
                            number = GetYearsOfAntiguaty(e);
                            break;
                        case "ANTID":
                            number = DateTime.Now.Subtract(e.entry_date).Days;
                            break;
                        case "SEMDI":
                            number = GetSemesterDays(period);
                            break;

                        case "DIVAP":
                            number = GetHolidaysP(e, period);
                            break;
                        case "DIVAF":
                            number = GetHolidaysF(e, period);
                            break;
                    }
                    quantity = quantity.Replace(v, number?.ToString().Replace(",", ".") ?? "0");
                }
            }

            return Calculate(quantity, null);
        }

        public decimal CalculateFormula(string formula, Employees e, Concepts c, DateTime period)
        {
            string[] variables = formula.Split(new char[] { '+', '-', '*', '/', '(', ')', '?', '<', '>', '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            formula = formula.Replace("?", "IIF");

            foreach (string v in variables)
            {
                if (!decimal.TryParse(v, out decimal result))
                {
                    switch (v)
                    {
                        case "PORCE":
                            number = (c.percentage ?? 0) / 100;
                            break;
                        case "IMPOR":
                            number = c.amount;
                            break;
                        case "CANTI":
                            number = _quantity;
                            break;
                        case "SUEBA":
                            number = e.basic_salary ?? e.Categorys.basic_salary;
                            break;
                        case "CATA1":
                            number = e.Categorys.amount_extra_1;
                            break;
                        case "CATA2":
                            number = e.Categorys.amount_extra_2;
                            break;
                        case "CATA3":
                            number = e.Categorys.amount_extra_3;
                            break;
                        case "CATA4":
                            number = e.Categorys.amount_extra_4;
                            break;
                        case "OBRRP":
                            number = (e.Social_works.percentage_retention ?? 0) / 100;
                            break;
                        case "OBRRI":
                            number = e.Social_works.amount_retention;
                            break;
                        case "SINRP":
                            number = (e.Labor_unions.percentage_retention ?? 0) / 100;
                            break;
                        case "SINRI":
                            number = e.Labor_unions.amount_retention;
                            break;
                        case "ANTIA":
                            number = GetYearsOfAntiguaty(e);
                            break;
                        case "ANTID":
                            number = DateTime.Now.Subtract(e.entry_date).Days;
                            break;
                        case "TOTHA":
                            number = _r;
                            break;
                        case "SEMDI":
                            number = GetSemesterDays(period);
                            break;

                        case "MSUEB":
                            number = GetBestGrossSalary(e, period);
                            break;                        
                        case "SUEBP":
                            number = GetPastMonthGrossSalary(e, period);
                            break;
                        case "DIVAP":
                            number = GetHolidaysP(e, period);
                            break;
                        case "DIVAF":
                            number = GetHolidaysF(e, period);
                            break;
                        case "SUEBT":
                            number = _r ?? 0 + _nr ?? 0;
                            break;
                    }
                    formula = formula.Replace(v, number?.ToString().Replace(",",".") ?? "0");
                }                
            }

            return Calculate(formula, c.concept_type_id); 
        }

        public string GetEquation(string formulaOrQuantity, Employees e, Concepts c, DateTime period, bool quantity)
        {
            string[] variables = formulaOrQuantity.Split(new char[] { '+', '-', '*', '/', '(', ')', '?', '<', '>', '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
        
            foreach (string v in variables)
            {
                if (!decimal.TryParse(v, out decimal result))
                {
                    switch (v)
                    {
                        case "PORCE":
                            number = (c.percentage ?? 0) / 100;
                            break;
                        case "IMPOR":
                            number = c.amount;
                            break;
                        case "CANTI":
                            number = _quantity;
                            break;
                        case "SUEBA":
                            number = e.basic_salary ?? e.Categorys.basic_salary;
                            break;
                        case "CATA1":
                            number = e.Categorys.amount_extra_1;
                            break;
                        case "CATA2":
                            number = e.Categorys.amount_extra_2;
                            break;
                        case "CATA3":
                            number = e.Categorys.amount_extra_3;
                            break;
                        case "CATA4":
                            number = e.Categorys.amount_extra_4;
                            break;
                        case "OBRRP":
                            number = (e.Social_works.percentage_retention ?? 0) / 100;
                            break;
                        case "OBRRI":
                            number = e.Social_works.amount_retention;
                            break;
                        case "SINRP":
                            number = (e.Labor_unions.percentage_retention ?? 0) / 100;
                            break;
                        case "SINRI":
                            number = e.Labor_unions.amount_retention;
                            break;
                        case "ANTIA":
                            number = GetYearsOfAntiguaty(e);
                            break;
                        case "ANTID":
                            number = DateTime.Now.Subtract(e.entry_date).Days;
                            break;
                        case "SEMDI":
                            number = GetSemesterDays(period);
                            break;

                        case "MSUEB":
                            number = GetBestGrossSalary(e, period);
                            break;
                        case "SUEBP":
                            number = GetPastMonthGrossSalary(e, period);
                            break;
                        case "DIVAP":
                            number = GetHolidaysP(e, period);
                            break;
                        case "DIVAF":
                            number = GetHolidaysF(e, period);
                            break;
                    }
                    formulaOrQuantity = formulaOrQuantity.Replace(v, number?.ToString().Replace(",",".") ?? "0");
                }
            }

            if (quantity) Calculate(formulaOrQuantity, null);
            return formulaOrQuantity;
        }

        private decimal Calculate(string s, int? t)
        {
            try
            {
                decimal result = decimal.Parse(calculator.Compute(s, null).ToString());

                switch (t)
                {
                    case null:
                        _quantity = result;
                        break;
                    case 1:
                        if (_r == null) _r = 0;
                        _r += decimal.Round(result, 2);
                        break;
                    case 2:
                        if (_nr == null) _nr = 0;
                        _nr += decimal.Round(result ,2);
                        break;
                    case 3:
                        if (_d == null) _d = 0;
                        _d += decimal.Round(result, 2);
                        break;
                }

                return result;
            }
            catch (Exception)
            {
                return 0;
            }            
        }

        private decimal? GetYearsOfAntiguaty(Employees e)
        {
            int antiguaty = DateTime.Today.Year - e.entry_date.Year;
            if ((DateTime.Today.Month < e.entry_date.Month) || (DateTime.Today.Month == e.entry_date.Month && DateTime.Today.Day < e.entry_date.Day))
            {
                antiguaty--;
            }

            return antiguaty;
        }

        private decimal? GetBestGrossSalary(Employees e, DateTime period)
        {
            return LiquidationFixedDataB.SelectBestGrossSalary(e.file_n, period);
        }

        private decimal? GetSemesterDays(DateTime period)
        {
            if (period.Month <= 6)
            {
                DateTime _1_1 = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime _6_30 = new DateTime(DateTime.Now.Year, 6, 30);

                return _6_30.Subtract(_1_1).Days;
            }
            else
            {
                DateTime _7_1 = new DateTime(DateTime.Now.Year, 7, 1);
                DateTime _12_31 = new DateTime(DateTime.Now.Year, 12, 31);

                return _12_31.Subtract(_7_1).Days;
            }
        }

        private decimal? GetPastMonthGrossSalary(Employees e, DateTime period)
        {
            return LiquidationFixedDataB.SelectPastMonthGrossSalary(e.file_n, period) ?? 0;
        }

        private int? GetHolidaysP(Employees e, DateTime period)
        {
            return CalendarB.SelectVacationsDays(e.file_n, period);
        }

        private int? GetHolidaysF(Employees e, DateTime period)
        {
            return LiquidationFixedDataB.SelectVacationsDays(e.file_n, period);
        }

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing) handle.Dispose();

            disposed = true;
        }
    }
}
