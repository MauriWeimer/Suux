using Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayout.Helper
{
    public class GetConnectionString
    {
        public static string GetConnection()
        {
            return new SuuxEntities().Database.Connection.ConnectionString;
        }
    }
}
