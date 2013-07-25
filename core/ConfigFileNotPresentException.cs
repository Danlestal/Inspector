using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioScriptInspector.Core
{
    public class ConfigFileNotPresentException : Exception
    {
        public ConfigFileNotPresentException(string message):base(message) { }
    }
}
