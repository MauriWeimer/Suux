using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class EmployeeLiquidatedB
    {
        public static void InsertEmployeeLiquidated(Employees_liquidated employeesLiquidated)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                if (ent.Employees_liquidated
                    .Any(el => el.file_n == employeesLiquidated.file_n && el.liquidation_fixed_data_id == employeesLiquidated.liquidation_fixed_data_id))
                    ent.Employees_liquidated
                        .Remove(ent.Employees_liquidated
                        .Single(el => el.file_n == employeesLiquidated.file_n && el.liquidation_fixed_data_id == employeesLiquidated.liquidation_fixed_data_id));

                ent.Employees_liquidated.Add(employeesLiquidated);
                ent.SaveChanges();                
            }
        }

        public static void UpdateEmployeeLiquidated(List<int> employeesLiquidatedId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                foreach (int employeeLiquidatedId in employeesLiquidatedId)
                {
                    ent.Employees_liquidated.Find(employeeLiquidatedId).issue = true;
                }

                ent.SaveChanges();
            }
        }
    }
}
