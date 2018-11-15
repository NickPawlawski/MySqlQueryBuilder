using MySqlQueryBuilder;
using System;
using System.Collections.Generic;
using System.IO;


namespace CAEMon.CaeMonResources
{
    /// <summary>
    /// Builds and writes reports of program antics.
    /// </summary>
    public static class Reporter
    {

        #region Variables

        private const string Filename = "Report.txt";
        private static readonly string Folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\CaeMon";
        private static readonly Stack<string> Section = new Stack<string>();

        private static string PreText = "";
        #endregion Variables


        #region Properties

        public static List<string> FrontEndReport { get; set; }

        public static List<List<string>> FrontEndReports => new List<List<string>>();

        #endregion Properties


        #region Methods

        /// <summary>
        /// Program start handling.
        /// </summary>
        public static void Startup()
        {
            // Ensure that directory exists.
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }

            // Attempt first write to file.
            try
            {
                StreamWriter streamWriter;
                using (streamWriter = File.AppendText(Folder + "\\" + Filename))
                {
                    streamWriter.WriteLine("*****************************************");
                    streamWriter.WriteLine("Software Launched at: " + DateTime.Now);
                    streamWriter.WriteLine(" ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }
        }


        public static void StartSection(string sectionName)
        {
            Section.Push(sectionName);

            FrontEndReport = new List<string> {DateTime.Now + " Started Section: " + sectionName};

            try
            {
                StreamWriter streamWriter;
                using (streamWriter = File.AppendText(Folder + "\\" + Filename))
                {
                    //Console.WriteLine(@"Error in reporter" + sectionName);
                    streamWriter.WriteLine("------------- Start: " + sectionName + " -------------");
                    streamWriter.WriteLine(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }
        }


        public static void EndSection()
        {
            var sectionName = Section.Pop();

            FrontEndReport.Add("-----------------------");
            FrontEndReports.Add(FrontEndReport);

            try
            {
                StreamWriter streamWriter;
                using (streamWriter = File.AppendText(Folder + "\\" + Filename))
                {
                    //Console.WriteLine(@"Error in reporter" + sectionName);
                    streamWriter.WriteLine("------------- End: " + sectionName + " -------------");
                    streamWriter.WriteLine(" ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }
        }


        /// <summary>
        /// Writes data to indicated location.
        /// </summary>
        /// <param name="content">Content to write.</param>
        /// <param name="location">???</param>
        public static void WriteContent(string content, int location)
        {
            if (Section.Count == 0)
            {
                PreText = "&&&&&    ";
            }
            try
            {
                StreamWriter streamWriter;
                switch (location)
                {
                    case 0:
                        using (streamWriter = File.AppendText(Folder + "\\" + Filename))
                        {
                            streamWriter.WriteLine(PreText + DateTime.Now + "    " + content);
                        }
                        break;

                    case 1:
                        FrontEndReport.Add("     " + content);
                        break;

                    case 2:
                        using (streamWriter = File.AppendText(Folder + "\\" + Filename))
                        {
                            
                        }
                        break;

                    case 3:
                        var dict = new Dictionary<string, string>();

                        var dateValue = DateTime.Now;
                        var mySqlFormatDate = dateValue.ToString("yyyy-MM-dd HH:mm:ss");

                        var split = content.Split(',');

                        dict.Add("pc_name", split[0]);
                        dict.Add("message", split[1]);
                        dict.Add("notes", split[2]);
                        dict.Add("created_at", mySqlFormatDate);
                        dict.Add("updated_at", mySqlFormatDate);

                        // ReSharper disable once UnusedVariable
                        //var dbc = new DatabaseConnection(dict, "error_log");
                        break;

                    case 4:
                        if (SoftwareConfiguration.Debug)
                        {
                            using (streamWriter = File.AppendText(Folder + "\\" + Filename))
                            {
                                streamWriter.WriteLine(PreText + DateTime.Now + "    " + content);
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error in reporter" + ex);
            }

            PreText = "";
        }


        public static void ClearList()
        {
            FrontEndReport?.Clear();
        }

        #endregion Methods
    }
}
 