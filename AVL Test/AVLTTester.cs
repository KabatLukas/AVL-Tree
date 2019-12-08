using System;
using System.Collections.Generic;
using AVLTree;
using AVLTree.BinaryTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BSTTest
{
    [TestClass]
    public class AVLTTester
    {
        class AVLData : IData
        {
            public int Key { get; }

            public AVLData(int key)
            {
                Key = key;
            }
            public int CompareTo(IData other)
            {
                if (Key < ((AVLData)other).Key)
                    return -1;
                else if (Key > ((AVLData)other).Key)
                    return 1;
                else
                    return 0;
            }
        }

        private readonly Random rng;
        private int seed;
        private int NumOFOperations { get; set; }
        private List<int> keyList;
        private AVLTree<AVLData> avlTree = new AVLTree<AVLData>();
        private List<double> operationsProb = new List<double>(2);
        private bool perOperationCheck;


        public AVLTTester(): this(300000, 80, 20, 40)
        {
            perOperationCheck = false;
            Generate();
        }

        public AVLTTester(int numOfOperations, int insertProb, int deleteProb, int findProb):
            this(numOfOperations, insertProb, deleteProb, findProb, new Random().Next())
        {
        }

        public AVLTTester(int numOfOperations, int insertProb, int deleteProb, int findProb, int seed)
        {
            this.seed = seed;
            rng = new Random(seed);
            NumOFOperations = numOfOperations;
            keyList = new List<int>(numOfOperations);
            operationsProb.Add((double)insertProb / (insertProb + deleteProb + findProb));
            operationsProb.Add((double)deleteProb / (insertProb + deleteProb + findProb));
        }
        
        [TestMethod]
        public void Generate()
        {
            avlTree = new AVLTree<AVLData>();
            int numberOfOperations = keyList.Count;
            keyList = new List<int>(numberOfOperations);
            int collisionCount = 0;
            for (int i = 0; i < NumOFOperations; i++)
            {
                double operationNum = rng.NextDouble();
                if (operationNum < operationsProb[0])
                {
                    int addKey = rng.Next(int.MinValue, int.MaxValue);
                    if (!avlTree.Insert(new AVLData(addKey)))
                    {
                        collisionCount++;
                    }
                    else
                    {
                        keyList.Add(addKey);
                    }

                    if (perOperationCheck)
                    {
                        InOrderCheck();
                        HeightBalanceCheck();
                    }
                }
                else if (operationNum < operationsProb[0] + operationsProb[1])
                {
                    if (keyList.Count > 0)
                    {
                        int deleteKey = keyList[rng.Next(keyList.Count - 1)];
                        if (!avlTree.Delete(new AVLData(deleteKey)))
                        {
                            Assert.Fail("Key to delete not found: " + deleteKey);
                        }
                        keyList.Remove(deleteKey);
                        if (perOperationCheck)
                        {
                            InOrderCheck();
                            HeightBalanceCheck();
                        }
                    }
                }
                else
                {
                    if (keyList.Count > 0)
                    {
                        int key = keyList[rng.Next(keyList.Count - 1)];
                        AVLData data = avlTree.Find(new AVLData(key));
                        if (perOperationCheck)
                        {
                            LinkedList<AVLData> inOrder = avlTree.TraversalInOrder();
                            if (!inOrder.Contains(data))
                            {
                                Assert.Fail("Key not found: " + key);
                            }
                        }
                    }
                }
            }

        }

        [TestMethod]
        public void InOrderCheck()
        {
            LinkedList<AVLData> inOrder = avlTree.TraversalInOrder();
            foreach (int key in keyList)
            {
                AVLData data = avlTree.Find(new AVLData(key));
                if (!inOrder.Contains(data))
                {
                    Assert.Fail("Key not found: " + key);
                }
            }
        }

        [TestMethod]
        public void LevelOrderCheck()
        {
            LinkedList<AVLData> levelOrder = avlTree.TraversalLevelOrder();
            foreach (int key in keyList)
            {
                AVLData data = avlTree.Find(new AVLData(key));
                if (!levelOrder.Contains(data))
                {
                    Assert.Fail("Key not found: " + key);
                }
            }
        }

        [TestMethod]
        public void HeightBalanceCheck()
        {
            Stack<Node<AVLData>> parents = new Stack<Node<AVLData>>();
            if (avlTree.Root != null)
            {
                Node<AVLData> curNode = avlTree.Root;
                while (curNode != null || parents.Count > 0)
                {
                    while (curNode != null)
                    {
                        parents.Push(curNode);
                        curNode = curNode.Left;
                    }

                    curNode = parents.Pop();
                    int height = Math.Max(((NodeAVL<AVLData>)curNode?.Right)?.Height ?? 0,
                                     ((NodeAVL<AVLData>)curNode?.Left)?.Height ?? 0) + 1;
                    int balance = (((NodeAVL<AVLData>)curNode.Left)?.Height ?? 0) -
                                  (((NodeAVL<AVLData>)curNode.Right)?.Height ?? 0);
                    if (((NodeAVL<AVLData>)curNode).Height != height)
                    {
                        Assert.Fail("Incorrect height at key: " + curNode.Data.Key);
                    }
                    if (balance < -1 || balance > 1)
                    {
                        Assert.Fail("Incorrect balance at key: " + curNode.Data.Key);
                    }
                    curNode = curNode.Right;
                }
            }
        }
    }
}
