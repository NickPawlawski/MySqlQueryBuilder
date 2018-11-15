using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlQueryBuilder
{
    class ParentBuilder
    {
        private List<ChildBuilder> _children = new List<ChildBuilder>();
        public enum Statements { Select, Delete, Update, Insert};
        private string _error;
        private int _status;

        public List<ChildBuilder> Children => _children;

        public ParentBuilder()
        {
            
        }

        public void AddChild(ChildBuilder child)
        {
            _children.Add(child);
        }

        public void RemoveChild(ChildBuilder child)
        {
            _children.Remove(child);
        }

        public void Run()
        {
            DatabaseConnection.Run(this);
        }
    }
}
