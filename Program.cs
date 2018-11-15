using CAEMon;
using System;
using System.Collections.Generic;
using System.Linq;



namespace MySqlQueryBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var cr = ConfigReader.GetConfigReader();

            ParentBuilder _pb = new ParentBuilder();

            Dictionary<string, List<string>> dict = DictionaryBuilder.BuildDictionary(new List<string>(){"CategoryID","CategoryName","Picture"});
            _pb.AddChild(new ChildBuilder("categories", dict, ParentBuilder.Statements.Select));

            Dictionary<string, List<string>> dict2 = DictionaryBuilder.BuildDictionary(new List<string>() {"ID"});
            dict2.Values.ElementAt(0).Add("1");
            _pb.AddChild(new ChildBuilder("order_details", dict2, ParentBuilder.Statements.Delete));

            Dictionary<string, List<string>> dict3 = DictionaryBuilder.BuildDictionary(new List<string>() { "Quantity"});
            dict3.Values.ElementAt(0).Add("15");
            //dict3.Values.ElementAt(1).Add("OrderID = 10248");
            _pb.AddChild(new ChildBuilder("order_details", dict3, ParentBuilder.Statements.Update,"UA"));

            Dictionary<string, List<string>> dict4 = DictionaryBuilder.BuildDictionary(new List<string>() { "CategoryName", "Picture", "Description" });
            dict4.Values.ElementAt(0).Add("Test123");
            dict4.Values.ElementAt(1).Add("Hello.jpg");
            dict4.Values.ElementAt(2).Add("Hahahaha");

            _pb.AddChild(new ChildBuilder("categories", dict4, ParentBuilder.Statements.Insert));

            _pb.Run();

            Console.WriteLine("EVERYTHING WORKED");
            Console.ReadLine();
        }
    }
}
