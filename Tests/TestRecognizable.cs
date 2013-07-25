using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    internal class TestRecognizable : AudioScriptInspector.Core.IRecognizable
    {
        string _number;
        Dictionary<string, string> _values;

        public TestRecognizable(string number)
        {
            _number = number;
            _values = new Dictionary<string, string>();
            _values.Add("parameter", number +"value");
        }

        public string GetKey()
        {
           return _number.ToString();
        }

        public string GetParameter(string key)
        {
            return _values[key];
        }

        public string[] GetParameterKeys()
        {
            return _values.Keys.ToArray();
        }

        public string[] GetParameterValues()
        {
            return _values.Values.ToArray();
        }
    }
}
