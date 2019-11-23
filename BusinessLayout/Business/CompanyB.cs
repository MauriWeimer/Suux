using Data.Context;
using System.Linq;

namespace BusinessLayout.Business
{
    public class CompanyB
    {
        public static Companys SelectCompany()
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                return ent.Companys.First();
            }
        }

        public static void UpdateCompany(string company, string street, int streetN, int provinceId, string city, int postalCode, long phoneN, long cuitN)
        {
            using (SuuxEntities ent = new SuuxEntities())
            {
                Companys c = ent.Companys.First();
                c.company = company.ToUpper();
                c.street = street.ToUpper();
                c.street_n = streetN;
                c.province_id = provinceId;
                c.city = city.ToUpper();
                c.postal_code = postalCode;
                c.phone_n = phoneN;
                c.cuit_n = cuitN;
                ent.SaveChanges();
            }
        }

    }
}
