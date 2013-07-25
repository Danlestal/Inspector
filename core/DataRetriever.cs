using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AudioScriptInspector.Core
{
    /// <summary>
    /// This class is a wrapper around a Raccoon plugin. Plugin activation and retrieving the data.
    /// Not really sure why Cesar decided to separate the plugin instanciation and its execution, it seems to me to be unnecessary.
    /// </summary>
    public class DataRetriever
    {
        /// <summary>
        /// Event triggered when the importation has finished
        /// </summary>
        public event EventHandler<ImportationFinishEventArgs> OnImportationFinish;
        /// <summary>
        /// ITextSource used. Already instanced.
        /// </summary>
        public EA.Eism.Raccoon.TIL.ITextSource Source {get; set;}
        /// <summary>
        /// RACCCON pluginc connection
        /// </summary>
        public EA.Eism.Raccoon.TIL.ConnectionInfo Connection{get;set;}
        /// <summary>
        /// RACCCON plugin category
        /// </summary>
        public string Category {get; set;}
        /// <summary>
        /// Defines if the key returned by the data gatherer has to be case sensitive or insensitive.
        /// </summary>
        public bool CaseSensitive {get; set;}
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IRecognizable> DataRetrieved
        {
            get { return _dataRetrieved; }
            set { _dataRetrieved = value; }
        }

        private IEnumerable<IRecognizable> _dataRetrieved;

        private EA.Eism.DotNetUtils.Types.LanguageMap _map;
        /// <summary>
        /// Constructor. It inits all the plugin params and lets the plugin ready for its use.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="connection"></param>
        /// <param name="category"></param>
        /// <param name="map"></param>
        public DataRetriever(EA.Eism.Raccoon.TIL.ITextSource source,EA.Eism.Raccoon.TIL.ConnectionInfo connection, string category, EA.Eism.DotNetUtils.Types.LanguageMap map)
        {
            Source = source;
            Connection = connection;
            Category = category;
            CaseSensitive = false;
            _map = map;

        }
        /// <summary>
        /// Use when there is no need of using an speficic Language_Map.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="connection"></param>
        /// <param name="category"></param>
        public DataRetriever(EA.Eism.Raccoon.TIL.ITextSource source, EA.Eism.Raccoon.TIL.ConnectionInfo connection, string category)
        {
            Source = source;
            Connection = connection;
            Category = category;
            //Init a default language map
            var defaultLanguageList = new List<EA.Eism.DotNetUtils.Types.Language>();
            defaultLanguageList.Add(EA.Eism.DotNetUtils.Types.LanguageUtils.ParseName("ENG_US"));
            var defaultPlatformList = new List<EA.Eism.DotNetUtils.Types.Platform>();
            defaultPlatformList.Add(EA.Eism.DotNetUtils.Types.Platform.PC);
            _map = new EA.Eism.DotNetUtils.Types.LanguageMap(defaultPlatformList, defaultLanguageList);

        }
        /// <summary>
        /// Starts the pluging execution. For performance reasons it executes on an independent thread.
        /// </summary>
        public void GetData()
        {
            var thread = new Thread(new ThreadStart(GetDataAux));
            thread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        private void GetDataAux()
        {
            _dataRetrieved = Source.GetStrings(Connection, Category, _map).Select(s=> new TextStringWrapper(s,CaseSensitive));
            RaiseSessionWizardOutputEvent();
        }
       /// <summary>
       /// 
       /// </summary>
        protected void RaiseSessionWizardOutputEvent()
        {
            if (OnImportationFinish != null)
            {
                foreach (Delegate del in OnImportationFinish.GetInvocationList())
                {
                    EventHandler<ImportationFinishEventArgs> changeHanler = del as EventHandler<ImportationFinishEventArgs>;
                    changeHanler(this, new ImportationFinishEventArgs(_dataRetrieved));
                }
            }
        }
    
    
    }
}
