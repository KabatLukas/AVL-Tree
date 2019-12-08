using System;

namespace AVLTree.BinaryTree
{
    /// <summary>
    /// Represent comparable data usable in the tree structure
    /// </summary>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public interface IData:IComparable<IData>
    {
    }
}
