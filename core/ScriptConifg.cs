using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AudioScriptInspector.Core
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "ScriptConfig", Namespace = "AudioScriptInspector.Core")]
    public class ScriptConfig
    {
        [DataMember(Name = "ScriptFileName")]
        private readonly string _scriptFileName;
        public string ScriptFileName
        {
            get { return _scriptFileName; }
        }
        [DataMember(Name = "ScriptColumns")]
        private readonly string[] _scriptColumns;
        public string[] ScriptColumns
        {
            get { return _scriptColumns; }
        }
        [DataMember(Name = "ScriptIDColumn")]
        private readonly string _scriptIDColumn;
        public string ScriptIDColumn
        {
            get { return _scriptIDColumn; }
        }
        [DataMember(Name = "Category")]
        private readonly string _category;
        public string Category
        {
            get { return _category; }
        }
        [DataMember(Name = "ScriptFormat")]
        private readonly AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat _scriptFormat;
        public AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat ScriptFormat
        {
            get { return _scriptFormat; }
        } 

        public ScriptConfig()
        { }
        
        public ScriptConfig(string scriptFileName, string[] scriptColumns, string scriptIDColumn, string category, AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat scriptFormat)
        {
            this._scriptFileName = scriptFileName;
            this._scriptColumns = scriptColumns;
            this._scriptIDColumn =  scriptIDColumn;
            this._category = category;
            this._scriptFormat = scriptFormat;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;
            ScriptConfig target = obj as ScriptConfig;
            if ((target.ScriptFileName == this.ScriptFileName) && 
                (target.ScriptIDColumn == this.ScriptIDColumn)&&
                (target.Category == this.Category)&&
                (target.ScriptFormat == this.ScriptFormat))
            {
                if (_scriptColumns.Length != target.ScriptColumns.Length)
                {
                    return false;
                }
                for (int i = 0; i < _scriptColumns.Length; ++i)
                {
                    if (_scriptColumns[i] != target.ScriptColumns[i])
                        return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (_scriptFileName.GetHashCode() + _scriptColumns.GetHashCode() + _scriptColumns.GetHashCode() + _scriptIDColumn.GetHashCode() + _category.GetHashCode() + _scriptFormat.GetHashCode());
        }
    }
}