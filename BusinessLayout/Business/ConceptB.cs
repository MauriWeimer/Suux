using Data.Context;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BusinessLayout.Business
{
    public class ConceptB
    {
        #region CRUD
        public static List<Concepts> SelectConcepts()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Concepts.ToList();
            }
        }

        public static void InsertConcept(Concepts concept)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Concepts.Add(concept);
                if (concept.common)
                {
                    InsertOrDeleteConceptCommon(concept, concept.common, ent);
                }
                else
                {
                    ent.SaveChanges();
                }                
            }
        }

        public static void UpdateConcept(Concepts concept, bool common)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Concepts.Attach(concept);
                ent.Entry(concept).State = EntityState.Modified;
                if (common != concept.common)
                {
                    InsertOrDeleteConceptCommon(concept, concept.common, ent);
                }
                else
                {
                    ent.SaveChanges();
                }                
            }
        }

        public static void DeleteConcept(int conceptId)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                ent.Concepts.Remove(ent.Concepts.Find(conceptId));
                ent.SaveChanges();
            }
        }
        #endregion

        #region ADD
        private static void InsertOrDeleteConceptCommon(Concepts concept, bool common, SuuxEntities ent)
        {
            using (ent)
            {
                if (common)
                {
                    ent.Employees.ToList().ForEach(e => e.Concepts.Add(concept));
                }
                else
                {
                    ent.Employees.ToList().ForEach(e => e.Concepts.Remove(concept));
                }
                ent.SaveChanges();
            }            
        }

        public static List<Concepts> SelectConceptsNotCommonsIncludeAll()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Concepts.Where(c => !c.common)
                    .Include(c => c.Concept_types)
                    .Include(c => c.Formulas)
                    .ToList();
            }
        }

        public static List<Concepts> SelectConceptsRAndNRIncludeAll()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Concepts.Where(c => !c.common)
                    .Include(c => c.Concept_types)
                    .Include(c => c.Formulas)
                    .ToList();
            }
        }

        public static List<Concepts> SelectConceptsIncludeTypes()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Concepts.Include(c => c.Concept_types).ToList();
            }
        }

        public static int? SelectAvailableId(int since, int until)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                List<Concepts> concepts = ent.Concepts
                    .OrderBy(c => c.sorted_concept_id)
                    .Where(c => c.sorted_concept_id > since && c.sorted_concept_id < until).ToList();

                if (concepts.Count == 0) return since + 1;

                int previousId = 0;

                foreach (Concepts concept in concepts)
                {
                    if (previousId == 0)
                    {
                        if (since + 1 < concept.sorted_concept_id)
                        {
                            return since + 1;
                        }

                        previousId = concept.sorted_concept_id;
                    }
                    else
                    {
                        if (previousId == concept.sorted_concept_id - 1)
                        {
                            previousId = concept.sorted_concept_id;
                        }
                        else
                        {
                            return previousId + 1;
                        }
                    }
                }

                return concepts.Last().sorted_concept_id + 1 == until ? (int?)null : concepts.Last().sorted_concept_id + 1;               
            }
        }
    }
    #endregion
}
