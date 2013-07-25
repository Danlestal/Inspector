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
using AudioScriptInspector.Core;


namespace AudioScriptInspector.Controls
{
    /// <summary>
    /// Interaction logic for ConfigTab.xaml
    /// </summary>
    public partial class ConfigTab : UserControl
    {
        private string _category;
        private bool _caseSensitive;
        /// <summary>
        /// 
        /// </summary>
        public InspectorConfig WInfo{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConfigTab()
        {
            _caseSensitive = false;
            _category = string.Empty;
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat GinmeFormat(string file)
        {
            if ((System.IO.Path.GetExtension(file) == @".xls") || (System.IO.Path.GetExtension(file) == @".xlsx") || (System.IO.Path.GetExtension(file) == @".csv"))
            {
                return AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat.Excel;
            }
            throw new Exception("Not supported format");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _scriptFileBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            _columnsComboBox.ItemsSource = null;
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Choose Script File";
            dialog.Filter = AudioScriptInspector.Core.SupportedFormats.ScriptFormats;
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _scriptFileTextBox.Text = dialog.FileName;
                System.IO.Path.GetExtension(dialog.FileName);
                AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat format = GinmeFormat(dialog.FileName);
                IScriptFileGossip gossip = null;
                switch (format)
                {
                    case (AudioScriptInspector.Core.SupportedFormats.SupportedScriptsFormat.Excel):
                        gossip = new ExcelScriptFileGossip(dialog.FileName);
                    break;
                }
                var columns = gossip.GetColumns();
                if (columns != null)
                {
                    _columnsComboBox.ItemsSource = columns;
                    _columnsComboBox.IsEnabled = true;
                    _category = gossip.GetCategory();
                }
                else
                {
                    _scriptFileTextBox.Text = string.Empty;
                    MessageBox.Show("The script file is not on a compatible format");
                }

            }
        }
        /// <summary>
        /// Builds the WInfo propery.
        /// </summary>
       public bool PrepareInspectorConfig()
        {
            if (validateGatheredData())
            {
                if (WInfo == null)
                {
                    WInfo = new InspectorConfig();
                    WInfo.FilesConfigList = _filesCollectionListBox.Items.Cast<FilesConfig>().ToList();
                    WInfo.ScriptConfig = new ScriptConfig(_scriptFileTextBox.Text, _columnsComboBox.Items.Cast<string>().ToArray(), _columnsComboBox.SelectedValue.ToString(), _category, GinmeFormat(_scriptFileTextBox.Text));
                    WInfo.CaseSensitive = _caseSensitive;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool validateGatheredData()
        {
            if (_scriptFileTextBox.Text == string.Empty)
            {
                MessageBox.Show("No Script source selected");
                return false;
            }
            if ((_columnsComboBox.SelectedValue == null) || (_columnsComboBox.SelectedValue.ToString() == string.Empty))
            {
                MessageBox.Show("No column for the filename on the script selected");
                return false;
            }
            if (_filesCollectionListBox.Items.Count == 0)
            {
                MessageBox.Show("No base folder for our files selected.");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _physFolderBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog lOpenFolderDlg = new Microsoft.Win32.OpenFileDialog();
            lOpenFolderDlg.Title = "Choose phys files base folder";
            lOpenFolderDlg.CheckFileExists = false;
            lOpenFolderDlg.CheckPathExists = true;
            lOpenFolderDlg.FileName = "[Select Folder…]";
            lOpenFolderDlg.Filter = "Folders|no.files";
            if ((bool)lOpenFolderDlg.ShowDialog())
            {
                _baseFolderTextBox.Text = System.IO.Path.GetDirectoryName(lOpenFolderDlg.FileName);
            }
        }

        public void SetConfig(InspectorConfig info)
        {
            WInfo = info;
            _category = info.ScriptConfig.Category;
            _filesCollectionListBox.ItemsSource = info.FilesConfigList;
            _scriptFileTextBox.Text = info.ScriptConfig.ScriptFileName;
            _columnsComboBox.ItemsSource = info.ScriptConfig.ScriptColumns;
            _columnsComboBox.IsEnabled = true;
            _columnsComboBox.SelectedItem = info.ScriptConfig.ScriptIDColumn;
        }

        private void _caseSensitiveCheckBox_Click(object sender, RoutedEventArgs e)
        {
            _caseSensitive = (bool)_caseSensitiveCheckBox.IsChecked;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if ((_baseFolderTextBox.Text != string.Empty) && (_filesExtensionTextBox.Text != string.Empty))
            {
                FilesConfig config = new FilesConfig(_baseFolderTextBox.Text, _filesExtensionTextBox.Text);
                _filesCollectionListBox.Items.Add(config);
                _baseFolderTextBox.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("No base folder or extension selected");
            }
           
        }

        private void _deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_filesCollectionListBox.SelectedItem != null)
            {
                var list = _filesCollectionListBox.ItemsSource.Cast<FilesConfig>().ToList();
                list.Remove((FilesConfig)_filesCollectionListBox.SelectedItem);
                _filesCollectionListBox.ItemsSource = list;
            }
        }
    }
}
