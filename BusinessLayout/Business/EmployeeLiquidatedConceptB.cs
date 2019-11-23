using Data.Context;
using System.Collections.Generic;

namespace BusinessLayout.Business
{
    public class EmployeeLiquidatedConceptB
    {
        public static void InsertEmployeeLiquidatedConcepts(int employeesLiquidatedId, List<Concepts> concepts)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                foreach (Concepts c in concepts)
                {
                    Employees_liquidated_concepts employeesLiquidatedConcepts = new Employees_liquidated_concepts()
                    {
                        employee_liquidated_id = employeesLiquidatedId,
                        sorted_concept_id = c.sorted_concept_id,
                        concept = c.concept,
                        quantity = c.quantityn + " " + c.Formulas.quantity_leyend,
                        rem = c.r,
                        no_rem = c.nr,
                        ded = c.d
                    };

                    ent.Employees_liquidated_concepts.Add(employeesLiquidatedConcepts);
                }
                ent.SaveChanges();
            }
        }
    }
}
