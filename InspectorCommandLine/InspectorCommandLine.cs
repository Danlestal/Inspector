using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using EA.Eism.DotNetUtils;
using EA.Eism.DotNetUtils.Types;
using System.Threading;


using AudioScriptInspector.Core;

namespace EA.Eism.Raccoon.CommandLine
{
    /// <summary>
    /// This class implements the entry point for the Raccoon command-line tool.
    /// </summary>
    [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
    public class RaccoonCommandLine
    {
        #region Private constants.
        /// <summary>
        /// Name of the parameter for the path to an optional log file.
        /// </summary>
        private const string ConfigFile = "Config";
        /// <summary>
        /// Name of the parameter for the path to an optional log file of strings.
        /// </summary>
        private const string ReportFile = "Report";
        #endregion
        #region Entry point.
        /// <summary>
        /// The entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            if(args.Count() != 2)
            {
                Console.WriteLine();
                Console.WriteLine("ERROR: use the next format");
                Console.WriteLine("Inspector.exe [config] [outputfile]");
                Environment.Exit(1);
            }
            //Preparing Raccoon
            if (!Directory.Exists(System.Environment.CurrentDirectory + @"\Lib"))
            {
                // No need for a stack trace in the case of command-line exceptions.
                Console.WriteLine();
                Console.WriteLine("ERROR: Lib folder not found");
                Environment.Exit(1);
            }
            RaccoonFactory.GetInstance().AddAssemblyFolder(System.Environment.CurrentDirectory + @"\Lib");
            try
            {
                // Parse the command line...
                var clp = GetCommandLineParser();
                clp.AddParameters(args);
                string configFile = clp.ExtractParameter(ConfigFile);
                if(!File.Exists(configFile))
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: {0} does not exists", configFile);
                    Environment.Exit(1);
                }
                string outPutFile = clp.ExtractParameter(ReportFile);
                if (outPutFile == string.Empty)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: {0} does not exists", outPutFile);
                    Environment.Exit(1);
                }
                InspectorConfig configInfo = InspectorConfig.DeserializeInspectorConfig(configFile);
                //Inspector initialization.
                //script
                //Inspector importation. True because the importation manager has to be synchronous
                ImportationManager manager = new ImportationManager(true);
                var scriptParameters = new Dictionary<string, object>() { { "TranslationTextColumnName", configInfo.ScriptConfig.ScriptIDColumn }, { "StringIdColumnName", configInfo.ScriptConfig.ScriptIDColumn }, { "PlatformIndependentColumnList", configInfo.ScriptConfig.ScriptColumns.Except(new string[] { configInfo.ScriptConfig.ScriptIDColumn }).ToArray() } };
                DataRetriever scriptRetriever = DataRetrieverCreator.GenerateDataRetriever(DataRetrieverCreator.DataRetrieverFormats.RaccoonExcel, scriptParameters, configInfo.ScriptConfig.ScriptFileName, configInfo.ScriptConfig.Category, configInfo.CaseSensitive);
                manager.AddRetriever(scriptRetriever);
                //phys
                List<DataRetriever> filesRetrieverList = new List<DataRetriever>();
                foreach (FilesConfig filesConfigItem in configInfo.FilesConfigList)
                {
                    var physParameters = new Dictionary<string, object>() { { "SourceDir", filesConfigItem.SourceBaseFolder }, { "Includes", filesConfigItem.FilesExtension }, { "WhoIsID", "filename" } };
                    DataRetriever physRetriever = DataRetrieverCreator.GenerateDataRetriever(DataRetrieverCreator.DataRetrieverFormats.RaccoonDirectories, physParameters, string.Empty, string.Empty, configInfo.CaseSensitive);
                    filesRetrieverList.Add(physRetriever);
                    manager.AddRetriever(physRetriever);
                }
                manager.Import();
                //Merge.
                InspectorMerger merger = new InspectorMerger(scriptRetriever.DataRetrieved, "SCRIPT", filesRetrieverList.SelectMany(s => s.DataRetrieved), "FILES");
                System.Data.DataSet resultDataSet = merger.MergeCollections().ToDataSet();
                //OutputResults
                CreateExcelFile.CreateExcelDocument(resultDataSet, outPutFile);

            }
            catch (CommandLineException e)
            {
                // No need for a stack trace in the case of command-line exceptions.
                Console.WriteLine();
                Console.WriteLine("ERROR: {0}", e.Message);
                Environment.Exit(1);
            }
            
        }


        #endregion

        #region Private static helpers.

        /// <summary>
        /// Initializes a command-line parser.
        /// </summary>
        /// <returns>A command-line parser.</returns>
        private static CommandLineParameters GetCommandLineParser()
        {
            var clp = new CommandLineParameters();
            clp.ParameterConfig.Add(new Dictionary<string, CommandLineParameterFlags>()
            {
                { ConfigFile, CommandLineParameterFlags.Value },
                { ReportFile, CommandLineParameterFlags.Value },
            });
            return clp;
        }
        #endregion
    }
}
