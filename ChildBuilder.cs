using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MySqlQueryBuilder
{
    class ChildBuilder
    {
        private string _command;
        private string _table;
        private Dictionary<string, List<string>> _returnValues;
        private ParentBuilder.Statements _keyParam;
        private List<string> _optionalParam = new List<string>();

        private string[] AllowedOptionalParam =  {"S", "P", "UA"};


        // Command string from user
        public string Command => _command;

        public void SetCommand(string command)
        {
            _command = command;
        }

        public string Table => _table;

        public Dictionary<string, List<string>> ReturnValue => _returnValues;

        public void SetReturnValue(Dictionary<string, List<string>> returnVal)
        {
            _returnValues = returnVal;
        }

        // Paramter (select, delete, insert)
        public ParentBuilder.Statements KeyParam => _keyParam;

        public List<string> OptionalParam => _optionalParam;

        // Default constructor
        public ChildBuilder(string table, Dictionary<string, List<string>> returnValues, ParentBuilder.Statements keyParam, string optionalParam = "")
        {
            //_command = command;
            _table = table;
            _returnValues = returnValues;
            _keyParam = keyParam;

            // Validate optional param
            string[] split = optionalParam.Split(' ');

            // Check if the param is in the allowed optional params
            foreach(var param in split)
            {
                if (AllowedOptionalParam.Contains(param))
                {
                    _optionalParam.Add(param);
                }
            }
        }
    }
}
