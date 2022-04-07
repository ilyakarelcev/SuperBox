using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Tools.CepheiExtantions.Tools
{
    public static class LinkedListExtantion
    {
        public static void ByPass<T>(this LinkedList<T> list, Action<T> action)
        {
            LinkedListNode<T> node = list.First;
            while (node != null)
            {
                action.Invoke(node.Value);
                node = node.Next;
            }
        }
    }
}
