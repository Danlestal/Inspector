using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Linq;

namespace AudioScriptInspector.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class InspectorMerger
    {
        public IEnumerable<IRecognizable> LeftData { get; set; }
        public string LeftCollectionName { get; set; }
        public IEnumerable<IRecognizable> RightData { get; set; }
        public string RightCollectionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftData"></param>
        /// <param name="leftCollectionName"></param>
        /// <param name="rightData"></param>
        /// <param name="rightCollectionName"></param>
        public InspectorMerger(IEnumerable<IRecognizable> leftData, string leftCollectionName, IEnumerable<IRecognizable> rightData,string rightCollectionName)
        {
            LeftData = leftData;
            LeftCollectionName = leftCollectionName;
            RightData = rightData;
            RightCollectionName = rightCollectionName;
        }
        /// <summary>
        /// Merge the two collections obtained from Raccoon into a single one and returns a table with all the gathered data.
        /// </summary>
        /// <returns></returns>
        public MergedCollection MergeCollections()
        {
            if ((RightData==null)||(!RightData.Any()))
            {
                throw new MergeCollectionException("No data found on " + RightCollectionName + " collection");
            }
            if ((LeftData == null) || (!LeftData.Any()))
            {
                throw new MergeCollectionException("No data found on " + LeftCollectionName + " collection");
            }
            List<string> keys = new List<string>();
            keys.Add("Status");
            keys.Add(LeftCollectionName + " Key");
            keys.Add(RightCollectionName + " Key");

            foreach (string customProperty in LeftData.First().GetParameterKeys())
                keys.Add(LeftCollectionName + "_" + customProperty);

            foreach (string customProperty in RightData.First().GetParameterKeys())
              keys.Add(RightCollectionName + "_" + customProperty);

            MergedCollection mergedCollection = new MergedCollection(LeftCollectionName, RightCollectionName, keys.ToArray());

            var left = from leftItem in LeftData
                       join rightItem in RightData
                       on leftItem.GetKey() equals rightItem.GetKey() into JoinedEmptyRight
                       from rightItem in JoinedEmptyRight.DefaultIfEmpty()
                       select new { LEFT = leftItem, RIGHT = rightItem };

            var right = (from rightItem in RightData
                         join leftItem in LeftData
                         on rightItem.GetKey() equals leftItem.GetKey() into JoinedEmptyLeft
                         from leftItem in JoinedEmptyLeft.DefaultIfEmpty()
                         select new { LEFT = leftItem, RIGHT = rightItem }).Where(s => s.LEFT == null);


            var join = left.Concat(right);

            mergedCollection.MatchNumber = join.Where(s => (s.LEFT != null) && (s.RIGHT != null)).Count();
            mergedCollection.LeftNull = join.Where(s => s.LEFT == null).Count();
            mergedCollection.RightNull = join.Where(s => s.RIGHT == null).Count();

            foreach (var item in join)
            {
                var row = mergedCollection.NewItem();
                if (item.LEFT != null)
                    row[LeftCollectionName + " Key"] = item.LEFT.GetKey();
                if (item.RIGHT != null)
                    row[RightCollectionName + " Key"] = item.RIGHT.GetKey();

                row["Status"] = "OK";
                if (item.LEFT != null)
                {
                    foreach (string key in item.LEFT.GetParameterKeys())
                    {
                        row[LeftCollectionName + "_" + key] = item.LEFT.GetParameter(key);
                    }
                }
                else
                {
                    row["Status"] = "NOT ON "+LeftCollectionName;
                }
                if (item.RIGHT != null)
                {
                    foreach (string key in item.RIGHT.GetParameterKeys())
                    {
                        row[RightCollectionName + "_" + key] = item.RIGHT.GetParameter(key);
                    }
                }
                else
                {
                    row["Status"] = "NOT ON "+RightCollectionName;
                }

                mergedCollection.AddItem(row);
            }
            return mergedCollection;
        }
    }
}
