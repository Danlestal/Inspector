using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EA.Eism.Raccoon.TEL;


namespace AudioScriptInspector.Core
{
    public class RaccoonFactory
    {
        private RaccoonPluginLoader _pluginLoader;
           #region SingletonStuff
        /// <summary>
        /// 
        /// </summary>
        private static RaccoonFactory _instance;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RaccoonFactory GetInstance()
        {
            if(_instance == null)
            {
                _instance = new RaccoonFactory();
            }
            return _instance;
        }
        #endregion
        /// <summary>
        /// Unaccesible: Only way to operate with this class is through he singleton.
        /// </summary>
        private RaccoonFactory()
        {
          _pluginLoader = new EA.Eism.Raccoon.TEL.RaccoonPluginLoader();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        public void AddAssemblyFolder(string folder)
        {
            _pluginLoader.AddAssemblies(folder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public EA.Eism.Raccoon.TIL.ITextSource InstancePlugin(string format, Dictionary<string, object> parameters)
        {
            return _pluginLoader.CreateTextSource(format, parameters);
        }
    }
}
