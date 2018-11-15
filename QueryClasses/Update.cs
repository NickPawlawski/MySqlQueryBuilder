using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlQueryBuilder.QueryClasses
{
    class Update
    {
        public static string CreateQuery(string tableName, Dictionary<string, List<string>> columnNames = null)
        //public static string CreateQuery(string tableName)
        {
            string query;

            query = "UPDATE " + tableName + " SET ";

            
            string[] keys = columnNames.Keys.ToArray();

            // Add in the variables to select, and FROM at the end
            for (int i = 0; i < columnNames.Count; i++)
            {
                if (!(keys[i].ToUpper().CompareTo("WHERE") == 0))
                {
                    query += keys[i];

                    query += " = " + columnNames.Values.ElementAt(i).ElementAt(0) + " "; // Look at values, get first element of list from whatever the current key is

                    if ((i < columnNames.Count - 2 && columnNames.ContainsKey("WHERE")) || i < columnNames.Count - 1 && !columnNames.ContainsKey("WHERE"))
                    {
                        query += ",";
                    }
                }

                
            }

            

            


            return query;
        }
    }
}
