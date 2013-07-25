using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioScriptInspector.Core
{
    /// <summary>
    /// This interface set the contract that any class that peeks-up a script file must accomplish.
    /// </summary>
    public interface IScriptFileGossip
    {
        /// <summary>
        /// Returns all the columns that the script currently have
        /// </summary>
        /// <returns>A list with the name of the columns that the script currently have.</returns>
        List<string> GetColumns();
        /// <summary>
        /// Returns the category name. The category is used by the RACCCON plugin and usually is related with the name of the Excel sheet we are trying to read. TODO: not sure if this method is usefull
        /// </summary>
        /// <returns>The script category name.</returns>
        string GetCategory();
    }
}
