using CAEMon;
using CAEMon.CaeMonResources;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlQueryBuilder.QueryClasses;

namespace MySqlQueryBuilder
{
    static class DatabaseConnection
    {

        private const string SqlServer = @"server=localhost;userid=root;password='';database=northwind";
        private static MySqlCommand _command;
        private static MySqlConnection _connection;
        private static MySqlTransaction _trans;
        private static ParentBuilder _parent;


        public static ParentBuilder Run(ParentBuilder parent)
        {
            _parent = parent;

            StartDbConnection();
            StartTransaction();

            for (int i = 0; i < parent.Children.Count; i++)
            {
                Fresh();
                ParseCommand(parent.Children[i]);
                Execute();
            }


            EndTransaction();

            EndDbConnection();

            return _parent;
        }

        private static void ParseCommand(ChildBuilder child)
        {
            switch(child.KeyParam)
            {
                case ParentBuilder.Statements.Select:
                    if (child.OptionalParam.Contains("S"))
                    {
                        child.SetCommand(Select.CreateQuery(child.Table));

                    }
                    else
                    {
                        child.SetCommand(Select.CreateQuery(child.Table, child.ReturnValue));

                        string[] keys = child.ReturnValue.Keys.ToArray();              
                    }

                    _command.CommandText = child.Command;
                    _command.Prepare();

                    var reader = _command.ExecuteReader();

                    var index = 0;

                    var temp = child.ReturnValue;
                    //read each line returned
                    while (reader.Read())
                    {
                        //new dictionary to hold all of the records
                        //dictList.Add(new Dictionary<string, string>());

                        try
                        {
                            var i = 0;
                            //read each line and add it to the newest dictionary in the list
                            while (i < child.ReturnValue.Keys.Count)
                            {
                                var read = reader.GetValue(i).ToString();
                                //Console.WriteLine(read);
                                // dictList.ElementAt(index).Add(dict.Keys.ElementAt(i), read);
                                temp.Values.ElementAt(i).Add(read);

                                i++;
                            }

                        }
                        catch (Exception c)
                        {
                            Reporter.WriteContent("Exception thrown in RetriveTable: " + c, 0);
                        }

                        index++;

                    }
                    reader.Close();

                    child.SetReturnValue(temp);

                    if (child.OptionalParam.Contains("P"))
                    {
                        
                            for (int c = 0; c < child.ReturnValue.Values.ElementAt(0).Count; c++)
                            {
                                Console.WriteLine("Record "+ c + ":");
                                for (int i = 0; i < child.ReturnValue.Keys.Count; i++)
                                {
                                    Console.WriteLine("-"+child.ReturnValue.Keys.ElementAt(i) + ": \t" + child.ReturnValue.Values.ElementAt(i).ElementAt(c));
                                }

                                Console.WriteLine();    
                            }

                        
                    }

                    break;
                case ParentBuilder.Statements.Insert:

                    for(int i = 0; i < child.ReturnValue.Values.ElementAt(0).Count; i++)
                    {
                        Fresh();

                        _command.CommandText = Insert.CreateQuery(child.Table, child.ReturnValue);

                        PrepareCommand(child, i);

                        Execute();
                    }

                    Fresh();

                    break;


                case ParentBuilder.Statements.Update:
                    string tempCommand = Update.CreateQuery(child.Table, child.ReturnValue);

                    if (!child.OptionalParam.Contains("UA"))
                    {
                        try
                        {
                            List<string> optionalParam = new List<string>();
                            child.ReturnValue.TryGetValue("WHERE", out optionalParam);

                            tempCommand += "WHERE " + optionalParam.ElementAt(0);

                            _command.CommandText = tempCommand;
                        }
                        catch (Exception ex)
                        {
                            Rollback();

                            Reporter.WriteContent("Exeption thrown in DB Connection Delete: " + ex, 0);
                        }
                    }
                    

                   

                    

                    break;
                case ParentBuilder.Statements.Delete:

                    for(int i = 0; i < child.ReturnValue.Count;i++)
                    {
                        Fresh();
                        
                        _command.CommandText = Delete.CreateQuery(child.Table,child.ReturnValue.Keys.ElementAt(i),child.ReturnValue.Values.ElementAt(i).ElementAt(0));

                        Execute();
                    }

                    Fresh();

                    break;
            }
            
        }

        private static void Rollback()
        {
            try
            {
                _trans.Rollback();
            }
            catch(Exception ex)
            {
                Reporter.WriteContent("General error: " + ex, 0);
            }
            
        }

        private static void PrepareCommand(ChildBuilder child,int index)
        {
            for (int i = 0; i < child.ReturnValue.Keys.Count; i++)
            {
                _command.Parameters.AddWithValue("@" + child.ReturnValue.Keys.ElementAt(i), child.ReturnValue.Values.ElementAt(i).ElementAt(index));
            }
        }

        private static void Execute()
        {

            if(_command.CommandText != null)
            {
                try
                {
                   _command.ExecuteNonQuery();
                }
                catch(MySqlException ex)
                {
                    Rollback();
                    Console.WriteLine(ex);
                }
            }
           
        }

        private static void Fresh()
        {
            _command.CommandText = null;
        }

        private static void StartTransaction()
        {
            _trans = _connection.BeginTransaction();

            _command.Transaction = _trans;
        }

        private static void EndTransaction()
        {
            _trans.Commit();
            _trans.Dispose();
        }

        /// <summary>
        /// Creates initial connection to db.
        /// </summary>
        private static void StartDbConnection()
        {
            try
            {
                _connection = new MySqlConnection(SqlServer);
                _connection.Open();

                _command = new MySqlCommand
                {
                    Connection = _connection,
                    
                };


            }
            catch (Exception e)
            {
                Reporter.WriteContent("Error starting Connection to database: " + e, 2);
            }
        }

        /// <summary>
        /// Closes connection to db.
        /// </summary>
        private static void EndDbConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception e)
            {
                Reporter.WriteContent("Error closing connection to database: " + e, 2);
            }
        }







        /*

        #region Variables

        private const string SqlServer = @"server=dolby.ceas.wmich.edu;userid=caemon;password=defeatedbadger;database=caemonweb";
        private MySqlCommand _command;
        private MySqlConnection _connection;

        #endregion Variables



        #region Constructors


        public DatabaseConnection(Dictionary<string, string> data, string tableName)
        {
            StartDbConnection();
            SendMessage(data, tableName);
            EndDbConnection();
        }


        public DatabaseConnection(string tableName, Dictionary<string, string> dict, out List<Dictionary<string, string>> dictList, string options = "")
        {
            StartDbConnection();
            RetriveTable(tableName, dict, out dictList, options);
            EndDbConnection();
        }


        public DatabaseConnection(string tableName, int id)
        {
            StartDbConnection();
            DeleteRecord(tableName, id);
            EndDbConnection();
        }

        #endregion Constructors



        #region Methods

        /// <summary>
        /// Creates initial connection to db.
        /// </summary>
        private void StartDbConnection()
        {
            try
            {
                _connection = new MySqlConnection(SqlServer);
                _connection.Open();
            }
            catch (Exception e)
            {
                Reporter.WriteContent("Error starting Connection to database: " + e, 2);
            }
        }


        /// <summary>
        /// Sends sql command to db.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        private void SendMessage(Dictionary<string, string> data, string tableName)
        {
            try
            {
                _command = new MySqlCommand
                {
                    Connection = _connection,
                    CommandText = "insert into " + tableName + "("
                };

                for (int i = 0; i < data.Count; i++)
                {
                    _command.CommandText += data.Keys.ElementAt(i);

                    if (i != data.Count - 1)
                        _command.CommandText += ",";
                }

                _command.CommandText += ") values(";

                for (int i = 0; i < data.Count; i++)
                {
                    _command.CommandText += "@" + data.Keys.ElementAt(i);

                    if (i != data.Count - 1)
                        _command.CommandText += ",";

                    _command.Parameters.AddWithValue("@" + data.Keys.ElementAt(i), data.Values.ElementAt(i));
                }
                _command.CommandText += ")";

                if (SoftwareConfiguration.Debug)
                {
                    Reporter.WriteContent("Command text: " + _command.CommandText, 0);
                }

                //Console.WriteLine(_command.CommandText);

                _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Reporter.WriteContent("Exception thrown: " + e, 0);
            }
        }


        /// <summary>
        /// Grabs table of records from database.
        /// </summary>
        /// <param name="tableName">name of the table</param>
        /// <param name="dict">dictionary that the read will be written to</param>
        /// <param name="dictList">the dictionary that will be returned</param>
        /// <param name="options">any additional options that will be added to the sql query</param>
        private void RetriveTable(string tableName, Dictionary<string, string> dict, out List<Dictionary<string, string>> dictList, string options = " ")
        {
            dictList = new List<Dictionary<string, string>>();
            _command = new MySqlCommand
            {
                Connection = _connection,
                CommandText = "select * from " + tableName + " " + options
            };

            try
            {

                var reader = _command.ExecuteReader();

                var index = 0;

                //read each line returned
                while (reader.Read())
                {
                    //new dictionary to hold all of the records
                    dictList.Add(new Dictionary<string, string>());

                    try
                    {
                        var i = 0;
                        //read each line and add it to the newest dictionary in the list
                        while (i < dict.Count)
                        {
                            var read = reader.GetValue(i).ToString();
                            //Console.WriteLine(read);
                            dictList.ElementAt(index).Add(dict.Keys.ElementAt(i), read);
                            i++;
                        }

                    }
                    catch (Exception c)
                    {
                        Reporter.WriteContent("Exception thrown in RetriveTable: " + c, 0);
                    }

                    index++;

                }
            }
            catch (Exception e)
            {
                Reporter.WriteContent("Exception thrown in RetriveTable: " + e, 0);
            }
        }


        /// <summary>
        /// Removes record from db.
        /// </summary>
        /// <param name="tableName">Table name the record is on</param>
        /// <param name="id">Id of the record to be removed</param>
        private void DeleteRecord(string tableName, int id)
        {
            try
            {
                _command = new MySqlCommand
                {
                    Connection = _connection,
                    CommandText = "delete from " + tableName + " where " + tableName + ".ID = " + id
                };
                _command.Prepare();

                _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Reporter.WriteContent("Exception Thrown in DeleteRecord: " + e, 0);
            }
        }


        /// <summary>
        /// Closes connection to db.
        /// </summary>
        private void EndDbConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception e)
            {
                Reporter.WriteContent("Error closing connection to database: " + e, 2);
            }
        }

        #endregion Methods
        */
    }
}
