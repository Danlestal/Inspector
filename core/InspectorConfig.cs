using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace AudioScriptInspector.Core
{
    [DataContract(Name = "InspectorConfig", Namespace = "AudioScriptInspector.Core")]
    public class InspectorConfig
    {
        /// <summary>
        /// Configuration obtained from the user about the files we are going to get into our system
        /// </summary>
        [DataMember()]
        public List<FilesConfig> FilesConfigList { get; set; }
        /// <summary>
        /// Configuration obtained from the user about the script we are going to get into our system
        /// </summary>
        [DataMember()]
        public ScriptConfig ScriptConfig { get; set; }
        /// <summary>
        /// If we are going to take into account the casing of the IDs while doing our comparation
        /// </summary>
        [DataMember()]
        public bool CaseSensitive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public InspectorConfig()
        {
        }
        /// <summary>
        /// Serializer contructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public InspectorConfig(SerializationInfo info, StreamingContext ctxt)
        {
            FilesConfigList = (List<FilesConfig>)info.GetValue("FilesConfig", typeof(FilesConfig));
            ScriptConfig = (ScriptConfig)info.GetValue("ScriptConfig", typeof(ScriptConfig));
        }
        /// <summary>
        /// Serialiation method
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("FilesConfig", FilesConfigList);
            info.AddValue("ScriptConfig", ScriptConfig);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void Serialize(string file)
        {
            using (FileStream writer = new FileStream(file, FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(this.GetType());
                ser.WriteObject(writer, this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static InspectorConfig DeserializeInspectorConfig(string file)
        {
            if (!File.Exists(file))
            {
                throw new ConfigFileNotPresentException("File provided to deserialize not found");
            }
            InspectorConfig deserializedWizard = null;
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using(XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(InspectorConfig));
                    deserializedWizard = (InspectorConfig)ser.ReadObject(reader, true);
                }
            }
            return deserializedWizard;
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
            InspectorConfig target = obj as InspectorConfig;
            if (this.CaseSensitive != target.CaseSensitive)
            {
                return false;
            }
            if (target.FilesConfigList.TrueForAll(s => this.FilesConfigList.Contains(s)) && (target.ScriptConfig.Equals(this.ScriptConfig)))
            {
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
            return (FilesConfigList.GetHashCode() + ScriptConfig.GetHashCode());
        }
    }
}
