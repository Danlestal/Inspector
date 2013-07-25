using AudioScriptInspector.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Collections.Generic;

namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for InspectorMergerTest and is intended
    ///to contain all InspectorMergerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InspectorMergerTest
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

        /// <summary>
        ///A test for MergeCollections
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EA.Eism.Inspector.InspectorCore.dll")]
        public void MergeCollectionsTest()
        {
            TestRecognizable[] leftCollection = new TestRecognizable[] { new TestRecognizable("a")};
            TestRecognizable[] rightCollection = new TestRecognizable[] { new TestRecognizable("a")}; ;
            InspectorMerger target = new InspectorMerger(leftCollection, "LEFT", rightCollection, "RIGHT");
            MergedCollection actual = target.MergeCollections();
            // TODO: Init result value.
            MergedCollection expected = new MergedCollection("LEFT","RIGHT",new[] { "Status", "LEFT Key", "RIGHT Key", "LEFT_parameter", "RIGHT_parameter" });
            Dictionary<string,object> values = expected.NewItem();
            values["Status"] = "OK";
            values["LEFT Key"] = "a";
            values["RIGHT Key"] = "a";
            values["LEFT_parameter"]= "avalue";
            values["RIGHT_parameter"]=  "avalue";
            expected.AddItem(values);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        [DeploymentItem("EA.Eism.Inspector.InspectorCore.dll")]
        public void MergeCollectionsTestWithOrphans()
        {
            TestRecognizable[] leftCollection = new TestRecognizable[] { new TestRecognizable("a"), new TestRecognizable("b") };
            TestRecognizable[] rightCollection = new TestRecognizable[] { new TestRecognizable("a"), new TestRecognizable("c") }; ;
            InspectorMerger target = new InspectorMerger(leftCollection, "LEFT", rightCollection, "RIGHT");
            MergedCollection actual = target.MergeCollections();
            // TODO: Init result value.
            Assert.IsTrue(actual.LeftNull == 1);
            Assert.IsTrue(actual.RightNull == 1);
            Assert.IsTrue(actual.MatchNumber == 1);
        }
        [TestMethod()]
        [DeploymentItem("EA.Eism.Inspector.InspectorCore.dll")]
        [ExpectedException(typeof(MergeCollectionException))]
        public void MergeCollectionsLeftNull()
        {
            TestRecognizable[] leftCollection = null;
            TestRecognizable[] rightCollection = new TestRecognizable[] { new TestRecognizable("a"), new TestRecognizable("c") }; ;
            InspectorMerger target = new InspectorMerger(leftCollection, "LEFT", rightCollection, "RIGHT");
            MergedCollection actual = target.MergeCollections();
        }
        [TestMethod()]
        [DeploymentItem("EA.Eism.Inspector.InspectorCore.dll")]
        [ExpectedException(typeof(MergeCollectionException))]
        public void MergeCollectionsRightNull()
        {
            TestRecognizable[] leftCollection = new TestRecognizable[] { new TestRecognizable("a"), new TestRecognizable("c") };
            TestRecognizable[] rightCollection = null;
            InspectorMerger target = new InspectorMerger(leftCollection, "LEFT", rightCollection, "RIGHT");
            MergedCollection actual = target.MergeCollections();
        }

    }
}
