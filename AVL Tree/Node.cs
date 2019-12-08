using System;
using System.Collections.Generic;

namespace AVL_Tree.Binary_Tree
{
    //T is type of data, to make different datasets nonexchangeable
    //V is type of data key
    class Node<T> : IComparable<Node<T>> where T : IData
    {
        public T Data { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public Node<T> Parent { get; set; }
        private IComparer<T> comparer;

        public Node(T pData)
        {
            Data = pData;
        }

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
