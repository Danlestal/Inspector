namespace AudioScriptInspector.Core
{
    /// <summary>
    /// Returns info about our currently supported formats. TODO: Scripth formats has to be some kind of dictionary.
    /// </summary>
    public class SupportedFormats
    {
        /// <summary>
        /// Enum with all the currently supported formats.
        /// </summary>
        public enum SupportedScriptsFormat
        {
            Excel
        };
        public const string ScriptFormats = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv";
     }
}
