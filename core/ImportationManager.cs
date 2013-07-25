using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Threading;

namespace AudioScriptInspector.Core
{
   /// <summary>
   /// Manager able to deal with several importation threads working on parallel. It has two different ways to work,
   /// </summary>
    public class ImportationManager
    {
        private List<DataRetriever> _retrievers;
        private int _importationsLeft;
        private bool _synchronous;
        /// <summary>
        /// Event triggered when the importation has finished
        /// </summary>
        public event EventHandler<EventArgs> OnImportationFinish;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="synchronous"></param>
        public ImportationManager(bool synchronous)
        {
            _synchronous = synchronous;
            _retrievers = new List<DataRetriever>();
            _importationsLeft = 0;
        }
        /// <summary>
        /// Add a DataRetriever to our collection.
        /// </summary>
        /// <param name="retriever"></param>
        public void AddRetriever(DataRetriever retriever)
        {
            _retrievers.Add(retriever);
           _importationsLeft++;
        }
        /// <summary>
        /// Executes all the retrievers on their independance threads and wait for each one of them to finish.
        /// </summary>
        public void Import()
        {
            if (_importationsLeft != 0)
            {
                foreach (DataRetriever retriever in _retrievers)
                {
                    retriever.OnImportationFinish += new EventHandler<ImportationFinishEventArgs>(Retriever_OnImportationFinish);
                    retriever.GetData();
                }
                if (_synchronous)
                {
                    //todo: change this is really ugly.
                    while (_importationsLeft > 0)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
        /// <summary>
        /// Substract 1 from our ImportationLeft counter. This way I can check if the
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Retriever_OnImportationFinish(object sender, ImportationFinishEventArgs e)
        {
            _importationsLeft--;
            if(_importationsLeft == 0)
                RaiseImportationFinishedEvent();
        }
        /// <summary>
        /// 
        /// </summary>
        protected void RaiseImportationFinishedEvent()
        {
            if (OnImportationFinish != null)
            {
                foreach (Delegate del in OnImportationFinish.GetInvocationList())
                {
                    EventHandler<EventArgs> changeHanler = del as EventHandler<EventArgs>;
                    changeHanler(this, null);
                }
            }
        }


    }
}