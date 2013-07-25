using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AudioScriptInspector.Core;
using System.Runtime.Serialization;
using System.Xml;

namespace EA.Eism.Inspector.Tests
{
    /// <summary>
    /// Tests.
    /// </summary>
    [TestClass]
    [DeploymentItem(@"D:\P4\eng\packages\Audio\Inspector\dev\source\Config.xml")]
    [DeploymentItem(@"D:\P4\eng\packages\Audio\Inspector\dev\source\ConfigFail.xml")]
    public class Tests
    {

        /// <summary>
        /// Path to the base of the package.
        /// </summary>
        private const string BasePackagePath = @"..\..\..\..\..\";
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        /// <summary>
        /// test
        /// </summary>
        [TestMethod]
        public void Serialize_Config()
        {
            if (File.Exists(@"config.xml"))
            {
                File.Delete(@"config.xml");
            }
            var info = new InspectorConfig();
            List<FilesConfig> filesList = new List<FilesConfig>();
            filesList.Add(new FilesConfig(@"E:\TWOnline\ML", ".*wav$"));
            filesList.Add(new FilesConfig(@"E:\TWOnline\ML2", ".*wav$"));
            
            info.FilesConfigList = filesList;
            info.ScriptConfig = new ScriptConfig(@"prueba.xls", new string[] { "prueba1", "prueba2" }, "prueba1", "category1", AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat.Excel);
            info.Serialize(@"config.xml");
            Assert.IsTrue(File.Exists("config.xml"));
        }
        /// <summary>
        /// A sample test.
        /// </summary>
        [TestMethod]
        public void DeSerialize_Config()
        {
            if (!File.Exists(@"Config.xml"))
            {
                Assert.Fail("Config file missed");
            }
            InspectorConfig deserializedWizar = InspectorConfig.DeserializeInspectorConfig(@"Config.xml");
        }
        /// <summary>
        /// A sample test.
        /// </summary>
        [TestMethod]
        [ExpectedException (typeof(SerializationException))]
        public void DeSerialize_Config_Fail()
        {
            if (!File.Exists(@"Config.xml"))
            {
                Assert.Fail("Config file missed");
            }
            InspectorConfig deserializedWizar = InspectorConfig.DeserializeInspectorConfig(@"ConfigFail.xml");
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConfigFileNotPresentException))]
        public void DeSerialize_Config_FileNotFound_Fail()
        {
            InspectorConfig deserializedWizar = InspectorConfig.DeserializeInspectorConfig(@"NotPresentFile.xml");
        }
        /// <summary>
        /// A sample test.
        /// </summary>
        [TestMethod]
        public void Serialize_DeSerialize_Config()
        {
            InspectorConfig originalWizard = new InspectorConfig();
            
            List<FilesConfig> filesList = new List<FilesConfig>();
            filesList.Add(new FilesConfig(@"E:\TWOnline\ML", ".*wav$"));
            originalWizard.FilesConfigList = filesList;
            originalWizard.ScriptConfig = new ScriptConfig(@"prueba.xls", new string[] { "prueba1", "prueba2" }, "prueba1", "category1", AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat.Excel);
            originalWizard.Serialize(@"config.xml");
            InspectorConfig deserializedWizard = InspectorConfig.DeserializeInspectorConfig(@"config.xml");
            Assert.AreEqual(originalWizard, deserializedWizard);
        }
        /// <summary>
        /// A sample test.
        /// </summary>
        [TestMethod]
        public void ConfigsComparation()
        {
            InspectorConfig configA = new InspectorConfig();
            configA.CaseSensitive = true;
            configA.FilesConfigList = new List<FilesConfig>();
            configA.ScriptConfig = new ScriptConfig(@"pruebaA.xls", new string[] { "prueba1", "prueba2" }, "prueba1", "category1", AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat.Excel);
            List<FilesConfig> filesList = new List<FilesConfig>();
            filesList.Add(new FilesConfig(@"E:\TWOnline\ML", ".*wav$"));
            configA.FilesConfigList = filesList;
            InspectorConfig configB = new InspectorConfig();
            configB.CaseSensitive = false;
            configB.FilesConfigList = new List<FilesConfig>();
            configB.ScriptConfig = new ScriptConfig(@"pruebaA.xls", new string[] { "prueba1", "prueba2" }, "prueba1", "category1", AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat.Excel);
            List<FilesConfig> filesListB = new List<FilesConfig>();
            filesListB.Add(new FilesConfig(@"E:\TWOnline\ML", ".*wav$"));
            configB.FilesConfigList = filesListB;
            Assert.AreNotEqual(configA, configB);
            configB.CaseSensitive = true;
            Assert.AreEqual(configA, configB);
        }
    
    }
}
