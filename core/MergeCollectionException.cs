using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioScriptInspector.Core
{
    public class MergeCollectionException : Exception
    {
        public MergeCollectionException(string message) : base(message) { }
    }
}
