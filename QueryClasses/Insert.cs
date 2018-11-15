using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlQueryBuilder.QueryClasses
{
    class Insert
    {
        public static string CreateQuery(string tableName, Dictionary<string, List<string>> columnNames = null)
        //public static string CreateQuery(string tableName)
        {
            string query;

            query = "insert into " + tableName + "(";

            string[] keys = columnNames.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                query += keys.ElementAt(i);

                if (i != keys.Length - 1)
                    query += ",";
            }

            query += ") values(";

            for (int i = 0; i < keys.Length; i++)
            {
                query += "@" + keys.ElementAt(i);

                if (i != keys.Length - 1)
                    query += ",";

                //_command.Parameters.AddWithValue("@" + data.Keys.ElementAt(i), data.Values.ElementAt(i));
            }
            query += ")";

            

            return query;
        }
    }
}
