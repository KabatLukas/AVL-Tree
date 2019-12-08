using System;
using System.Collections.Generic;

namespace AVLTree.BinaryTree
{
    /// <summary>
    /// Represent one branch in the binary tree
    /// </summary>
    /// <typeparam name="T">Class type of the all data stored in the tree, used to sort the tree</typeparam>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class Node<T> : IComparable<Node<T>> where T : IData
    {
        /// <summary>
        /// Unique data which node contains
        /// </summary>
        public T Data { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public Node<T> Parent { get; set; }

        /// <summary>
        /// Comparator which node can use during sorting
        /// </summary>
        private IComparer<T> comparer;

        /// <param name="pData">Unique data which node will contains</param>
        public Node(T pData)
        {
            Data = pData;
        }

        /// <param name="pData">Unique data which node will contains</param>
        /// <param name="comparer">Custom comparer which will override default</param>
        public Node(T pData,IComparer<T> comparer)
        {
            Data = pData;
            this.comparer = comparer;
        }

        public int CompareTo(Node<T> other)
        {
            return comparer?.Compare(Data, other.Data)
                   ?? Data.CompareTo(other.Data);
        }
        public static bool operator <(Node<T> first, Node<T> second)
        {
            return first.CompareTo(second) < 0;
        }

        public static bool operator >(Node<T> first, Node<T> second)
        {
            return first.CompareTo(second) > 0;
        }
    }
}
