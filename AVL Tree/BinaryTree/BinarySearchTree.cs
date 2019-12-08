using System.Collections.Generic;

namespace AVLTree.BinaryTree
{
    /// <summary>
    /// Implementation of the binary ,a tree data structure in which each node has at most two children,
    /// which are referred to as the left child and the right child.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class BinarySearchTree<T> where T : IData
    {
        /// <summary>
        /// Get how many elemets tree contains
        /// </summary>
        public int NodeCount { get; protected set; } = 0;

        /// <summary>
        /// Starting element of the tree
        /// </summary>
        public Node<T> Root { get; set; }

        /// <summary>
        /// Posible comparator used for comparing elements of tree
        /// </summary>
        protected IComparer<T> comparator;

        public BinarySearchTree()
        {
        }

        /// <summary>
        /// Allow to use custom comparer instead of default one
        /// </summary>
        /// <param name="comparator"></param>
        public BinarySearchTree(IComparer<T> comparator)
        {
            this.comparator = comparator;
        }

        public bool IsEmpty()
        {
            return NodeCount == 0;
        }

        /// <summary>
        /// Insert data to the tree
        /// </summary>
        /// <param name="data">Unique data to be inserted</param>
        /// <returns>If comparator found same data in tree it wont insert to tree and
        /// return false, else insert data to the tree and return true</returns>
        public virtual bool Insert(T data)
        {
             return comparator==null?
                 Insert(new Node<T>(data)):
                 Insert(new Node<T>(data,comparator));
            
        }

        /// <summary>
        /// If tree doesnt contain node with same unique identifier, insert node to the tree
        /// </summary>
        /// <param name="node">Node with unique data to be inserted</param>
        /// <returns>False if comparator found node with same unique identifier,
        /// true otherwise</returns>
        protected virtual bool Insert(Node<T> node)
        {
            bool repeat = true;
            if (Root == null)
            {
                Root = node;
                NodeCount++;
                return true;
            }
            Node<T> actual = Root;
            while (repeat)
            {
                if (node < actual)
                {
                    if (actual.Left == null)
                    {
                        actual.Left = node;
                        actual.Left.Parent = actual;
                        NodeCount++;
                        repeat = false;
                    }
                    else actual = actual.Left;

                }
                else if (node > actual)
                {
                    if (actual.Right == null)
                    {
                        actual.Right = node;
                        actual.Right.Parent = actual;
                        NodeCount++;
                        repeat = false;
                    }
                    else actual = actual.Right;
                }
                else
                {
                    //key already exist
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Find data in the tree
        /// </summary>
        /// <param name="data">Data with same unique identifier to be found</param>
        /// <returns>Data that tree contains, with same unique identifier</returns>
        public virtual T Find(T data)
        {
            Node<T> foundNode = FindNode(new Node<T>(data));
            if (foundNode == null)
                //default(T) == null for reference types
                return default(T);
            else
                return foundNode.Data;
        }

        /// <summary>
        /// Find node in the tree
        /// </summary>
        /// <param name="node">Node with same unique data identifier to be found</param>
        /// <returns>Node that tree contains, with same unique identifier</returns>
        protected virtual Node<T> FindNode(Node<T> node)
        {
            Node<T> actual = Root;
            while (true)
            {
                if (actual != null)
                {
                    if (node < actual)
                    {
                        actual = actual.Left;
                    }
                    else if (node > actual)
                    {
                        actual = actual.Right;
                    }
                    else
                    {
                        return actual;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Find and delete data from tree
        /// </summary>
        /// <param name="data">Data with same unique identifier to be deleted</param>
        /// <returns>True if data was deleted, false if unique identifier was not found in the tree</returns>
        public virtual bool Delete(T data)
        {
            return Delete(new Node<T>(data)) != null;
        }

        /// <summary>
        /// Find and delete node from the tree
        /// </summary>
        /// <param name="node">Node with same unique data identifier to be deleted</param>
        /// <returns>True if node was deleted, false if unique data identifier was not found in the tree</returns>
        protected virtual Node<T> Delete(Node<T> node)
        {
            Node<T> deleteNode = FindNode(node);
            if (deleteNode != null)
            {
                //only true if both null, is leaf
                if (deleteNode.Left == deleteNode.Right)
                {
                    ChangeNode(deleteNode, null);
                }
                else if (deleteNode.Right == null)
                {
                    ChangeNode(deleteNode, deleteNode.Left);
                }
                else if (deleteNode.Left == null)
                {
                    ChangeNode(deleteNode, deleteNode.Right);
                }
                //change found node, to rightmost node of left subtree
                else if (deleteNode.Left != null && deleteNode.Right != null)
                {
                    Node<T> leftRightMost = deleteNode.Left;
                    Node<T> leftRightMostParent = deleteNode;
                    //go to rightmost node of left subtree
                    while (leftRightMost.Right != null)
                    {
                        leftRightMostParent = leftRightMost;
                        leftRightMost = leftRightMost.Right;
                    }

                    //if there was right subtree of left subtree change parent of rightmost node
                    if (leftRightMostParent != deleteNode)
                    {
                        leftRightMostParent.Right = null;
                        leftRightMost.Left = deleteNode.Left;
                        leftRightMost.Left.Parent = leftRightMost;
                    }
                    //left subtree don't have right node/tree
                    else
                    {
                        // leftRightMost.Left = null;
                    }

                    leftRightMost.Right = deleteNode.Right;
                    ChangeNode(deleteNode, leftRightMost);
                }
                NodeCount--;
            }
            return deleteNode;
        }

        /// <summary>
        /// Find and delete node from the tree
        /// </summary>
        /// <param name="node">Node with same unique data identifier to be deleted</param>
        /// <param name="oLeftRightMostParent">Parent of left rightmost element of the tree</param>
        /// <returns>True if node was deleted, false if unique data identifier was not found in the tree</returns>
        protected virtual Node<T> Delete(Node<T> node, out Node<T> oLeftRightMostParent)
        {
            oLeftRightMostParent = null;
            Node<T> deleteNode = FindNode(node);
            if (deleteNode != null)
            {
                if (deleteNode.Left == deleteNode.Right)
                {
                    ChangeNode(deleteNode, null);
                }
                else if (deleteNode.Right == null)
                {
                    ChangeNode(deleteNode, deleteNode.Left);
                }
                else if (deleteNode.Left == null)
                {
                    ChangeNode(deleteNode, deleteNode.Right);
                }
                //change found node, to rightmost node of left subtree
                else if (deleteNode.Left != null && deleteNode.Right != null)
                {
                    Node<T> leftRightMost = deleteNode.Left;
                    Node<T> leftRightMostParent = deleteNode;
                    //go to rightmost node of left subtree
                    while (leftRightMost.Right != null)
                    {
                        leftRightMostParent = leftRightMost;
                        leftRightMost = leftRightMost.Right;
                    }

                    //if there is right subtree of left subtree change parent of rightmost node
                    if (leftRightMostParent != deleteNode)
                    {
                        leftRightMostParent.Right = leftRightMost.Left;
                        if (leftRightMostParent.Right != null)
                            leftRightMostParent.Right.Parent = leftRightMostParent;
                        leftRightMost.Left = deleteNode.Left;
                        leftRightMost.Left.Parent = leftRightMost;
                            oLeftRightMostParent = leftRightMostParent;
                       /*if (leftRightMostParent.Left != null)
                            oLeftRightMostParent = leftRightMostParent.Left;*/
                    }
                    else
                    {
                        //if there isn't right subtree of left subtree recalculate height from left-right most
                        oLeftRightMostParent = leftRightMost;
                    }
                    ;
                    leftRightMost.Right = deleteNode.Right;
                    leftRightMost.Right.Parent = leftRightMost;
                    ChangeNode(deleteNode, leftRightMost);
                }
                NodeCount--;
            }
            return deleteNode;
        }

        /// <summary>
        /// Traverse through the tree from leftmost element(leaf) to rightmost element(leaf) and save path to the list
        /// </summary>
        /// <returns>List of the elements from lowest to highest comparator value</returns>
        public LinkedList<T> TraversalInOrder()
        {
            LinkedList<T> nodes = new LinkedList<T>();
            Stack<Node<T>> parents = new Stack<Node<T>>();
            if (Root != null)
            {
                Node<T> curNode = Root;
                while (curNode != null || parents.Count > 0)
                {
                    while (curNode != null)
                    {
                        parents.Push(curNode);
                        curNode = curNode.Left;
                    }

                    curNode = parents.Pop();
                    nodes.AddLast(curNode.Data);
                    curNode = curNode.Right;
                }
            }
            return nodes;
        }

        /// <summary>
        /// Traverse the tree in levels from top to bottom from left to right
        /// </summary>
        /// <returns>List ordered by levels of the tree</returns>
        public LinkedList<T> TraversalLevelOrder()
        {
            LinkedList<T> levelOrder = new LinkedList<T>();
            Node<T> current = Root;
            Queue<Node<T>> list = new Queue<Node<T>>();
            list.Enqueue(current);
            while (list.Count > 0)
            {
                current = list.Dequeue();
                levelOrder.AddLast(current.Data);
                if (current.Left != null)
                    list.Enqueue(current.Left);
                if (current.Right != null)
                    list.Enqueue(current.Right);
            }

            return levelOrder;
        }
        
        /// <summary>
        /// change subtree/node of parent to newNode base on actualNode(subtree/node of parent)
        /// </summary>
        /// <param name="actualNode"></param>
        /// <param name="newNode"></param>
        private void ChangeNode(Node<T> actualNode, Node<T> newNode)
        {
            if (actualNode.Parent == null)
            {
                Root = newNode;
                if (newNode != null)
                    Root.Parent = null;
            }
            else if (actualNode == actualNode.Parent.Left)
            {
                actualNode.Parent.Left = newNode;
                if (newNode != null)
                    newNode.Parent = actualNode.Parent;
            }
            else if (actualNode == actualNode.Parent.Right)
            {
                actualNode.Parent.Right = newNode;
                if (newNode != null)
                    newNode.Parent = actualNode.Parent;
            }
        }
    }
}

