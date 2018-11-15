using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlQueryBuilder
{
    static class DictionaryBuilder
    {

        public static Dictionary<string, List<string>> BuildDictionary(List<string> columnNames)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            for(int i = 0; i < columnNames.Count; i++)
            {
                dict.Add(columnNames[i], new List<string>());
            }

            return dict;   
        }
    }
}
