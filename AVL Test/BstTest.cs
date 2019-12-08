using System;
using System.Collections.Generic;
using System.Linq;
using AVLTree.BinaryTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BSTTest
{
    [TestClass]
    public class BstTest
    {
        public class TestData : IData
        {
            public TestData(int key)
            {
                Key = key;
            }

            public TestData(int key, string mesage)
            {
                Key = key;
                Message = mesage;
            }

            public int Key { get; set; }
            public string Message { get; set; }

            public int CompareTo(IData other)
            {
                //allow comparing only test data to test data
                if (other.GetType() == typeof(TestData))
                {
                    if (Key < ((TestData) other).Key)
                        return -1;
                    else if (Key > ((TestData) other).Key)
                        return 1;
                    else
                        return 0;
                }
                else
                {
                    throw new Exception("Test data can be compared only to test data");
                }
            }
        }



        [TestMethod]
        public void InsertTest()
        {
            BinarySearchTree<TestData> bst = new BinarySearchTree<TestData>();
            bst.Insert(new TestData(20));
            bst.Insert(new TestData(10));
            bst.Insert(new TestData(5));
            bst.Insert(new TestData(3));
            bst.Insert(new TestData(1));
            bst.Insert(new TestData(4));
            bst.Insert(new TestData(7));
            bst.Insert(new TestData(6));
            bst.Insert(new TestData(9));
            bst.Insert(new TestData(15));
            bst.Insert(new TestData(13));
            bst.Insert(new TestData(11));
            bst.Insert(new TestData(14));
            bst.Insert(new TestData(17));
            bst.Insert(new TestData(16));
            bst.Insert(new TestData(19));
            bst.Insert(new TestData(30));
            bst.Insert(new TestData(25));
            bst.Insert(new TestData(23));
            bst.Insert(new TestData(21));
            bst.Insert(new TestData(24));
            bst.Insert(new TestData(27));
            bst.Insert(new TestData(26));
            bst.Insert(new TestData(29));
            bst.Insert(new TestData(35));
            bst.Insert(new TestData(33));
            bst.Insert(new TestData(31));
            bst.Insert(new TestData(34));
            bst.Insert(new TestData(37));
            bst.Insert(new TestData(36));
            bst.Insert(new TestData(39));
            bst.Insert(new TestData(39));
            bst.Insert(new TestData(39));
            if (bst.NodeCount != 31)
            {
                Assert.Fail("Incorect node count");
            }
        }

        [TestMethod]
        public void FindTest()
        {
            BinarySearchTree<TestData> bst = new BinarySearchTree<TestData>();
            bst.Insert(new TestData(20));
            bst.Insert(new TestData(10));
            bst.Insert(new TestData(5));
            bst.Insert(new TestData(3));
            bst.Insert(new TestData(1));
            bst.Insert(new TestData(4));
            bst.Insert(new TestData(7));
            bst.Insert(new TestData(6));
            bst.Insert(new TestData(9));
            bst.Insert(new TestData(15));
            bst.Insert(new TestData(13));
            bst.Insert(new TestData(11));
            bst.Insert(new TestData(14));
            bst.Insert(new TestData(17));
            bst.Insert(new TestData(16));
            bst.Insert(new TestData(19));
            bst.Insert(new TestData(30));
            bst.Insert(new TestData(25));
            bst.Insert(new TestData(23));
            bst.Insert(new TestData(21, "TestMessage"));
            bst.Insert(new TestData(24));
            bst.Insert(new TestData(27));
            bst.Insert(new TestData(26));
            bst.Insert(new TestData(29));
            bst.Insert(new TestData(35));
            bst.Insert(new TestData(33));
            bst.Insert(new TestData(31));
            bst.Insert(new TestData(34));
            bst.Insert(new TestData(37));
            bst.Insert(new TestData(36));
            bst.Insert(new TestData(39));

            if (bst.Find(new TestData(21)).Message != "TestMessage")
            {
                Assert.Fail("Node of message not found");
            }

            if (bst.Find(new TestData(22)) != null || (new TestData(21)).Message == "IncorrectMessage")
            {
                Assert.Fail("Bad node found");
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            BinarySearchTree<TestData> bst = new BinarySearchTree<TestData>();
            bst.Insert(new TestData(20));
            bst.Insert(new TestData(10));
            bst.Insert(new TestData(5));
            bst.Insert(new TestData(3));
            bst.Insert(new TestData(1));
            bst.Insert(new TestData(4));
            bst.Insert(new TestData(7));
            bst.Insert(new TestData(6));
            bst.Insert(new TestData(9));
            bst.Insert(new TestData(15));
            bst.Insert(new TestData(13));
            bst.Insert(new TestData(11));
            bst.Insert(new TestData(14));
            bst.Insert(new TestData(17));
            bst.Insert(new TestData(16));
            bst.Insert(new TestData(19));
            bst.Insert(new TestData(30));
            bst.Insert(new TestData(25));
            bst.Insert(new TestData(23));
            bst.Insert(new TestData(21, "TestMessage"));
            bst.Insert(new TestData(24));
            bst.Insert(new TestData(27));
            bst.Insert(new TestData(26));
            bst.Insert(new TestData(29));
            bst.Insert(new TestData(35));
            bst.Insert(new TestData(33));
            bst.Insert(new TestData(31));
            bst.Insert(new TestData(34));
            bst.Insert(new TestData(37));
            bst.Insert(new TestData(36));
            bst.Insert(new TestData(39));

            bst.Delete(new TestData(21));
            bst.Delete(new TestData(20));
            bst.Delete(new TestData(9));

            if (bst.NodeCount != 28)
            {
                Assert.Fail("Incorect node count");
            }

            if (bst.Find(new TestData(21)) != null || bst.Find(new TestData(20)) != null)
            {
                Assert.Fail("Found deleted data");
            }
        }

        [TestMethod]
        public void TraversalInOrderTest()
        {
            BinarySearchTree<TestData> bst = new BinarySearchTree<TestData>();
            bst.Insert(new TestData(20));
            bst.Insert(new TestData(10));
            bst.Insert(new TestData(5));
            bst.Insert(new TestData(3));
            bst.Insert(new TestData(1));
            bst.Insert(new TestData(4));
            bst.Insert(new TestData(7));
            bst.Insert(new TestData(6));
            bst.Insert(new TestData(9));
            bst.Insert(new TestData(15));
            bst.Insert(new TestData(13));
            bst.Insert(new TestData(11));
            bst.Insert(new TestData(14));
            bst.Insert(new TestData(17));
            bst.Insert(new TestData(16));
            bst.Insert(new TestData(19));
            bst.Insert(new TestData(30));
            bst.Insert(new TestData(25));
            bst.Insert(new TestData(23));
            bst.Insert(new TestData(21));
            bst.Insert(new TestData(24));
            bst.Insert(new TestData(27));
            bst.Insert(new TestData(26));
            bst.Insert(new TestData(29));
            bst.Insert(new TestData(35));
            bst.Insert(new TestData(33));
            bst.Insert(new TestData(31));
            bst.Insert(new TestData(34));
            bst.Insert(new TestData(37));
            bst.Insert(new TestData(36));
            bst.Insert(new TestData(39));
            bst.Insert(new TestData(39));
            bst.Insert(new TestData(39));
            int[] keyInOrder = new int[]
            {
                1,3,4,5,6,7,9,10,11,13,14,15,16,17,19,20,21,23,24,25,26,27,29,
                30,31,33,34,35,36,37,39
            };
            List<TestData> treeInOrder = bst.TraversalInOrder().ToList();
            for (int i = 0; i < bst.NodeCount; i++)
            {
                if (keyInOrder[i] != treeInOrder[i].Key)
                {
                    Assert.Fail("Expected " + keyInOrder[i] + ", found " + treeInOrder[i].Key);
                }
            }
        }
        [TestMethod]
        public void TraversalLevelOrderTest()
        {
            BinarySearchTree<TestData> bst = new BinarySearchTree<TestData>();
            bst.Insert(new TestData(20));
            bst.Insert(new TestData(10));
            bst.Insert(new TestData(5));
            bst.Insert(new TestData(3));
            bst.Insert(new TestData(1));
            bst.Insert(new TestData(4));
            bst.Insert(new TestData(7));
            bst.Insert(new TestData(6));
            bst.Insert(new TestData(9));
            bst.Insert(new TestData(15));
            bst.Insert(new TestData(13));
            bst.Insert(new TestData(11));
            bst.Insert(new TestData(14));
            bst.Insert(new TestData(17));
            bst.Insert(new TestData(16));
            bst.Insert(new TestData(19));
            bst.Insert(new TestData(30));
            bst.Insert(new TestData(25));
            bst.Insert(new TestData(23));
            bst.Insert(new TestData(21));
            bst.Insert(new TestData(24));
            bst.Insert(new TestData(27));
            bst.Insert(new TestData(26));
            bst.Insert(new TestData(29));
            bst.Insert(new TestData(35));
            bst.Insert(new TestData(33));
            bst.Insert(new TestData(31));
            bst.Insert(new TestData(34));
            bst.Insert(new TestData(37));
            bst.Insert(new TestData(36));
            bst.Insert(new TestData(39));
            bst.Insert(new TestData(39));
            bst.Insert(new TestData(39));
            int[] keyInOrder = new int[]
            {
                20,10,30,5,15,25,35,3,7,13,17,23,27,33,37,
                1,4,6,9,11,14,16,19,21,24,26,29,31,34,36,39
            };
            List<TestData> treeLevelOrder = bst.TraversalLevelOrder().ToList();
            for (int i = 0; i < bst.NodeCount; i++)
            {
                if (keyInOrder[i] != treeLevelOrder[i].Key)
                {
                    Assert.Fail("Expected " + keyInOrder[i] + ", found " + treeLevelOrder[i].Key);
                }
            }
        }
    }
}
