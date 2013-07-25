using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToExcel;
namespace AudioScriptInspector.Core
{
    public class ExcelScriptFileGossip : IScriptFileGossip
    {
        private string _file;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public ExcelScriptFileGossip(string file)
        {
            _file = file;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string>  GetColumns()
        {
            var book = new ExcelQueryFactory(_file);
            try
            {
                return book.GetColumnNames(book.GetWorksheetNames().First()).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCategory()
        {
            var book = new ExcelQueryFactory(_file);
            try
            {
                return book.GetWorksheetNames().First();
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
