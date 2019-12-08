using System;
using System.Collections.Generic;
using AVLTree.BinaryTree;

namespace AVLTree
{
    /// <summary>
    /// Implementation of AVL tree where the heights of the two child subtrees of any node differ by at most one.
    /// If at any time they differ by more than one, rebalancing is done to restore this property.
    /// Lookup, insertion, and deletion all take O(log n) time in both the average and worst cases,
    /// where n is the number of nodes in the tree prior to the operation.
    /// Insertions and deletions may require the tree to be rebalanced by one or more tree rotations.
    /// </summary>
    /// <typeparam name="T">Class type which implements IData interface, used to sort data in the tree</typeparam>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class AVLTree<T> : BinarySearchTree<T> where T : IData
    {
#if DEBUG
        protected int recursionCounter = 0;
        protected int Lrotations = 0;
        protected int Rrotations = 0;
        protected int LRrotations = 0;
        protected int RLrotations = 0;
#endif
        
        public AVLTree()
        {
        }

        
        /// <param name="comparator">Overide default comparator for a tree nodes</param>
        public AVLTree(IComparer<T> comparator)
        {
            this.comparator = comparator;
        }

        /// <summary>
        /// If tree doesnt contain data with same unique identifier, insert node to the tree
        /// </summary>
        /// <param name="node">Data with unique identifier to be inserted</param>
        /// <returns>False if comparator found data with same unique identifier,
        /// true otherwise</returns>
        public override bool Insert(T data)
        {
            return comparator == null ?
                Insert(new NodeAVL<T>(data)) :
                Insert(new NodeAVL<T>(data, comparator));
        }

        /// <summary>
        /// If tree doesnt contain node with same unique identifier, insert node to the tree
        /// </summary>
        /// <param name="node">Node with unique identifier to be inserted</param>
        /// <returns>False if comparator found node with same unique identifier,
        /// true otherwise</returns>
        protected bool Insert(NodeAVL<T> node)
        {
            //Node already exist return false
            if (!base.Insert(node))
                return false;
            node.Height = 1;
            NodeAVL<T> current = (NodeAVL<T>)node.Parent;
            while (current != null)
            {
                RecalculateHeight(current);

                if (current.GetBalanceFactor() < -1)
                {
                    if (((NodeAVL<T>)current.Left)?.GetBalanceFactor() < 0)
                    {
                        RRotation(current);
                    }
                    else
                    {
                        LRRotation(current);
                    }

                    return true;
                }
                else if (current.GetBalanceFactor() > 1)
                {
                    if (((NodeAVL<T>)current.Right)?.GetBalanceFactor() > 0)
                    {
                        LRotation(current);
                    }
                    else
                    {
                        RLRotation(current);
                    }

                    return true;
                }
                if (current.GetBalanceFactor() == 0)
                    return true;

                current = (NodeAVL<T>)current.Parent;
            }

            return true;
        }

        /// <summary>
        /// Find and delete data from tree
        /// </summary>
        /// <param name="data">Data with same unique identifier to be deleted</param>
        /// <returns>True if data was deleted, false if unique identifier was not found in the tree</returns>
        public override bool Delete(T data)
        {
            return Delete(new NodeAVL<T>(data));
        }

        /// <summary>
        /// Find and delete node from the tree
        /// </summary>
        /// <param name="node">Node with same unique data identifier to be deleted</param>
        /// <returns>True if node was deleted, false if unique data identifier was not found in the tree</returns>
        protected bool Delete(NodeAVL<T> node)
        {
            Node<T> current;
            Node<T> deletedNode = base.Delete(node, out current);
            //Node not found
            if (deletedNode == null)
                return false;
            //delete node had less than 2 children
            if (current == null)
                current = deletedNode;
            else
                RecalculateHeight((NodeAVL<T>)current);
            while (current != null)
            {
                //current father no longer have pointer to current because it has been removed
                //if coming from right side, left side is getting bigger in height
                RebalanceNode(current);
#if DEBUG
                if (recursionCounter > 10)
                {
                    throw new Exception("Recursion called too much");
                }
                recursionCounter = 0;
#endif
                current = (NodeAVL<T>)current.Parent;
            }
            return true;
        }

        /// <summary>
        /// Perform right rotation of nodes
        /// </summary>
        /// <param name="around">Hihgest node to rotate around</param>
        /// <returns>Rebalanced node</returns>
        private Node<T> RRotation(NodeAVL<T> around)
        {
#if DEBUG
            Rrotations++;
#endif
            Node<T> N = around;
            Node<T> B = around.Left;
            //N is overweight node ,B is left child of overweight node
            //change parent of N(1)
            if (N.Parent == null) Root = B;
            else if (N < N.Parent) N.Parent.Left = B;
            else if (N > N.Parent) N.Parent.Right = B;
            //change B parent to N parent (5)
            B.Parent = N.Parent;
            //change right child of B to N (3)
            Node<T> tempNode = B.Right;
            B.Right = around;
            //change parent of N to B (6)
            N.Parent = B;
            //change N left child to right node of B (2)
            N.Left = tempNode;
            //change new N left child parent to N (4)
            if (N.Left != null)
                N.Left.Parent = N;

            RecalculateHeight((NodeAVL<T>)N);
            RecalculateHeight((NodeAVL<T>)B);

            return N;
        }
        
        /// <summary>
        /// Perform leftright rotation with owerweight node
        /// </summary>
        /// <param name="overweight"></param>
        /// <returns></returns>
        private Node<T> LRRotation(NodeAVL<T> overweight)
        {
#if DEBUG
            LRrotations++;
#endif
            Node<T> N = overweight;
            Node<T> B = overweight.Left;
            Node<T> F = overweight.Left.Right;
            //(1)
            if (N.Parent == null) Root = F;
            else if (N < N.Parent) N.Parent.Left = F;
            else if (N > N.Parent) N.Parent.Right = F;
            //(8)
            F.Parent = N.Parent;
            //(10)
            N.Parent = F;
            //(4) save T2
            Node<T> tempNode = F.Left;
            F.Left = B;
            //(9)
            B.Parent = F;
            //(3) 
            B.Right = tempNode;
            //(6)
            if (B.Right != null)
                B.Right.Parent = B;
            //(2) 
            N.Left = F.Right;
            //(7)
            if (N.Left != null)
                N.Left.Parent = N;
            //(5)
            F.Right = N;

            RecalculateHeight((NodeAVL<T>)N);
            RecalculateHeight((NodeAVL<T>)B);
            
            if (((NodeAVL<T>)B).GetBalanceFactor() < -1)
            {
                RebalanceNode((NodeAVL<T>)B);
#if DEBUG
                recursionCounter++;
#endif
            }

            RecalculateHeight((NodeAVL<T>)F);

            return N;
        }

        /// <summary>
        /// Perform left rotation of nodes
        /// </summary>
        /// <param name="around">Hihgest node to rotate around</param>
        /// <returns>Rebalanced node</returns>
        private Node<T> LRotation(NodeAVL<T> around)
        {

#if DEBUG
            Lrotations++;
#endif
            Node<T> N = around;
            Node<T> B = around.Right;
            //N is overweight node ,B is right child of overweight node
            //change parent of N(1)
            if (N.Parent == null) Root = B;
            else if (N < N.Parent) N.Parent.Left = B;
            else if (N > N.Parent) N.Parent.Right = B;
            //change B parent to N parent (5)
            B.Parent = N.Parent;
            //(3) save T2
            Node<T> tempNode = B.Left;
            B.Left = N;
            //change parent of N to B (6)
            N.Parent = B;
            //change right child of N to right node of B (2)
            N.Right = tempNode;
            //change new N right child parent to N (4)
            //T2 can be null
            if (N.Right != null)
                N.Right.Parent = N;

            RecalculateHeight((NodeAVL<T>)N);
            RecalculateHeight((NodeAVL<T>)B);

            return N;
        }

        /// <summary>
        /// Perform rightleft rotation with owerweght node
        /// </summary>
        /// <param name="overweight"></param>
        /// <returns>Rebalanced node</returns>
        private Node<T> RLRotation(NodeAVL<T> overweight)
        {

#if DEBUG
            RLrotations++;
#endif
            Node<T> N = overweight;
            Node<T> B = overweight.Right;
            Node<T> F = overweight.Right.Left;
            //(1)
            if (N.Parent == null) Root = F;
            else if (N < N.Parent) N.Parent.Left = F;
            else if (N > N.Parent) N.Parent.Right = F;
            //(8)
            F.Parent = N.Parent;
            //(10)
            N.Parent = F;
            //(5) save T3
            Node<T> tempNode = F.Right;
            F.Right = B;
            //(9)
            B.Parent = F;
            //(3)
            B.Left = tempNode;
            //(7) T3 don't necessary exist
            if (B.Left != null)
                B.Left.Parent = B;
            //(2) new parent
            N.Right = F.Left;
            //(6)  T2 don't necessary exist
            if (N.Right != null)
                N.Right.Parent = N;
            //(4) new parent
            F.Left = N;

            RecalculateHeight((NodeAVL<T>)N);
            RecalculateHeight((NodeAVL<T>)B);
            if (((NodeAVL<T>)B).GetBalanceFactor() > 1)
            {
                RebalanceNode((NodeAVL<T>)B);
#if DEBUG
                recursionCounter++;
#endif
            }
            RecalculateHeight((NodeAVL<T>)F);

            return N;
        }
        
        /// <summary>
        /// Calculate height of node in the tree based of heigh of children elements
        /// </summary>
        /// <param name="node">Node which height is recalculated</param>
        protected void RecalculateHeight(NodeAVL<T> node)
        {
            node.Height = Math.Max(((NodeAVL<T>)node?.Right)?.Height ?? 0, ((NodeAVL<T>)node?.Left)?.Height ?? 0) + 1;
        }

        /// <summary>
        /// Based on balance of the node perform balance correction with correct rotations
        /// </summary>
        /// <param name="pCurrent"></param>
        /// <returns>Node which was evaluated</returns>
        protected Node<T> RebalanceNode(Node<T> pCurrent)
        {
            Node<T> current = pCurrent;
            Node<T> tempNode;
            RecalculateHeight((NodeAVL<T>)current);
            if (((NodeAVL<T>)current).GetBalanceFactor() < -1)
            {
                if (((NodeAVL<T>)current.Left).GetBalanceFactor() < 0)
                {
                    tempNode = RRotation((NodeAVL<T>)current);
                    if (tempNode != null)
                        current = tempNode;
                }
                else
                {
                    tempNode = LRRotation((NodeAVL<T>)current);
                    if (tempNode != null)
                        current = tempNode;
                }

            }
            if (((NodeAVL<T>)current).GetBalanceFactor() > 1)
            {
                if (((NodeAVL<T>)current.Right).GetBalanceFactor() > 0)
                {
                    tempNode = LRotation((NodeAVL<T>)current);
                    if (tempNode != null)
                        current = tempNode;
                }
                else
                {
                    tempNode = RLRotation((NodeAVL<T>)current);
                    if (tempNode != null)
                        current = tempNode;
                }

            }

            return current;
        }
    }
}
