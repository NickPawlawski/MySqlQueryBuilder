using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlQueryBuilder.QueryClasses
{
    static class Delete
    {

        public static string CreateQuery(string tableName,string condition, string value)
        {
            string query = "";

            query += "DELETE FROM " + tableName + " WHERE "+ condition + " = \"" + value+"\"";

            return query;
        }


    }
}
