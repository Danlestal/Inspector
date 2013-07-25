using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AudioScriptInspector.Core
{
    [DataContract(Name = "FilesConfig", Namespace = "AudioScriptInspector.Core")]
    public class FilesConfig
    {
        [DataMember(Name = "SourceBaseFolder")]
        private string _sourceBaseFolder;
        /// <summary>
        /// The root folder from where we are going to take all the files.
        /// </summary>
       
        public string SourceBaseFolder
        {
            get { return _sourceBaseFolder; }
        }

        [DataMember(Name= "FilesExtension")]
        private string _filesExtension;
        /// <summary>
        /// The file extension we are going to retrieve. Its a regular expression.
        /// </summary>
        public string FilesExtension
        {
            get { return _filesExtension; }
        } 
        /// <summary>
        /// 
        /// </summary>
        public FilesConfig()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceBaseFolder"></param>
        /// <param name="filesExtension"></param>
        public FilesConfig(string sourceBaseFolder, string filesExtension)
        {
            this._sourceBaseFolder = sourceBaseFolder;
            this._filesExtension = filesExtension;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;
            FilesConfig target = obj as FilesConfig;
            if ((target.SourceBaseFolder == this.SourceBaseFolder) && (target.FilesExtension == this.FilesExtension))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (SourceBaseFolder.GetHashCode() + FilesExtension.GetHashCode());
        }

        public override string ToString()
        {
            return "FOLDER:["+SourceBaseFolder + "] | SOURCE:[" + FilesExtension+"]";
        }
     }
}