using System;
using System.Collections.Generic;

namespace AudioScriptInspector.Core
{
    /// <summary>
    ///  Event class returned by a DataGathered when the import process has finished.
    /// </summary>
    public class ImportationFinishEventArgs : EventArgs
    {
        /// <summary>
        /// Data Retrieved by the DataGatherer
        /// </summary>
        public IEnumerable<IRecognizable> RetrievedData { get; set; }
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="data">Data Retrieved by the DataGatherer</param>
        public ImportationFinishEventArgs(IEnumerable<IRecognizable> data)
        {
            RetrievedData = data;
        }
    }
}