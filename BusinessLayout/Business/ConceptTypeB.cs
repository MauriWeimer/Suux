using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayout.Business
{
    public class ConceptTypeB
    {
        public static List<Concept_types> SelectConceptTypes()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Concept_types.ToList();
            }
        }
    }
}
