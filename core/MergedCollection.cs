using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AudioScriptInspector.Core
{
    public class MergedCollection
    {
        private int _matchNumber;

        public int MatchNumber
        {
            get { return _matchNumber; }
            set { _matchNumber = value; }
        }
        private int _leftNull;

        public int LeftNull
        {
            get { return _leftNull; }
            set { _leftNull = value; }
        }
        private int _rightNull;

        public int RightNull
        {
            get { return _rightNull; }
            set { _rightNull = value; }
        }


        private string _leftCollectioName;

        public string LeftCollectioName
        {
            get { return _leftCollectioName; }
            set { _leftCollectioName = value; }
        }
        private string _rightCollectionName;

        public string RightCollectionName
        {
            get { return _rightCollectionName; }
            set { _rightCollectionName = value; }
        }

        private string[] _keyColumns;
        /// <summary>
        /// The key columns used by this collection.
        /// </summary>
        public string[] KeyColumns
        {
            get { return _keyColumns; }
            set { _keyColumns = value; }
        }
        private List<Dictionary<string, object>> _data;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftCollectionName"></param>
        /// <param name="rightCollectionName"></param>
        /// <param name="columns"></param>
        public MergedCollection(string leftCollectionName, string rightCollectionName, string[] columns)
        {
            _leftCollectioName = leftCollectionName;
            _rightCollectionName = rightCollectionName;
            _keyColumns = columns;
            _matchNumber = 0;
            _leftNull = 0;
            _rightNull = 0;
            _data = new List<Dictionary<string,object>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Dictionary<string,object> item)
        {
            if (item.Keys.ToArray() != _keyColumns)
            {
                _data.Add(item);
            }
            else
                throw new Exception("Item not suitable to this collection");
        }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> NewItem()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in _keyColumns)
            {
                result.Add(key, null);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable ToDataTable(string name)
        {
            var result = new System.Data.DataTable(name);
            foreach (string key in _keyColumns)
                result.Columns.Add(key);
            foreach (Dictionary<string, object> item in _data)
            {
                DataRow row = result.NewRow();
                foreach (string key in item.Keys)
                {
                    row[key] = item[key];
                }
                result.Rows.Add(row);
            }
            return result;
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
            MergedCollection target = obj as MergedCollection;
            if (_keyColumns.Count()!=target._keyColumns.Count())
            {
                return false;
            }
            foreach (string item in _keyColumns)
            {
                if (!target.KeyColumns.Contains(item))
                    return false;
            }
            if (_data.Count != target._data.Count)
            {
                return false;
            }
            /*
            for(int i=0; i<_data.Count; ++i)
            {
               foreach(object item in _data[i].Values!=target._data[i].Values)
                {
                    return false;
                }
            }*/
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (_keyColumns.GetHashCode() + _data.GetHashCode());
        }
        /// <summary>
        /// Creates a digest table.
        /// </summary>
        /// <returns></returns>
        private System.Data.DataTable CreateDigestTable()
        {
            var digestTable = new System.Data.DataTable();
            digestTable.TableName = "Digest";
            digestTable.Columns.Add("MATCHES");
            digestTable.Columns.Add(_leftCollectioName + " ORPHANS");
            digestTable.Columns.Add(_rightCollectionName + " ORPHANS");
            var row = digestTable.NewRow();
            row["MATCHES"] = _matchNumber;
            row[_leftCollectioName + " ORPHANS"] = _rightNull;
            row[_rightCollectionName + " ORPHANS"] = _leftNull;
            digestTable.Rows.Add(row);
            return digestTable;
        }
        /// <summary>
        /// Returns a Dataset with two tables.
        /// The first one contains a digest that shows a digest with the merge results.
        /// The second shows the proper data.
        /// </summary>
        /// <returns></returns>
        public System.Data.DataSet ToDataSet()
        {
            System.Data.DataTable mergeTable = ToDataTable("Results");
            if (mergeTable == null)
                throw new MergeCollectionException("Merged table is null");
            System.Data.DataTable digestTable = CreateDigestTable();
            System.Data.DataSet resultSet = new System.Data.DataSet();
            resultSet.Tables.Add(digestTable);
            resultSet.Tables.Add(mergeTable);
            return resultSet;
        }
    
    }
}
