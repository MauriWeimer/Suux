using Data.Context;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace BusinessLayout.Business
{
    public class EmployeeB
    {
        #region CRUD
        public static List<Employees> SelectEmployees()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Employees.ToList();
            }
        }

        public static void InsertEmployee(Employees employee)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                employee.Concepts = new ObservableCollection<Concepts>(ent.Concepts.Where(c => c.common));
                ent.Employees.Add(employee);
                ent.SaveChanges();
            }
        }

        public static void UpdateEmployee(Employees employee)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Employees.Attach(employee);
                ent.Entry(employee).State = EntityState.Modified;
                ent.SaveChanges();
            }
        }
        #endregion

        #region ADD
        public static List<Employees> SelectEmployeesIncludeAll()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Employees.Where(e => e.state)
                    .Include(e => e.Categorys)
                    .Include(e => e.Categorys)
                    .Include(e => e.Social_works)
                    .Include(e => e.Labor_unions)
                    .Include(e => e.Concepts)
                    .Include(e => e.Concepts.Select(c => c.Formulas))
                    .ToList();
            }
        }

        public static List<Employees> SelectEmployeesIncludeConcepts()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Employees
                    .Where(e => e.state)
                    .Include(e => e.Concepts)
                    .ToList();
            }
        }        

        public static void UpdateIndividualConcepts(Employees employee)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                List<Concepts> allConcepts = ent.Concepts
                    .Include(c => c.Employees)
                    .Where(c => c.common == false)
                    .ToList();
                List<Concepts> oldConcepts = allConcepts
                    .Where(c => c.Employees.Select(e => e.file_n).Contains(employee.file_n))
                    .ToList();

                List<Concepts> addedConcepts = new List<Concepts>();
                allConcepts.Where(c => !oldConcepts.Select(cx => cx.concept_id).Contains(c.concept_id) &&
                employee.Concepts.Select(cx => cx.concept_id).Contains(c.concept_id))
                    .ToList()
                    .ForEach(c => addedConcepts.Add(c));

                List<Concepts> deletedConcepts = oldConcepts.Where(c => !employee.Concepts.Select(cx => cx.concept_id).Contains(c.concept_id)).ToList();

                Employees employeeDB = ent.Employees
                    .Include(e => e.Concepts)
                    .Single(e => e.file_n == employee.file_n);
                deletedConcepts.ForEach(c => employeeDB.Concepts.Remove(c));
                addedConcepts.ForEach(c => employeeDB.Concepts.Add(c));
                ent.SaveChanges();
            }
        }

        public static List<Employees> SelectEmployeesIncludeFixedSchedules()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return new List<Employees>(ent.Employees
                    .Where(e => e.state)
                    .Include(e => e.Fixed_schedules)
                    .OrderBy(e => e.fixed_schedule_id == null)
                    .ThenBy(e => e.file_n));
            }
        }
        #endregion
    }
}
