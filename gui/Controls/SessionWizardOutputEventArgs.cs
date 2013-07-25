using System;
using AudioScriptInspector.Core;

namespace AudioScriptInspector.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionWizardOutputEventArgs : EventArgs
    {
        public readonly ScriptConfig scriptConfig;
        public readonly FilesConfig filesConfig;
        public readonly bool caseSensitive;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptConfig"></param>
        /// <param name="filesConfig"></param>
        /// <param name="caseSensitive"></param>
        public SessionWizardOutputEventArgs(ScriptConfig scriptConfig,FilesConfig filesConfig,bool caseSensitive)
        {
            this.scriptConfig = scriptConfig;
            this.filesConfig = filesConfig;
            this.caseSensitive = caseSensitive;
        }

    }
}