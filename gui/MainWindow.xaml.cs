using System;
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
using System.Windows.Forms;
using AudioScriptInspector.Core;
using System.Windows.Threading;
using System.Runtime.Serialization;

namespace AudioScriptInspector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wizardOutput"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private DataRetriever initDataRetriever(AudioScriptInspector.Core.ScriptConfig wizardOutput, bool caseSensitive)
        {
            var parameters = new Dictionary<string, object>() { { "TranslationTextColumnName", wizardOutput.ScriptIDColumn }, { "StringIdColumnName", wizardOutput.ScriptIDColumn }, { "PlatformIndependentColumnList", wizardOutput.ScriptColumns.Except(new string[] { wizardOutput.ScriptIDColumn }).ToArray() }, { "UseMemoCheater", false } };
            return DataRetrieverCreator.GenerateDataRetriever(DataRetrieverCreator.DataRetrieverFormats.RaccoonExcel, parameters, wizardOutput.ScriptFileName, wizardOutput.Category, caseSensitive);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wizardOutput"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private DataRetriever initDataRetriever(AudioScriptInspector.Core.FilesConfig wizardOutput, bool caseSensitive)
        {
            var parameters = new Dictionary<string, object>() { { "SourceDir", wizardOutput.SourceBaseFolder }, { "Includes", wizardOutput.FilesExtension }, { "WhoIsID", "filename" } };
            return DataRetrieverCreator.GenerateDataRetriever(DataRetrieverCreator.DataRetrieverFormats.RaccoonDirectories, parameters, string.Empty, string.Empty, caseSensitive);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _showResultsButton_Click(object sender, RoutedEventArgs e)
        {
            RaccoonFactory.GetInstance().AddAssemblyFolder(System.Environment.CurrentDirectory + @"\Lib");
            _exportItem.IsEnabled = false;
            _saveItem.IsEnabled = false;
            if (_configTab.PrepareInspectorConfig())
            {
                List<DataRetriever> filesRetrieverList = new List<DataRetriever>();
                foreach (FilesConfig filesConfigItem in _configTab.WInfo.FilesConfigList)
                {
                    filesRetrieverList.Add(initDataRetriever(filesConfigItem, _configTab.WInfo.CaseSensitive));
                }
                DataRetriever scriptRetriever = initDataRetriever(_configTab.WInfo.ScriptConfig, _configTab.WInfo.CaseSensitive);

                if ((filesRetrieverList != null) && (scriptRetriever != null))
                {
                    _audioInspectorControl.IsEnabled = true;
                    _audioInspectorControl.OnDataLoaded += new EventHandler<EventArgs>(_audioInspectorControl_OnDataLoaded);
                    _audioInspectorControl.InitControl(scriptRetriever, "SCRIPT", filesRetrieverList, "FILES");
                }
                else
                    throw new Exception("everything went wrong");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _audioInspectorControl_OnDataLoaded(object sender, EventArgs e)
        {
            _exportItem.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Action)(() => { _exportItem.IsEnabled = true; }));
            _saveItem.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Action)(() => { _saveItem.IsEnabled = true; }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportResults_Click(object sender, RoutedEventArgs e)
        {
            if (!_audioInspectorControl.HasDataLoaded)
            {
                System.Windows.MessageBox.Show("No data to export");
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel File (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    _audioInspectorControl.exportToExcel(saveFileDialog.FileName);
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml file (*.xml)|*.xml";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                _configTab.WInfo.Serialize(saveFileDialog.FileName);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadItem_Click(object sender, RoutedEventArgs e)
        {
             var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Choose XML File";
            dialog.Filter = "Xml file (*.xml)|*.xml";
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var config = AudioScriptInspector.Core.InspectorConfig.DeserializeInspectorConfig(dialog.FileName);
                    _configTab.SetConfig(config);
                    _configTab.IsEnabled = true;
                }
                catch (SerializationException exception)
                {
                    System.Windows.MessageBox.Show("The config file is not on a compatible format\n\n" + exception.Message);
                }
                
            }
        }
    }
}
