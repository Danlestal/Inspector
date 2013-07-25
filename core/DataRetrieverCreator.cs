using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioScriptInspector.Core
{
    /// <summary>
    /// This class allow us to instance a DataRetriever given an speficic format. It´s basically a Factory pattern. A DataRetriever is a wrapper around a Racccoon plugin.
    /// </summary>
    public static class DataRetrieverCreator
    {
        /// <summary>
        /// Currently supported formats
        /// </summary>
        public enum DataRetrieverFormats
        {
            RaccoonExcel,
            RaccoonDirectories
        }
        /// <summary>
        /// This method will instance a DataRetriever object.
        /// </summary>
        /// <param name="format">The format our data retriever will have</param>
        /// <param name="parameters">The parameters used to init the Raccoon plugin.</param>
        /// <param name="filePath">Path to the source file we are going to use to get the data</param>
        /// <param name="category">Category used by Racccon plugin.</param>
        /// <param name="caseSensitive">If tru, the data retriever returned will be case sensitive.</param>
        /// <returns></returns>
        public static DataRetriever GenerateDataRetriever(DataRetrieverFormats format,Dictionary<string, object> parameters,string filePath,string category,bool caseSensitive) 
        {
            DataRetriever result = null;
            switch (format)
            {
                case(DataRetrieverFormats.RaccoonExcel):
                     result = new DataRetriever(RaccoonFactory.GetInstance().InstancePlugin("excel", parameters), new EA.Eism.Raccoon.TIL.ConnectionInfo(filePath),category);
                break;
                case (DataRetrieverFormats.RaccoonDirectories):
                     result = new DataRetriever(RaccoonFactory.GetInstance().InstancePlugin("directories", parameters), new EA.Eism.Raccoon.TIL.ConnectionInfo(), String.Empty);
                break;
            }
            result.CaseSensitive = caseSensitive;
            return result;
        }
    }
}
