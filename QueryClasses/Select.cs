using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlQueryBuilder.QueryClasses
{
    static class Select
    {
        public static string CreateQuery(string tableName, Dictionary<string, List<string>> columnNames = null)
        //public static string CreateQuery(string tableName)
        {
            string query;

            if (columnNames == null)
            {
                query = "SELECT * FROM " + tableName;
            }
            else
            {
                string[] keys = columnNames.Keys.ToArray();

                query = "SELECT ";

                // Add in the variables to select, and FROM at the end
                for (int i =  0; i < columnNames.Count; i++)
                {
                    query += keys[i];

                    if(i < columnNames.Count-1)
                    {
                        query += ",";
                    }
                }

                query += " FROM " + tableName;
                
            }


            return query;
        }

    }
}
