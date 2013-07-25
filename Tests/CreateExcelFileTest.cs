using AudioScriptInspector.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;

namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for CreateExcelFileTest and is intended
    ///to contain all CreateExcelFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CreateExcelFileTest
    {


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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CreateExcelDocument
        ///</summary>
        [TestMethod()]
        public void CreateExcelDocumentTest()
        {
            if (File.Exists("result.xls"))
            {
                File.Delete("result.xls");
            }
            DataSet ds = CreateDataSet(4,200); 
            string xlsxFilePath = "result.xls"; 
            CreateExcelFile.CreateExcelDocument(ds, xlsxFilePath);
            Assert.IsTrue(File.Exists("result.xls"));
        }

        [TestMethod()]
        public void CreateExcelDocumentTestMassive()
        {
            if (File.Exists("resultMassive.xlsx"))
            {
                File.Delete("resultMassive.xlsx");
            }
            DataSet ds = CreateDataSet(4, 200000);
            string xlsxFilePath = "resultMassive.xlsx";
            CreateExcelFile.CreateExcelDocument(ds, xlsxFilePath);
            Assert.IsTrue(File.Exists("resultMassive.xlsx"));
        }


        private System.Data.DataSet CreateDataSet(int numColumnsOnTable, int numOfRowsOnTable)
        {
            DataTable table = new DataTable();
            object[] rowParameters = new object[numColumnsOnTable];
            for(int i=0;i<numColumnsOnTable;++i)
            {
                table.Columns.Add(i.ToString());
                rowParameters[i] = "a";
            }

            for(int i=0;i<numOfRowsOnTable;++i)
                table.Rows.Add(rowParameters);

            DataSet resultDataSet = new DataSet();
            resultDataSet.Tables.Add(table);
            return resultDataSet;
        }


    }
}
