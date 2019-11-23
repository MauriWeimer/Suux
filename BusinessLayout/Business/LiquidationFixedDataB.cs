using Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BusinessLayout.Business
{
    public class LiquidationFixedDataB
    {
        #region CRUD
        public static void InsertLiquidationFixedData(Liquidation_fixed_datas liquidationFixedData)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Liquidation_fixed_datas.Add(liquidationFixedData);
                ent.SaveChanges();
            }
        }

        public static void UpdateLiquidationFixedData(Liquidation_fixed_datas liquidationFixedData)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Liquidation_fixed_datas.Attach(liquidationFixedData);
                ent.Entry(liquidationFixedData).State = EntityState.Modified;
                ent.SaveChanges();
            }
        }

        public static void DeleteLiquidationFixedData(int liquidationFixedDataId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Liquidation_fixed_datas.Remove(ent.Liquidation_fixed_datas.Find(liquidationFixedDataId));
                ent.SaveChanges();
            }
        }
        #endregion

        #region ADD
        public static List<Liquidation_fixed_datas> SelectLiquidationFixedDatasIncludeTypesAndEmployeesLiquidated()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Liquidation_fixed_datas
                    .Take(25)
                    .Include(lfd => lfd.Liquidation_types)
                    .Include(lfd => lfd.Employees_liquidated)
                    .ToList();
            }
        }

        public static List<Liquidation_fixed_datas> SelectLiquidationFixedDatasIncludeAll()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Liquidation_fixed_datas
                    .Take(25)
                    .Include(lfd => lfd.Employees_liquidated)
                    .Include(lfd => lfd.Employees_liquidated.Select(el => el.Employees))
                    .Include(lfd => lfd.Liquidation_types)
                    .OrderByDescending(lfd => lfd.period)
                    .ThenByDescending(lfd => lfd.liquidation_fixed_data_id)
                    .ToList();
            }
        }

        public static decimal SelectBestGrossSalary(int fileN, DateTime period)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Liquidation_fixed_datas
                    .OrderByDescending(lfd => lfd.period)
                    .Take(6)
                    .Where(lfd => lfd.period <= period
                    && lfd.liquidation_type_id == 1)
                    .Select(lfd => lfd.Employees_liquidated.Where(el => el.file_n == fileN).Select(elx => elx.gross_salary).FirstOrDefault())
                    .Max(); 
            }
        }

        public static decimal? SelectPastMonthGrossSalary(int fileN, DateTime period)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                DateTime searchPeriod = period.AddMonths(-1);
                return ent.Liquidation_fixed_datas
                    .OrderByDescending(lfd => lfd.period)
                    .SingleOrDefault(lfd => lfd.period == searchPeriod
                    && lfd.liquidation_type_id == 1
                    && lfd.Employees_liquidated.Select(el => el.file_n).Contains(fileN))
                    ?.Employees_liquidated.Select(el => el.gross_salary).First();
            }
        }

        public static int SelectVacationsDays(int fileN, DateTime period)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                int? employeeLiquidatedId = ent.Liquidation_fixed_datas
                    .SingleOrDefault(lfd => lfd.period == period
                    && lfd.liquidation_type_id == 3
                    && lfd.Employees_liquidated.Select(el => el.file_n).Contains(fileN))
                    ?.Employees_liquidated.Select(el => el.employee_liquidated_id).First();

                if (employeeLiquidatedId == null) return 0;

                return int.Parse(ent.Employees_liquidated_concepts
                    .Single(elc => elc.employee_liquidated_id == employeeLiquidatedId &&
                    elc.sorted_concept_id == 0)
                    .quantity.Substring(0, 2));
            }
        }
        #endregion
    }
}
