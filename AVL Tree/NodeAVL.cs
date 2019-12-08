using AVLTree.BinaryTree;
using System.Collections.Generic;

namespace AVLTree
{
    /// <summary>
    /// Represent one branch in the AVL Tree
    /// </summary>
    /// <typeparam name="T">Class type of the all data stored in the tree, used to sort the tree</typeparam>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class NodeAVL<T> : Node<T> where T : IData
    {
        /// <summary>
        /// Get at height of the node
        /// </summary>
        public int Height { get; set; }


        /// <param name="pData">Unique data which node will contains</param>
        public NodeAVL(T pData) : base(pData)
        {
        }


        /// <param name="pData">Unique data which node will contains</param>
        /// <param name="comparer">Custom comparer which will override default one</param>
        public NodeAVL(T pData,IComparer<T> comparer) : base(pData, comparer)
        {
        }

        /// <summary>
        /// Calculate balance factor based on children as diference of left and right child
        /// </summary>
        /// <returns>Diference of height of left and right child</returns>
        public int GetBalanceFactor()
        {
            return (((NodeAVL<T>)Right)?.Height ?? 0) - (((NodeAVL<T>)Left)?.Height ?? 0);
        }
    }
}
