using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Forms;


using AudioScriptInspector.Core;
using Excel = Microsoft.Office.Interop.Excel; 
using System.Data.OleDb;


namespace AudioScriptInspector.Controls
{
    /// <summary>
    /// Interaction logic for AudioScriptInspectorControl.xaml
    /// </summary>
    public partial class AudioScriptInspectorControl : System.Windows.Controls.UserControl
    {

        private DataSet _auxDataSet;
        private IEnumerable<IRecognizable> LeftData { get; set; }
        private IEnumerable<IRecognizable> RightData { get; set; }
        private bool _dataLoaded;
        private string LeftPrefix { get; set; }
        private string RightPrefix { get; set; }
        private ImportationManager _importationManager;
        
        public bool HasDataLoaded
        {
            get
            {
                return _dataLoaded;
            }
        }
        public event EventHandler<EventArgs> OnDataLoaded;
        /// <summary>
        /// 
        /// </summary>
        public AudioScriptInspectorControl()
        {
            RightData = new List<IRecognizable>();
            LeftData = new List<IRecognizable>();
            _importationManager = new ImportationManager(false);
            _dataLoaded = false;
            InitializeComponent();
            _inspectionDataGrid.IsEnabled = false;
            _loadingAnimation.IsEnabled = false;
            _loadingAnimation.Visibility = Visibility.Hidden;
            _auxDataSet = new DataSet();
        }
        /// <summary>
        ///     
        /// </summary>
        /// <param name="leftRetriever"></param>
        /// <param name="leftPrefix"></param>
        /// <param name="rightRetrieversList"></param>
        /// <param name="rightPrefix"></param>
        public void InitControl(DataRetriever leftRetriever, string leftPrefix, List<DataRetriever> rightRetrieversList, string rightPrefix)
        {
            LeftPrefix = leftPrefix;
            RightPrefix = rightPrefix;
            _inspectionDataGrid.ItemsSource = null;
            _inspectionDataGrid.IsEnabled = false;
            _loadingAnimation.IsEnabled = true;
            _loadingAnimation.Visibility = Visibility.Visible;
            _loadingAnimation.BringIntoView();

            //Async importation manager.
            _importationManager.OnImportationFinish += new EventHandler<EventArgs>(manager_OnImportationFinish);
            leftRetriever.OnImportationFinish += new EventHandler<ImportationFinishEventArgs>(leftRetriever_OnImportationFinish);
            _importationManager.AddRetriever(leftRetriever);
            foreach (DataRetriever rightRetriever in rightRetrieversList)
            {
                rightRetriever.OnImportationFinish += new EventHandler<ImportationFinishEventArgs>(rightRetriever_OnImportationFinish);
                _importationManager.AddRetriever(rightRetriever);
            }
            _importationManager.Import();

        }

        void manager_OnImportationFinish(object sender, EventArgs e)
        {
            InitInspector();
            RaiseDataLoadedEvent();
            _inspectionDataGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Action)(() => { _inspectionDataGrid.IsEnabled = true; }));
            _loadingAnimation.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Action)(() => { _loadingAnimation.IsEnabled = false; }));
            _loadingAnimation.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Action)(() => { _loadingAnimation.Visibility = Visibility.Hidden; }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rightRetriever_OnImportationFinish(object sender, ImportationFinishEventArgs e)
        {
            RightData = RightData.Concat(e.RetrievedData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void leftRetriever_OnImportationFinish(object sender, ImportationFinishEventArgs e)
        {
            LeftData = e.RetrievedData;
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitInspector()
        {
           InspectorMerger merger = new InspectorMerger(LeftData,LeftPrefix,RightData,RightPrefix);
           try
           {
               _auxDataSet = merger.MergeCollections().ToDataSet();
           }
           catch (MergeCollectionException e)
           {
               System.Windows.Forms.MessageBox.Show(e.Message);
           }

           if (_inspectionDataGrid.Dispatcher.CheckAccess())
           {
                _inspectionDataGrid.ItemsSource = _auxDataSet.Tables[1].DefaultView;
           }
           else
           {
                _inspectionDataGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Action)(() => { _inspectionDataGrid.ItemsSource = _auxDataSet.Tables[1].DefaultView; }));
           }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void RaiseDataLoadedEvent()
        {
            _dataLoaded = true;
            if (OnDataLoaded != null)
            {
                foreach (Delegate del in OnDataLoaded.GetInvocationList())
                {
                    EventHandler<EventArgs> changeHanler = del as EventHandler<EventArgs>;
                    changeHanler(this, null);
                }
            }
        }
        /// <summary>
        ///  Fast method to export the data we have on our datagrid to an Excel file.
        /// </summary>
        /// <param name="fileName">Destination filename</param>
        public void exportToExcel(string fileName)
        {
            CreateExcelFile.CreateExcelDocument(_auxDataSet, fileName);
        }
    }
}
