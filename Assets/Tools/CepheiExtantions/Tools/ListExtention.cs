using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class ListExtention
    {
        public static void SmartDelete<T>(this List<T> ls, int index)
        {
            T lastElement = ls[ls.Count - 1];
            ls[ls.Count - 1] = ls[index];
            ls[index] = lastElement;
            ls.RemoveAt(ls.Count - 1);
        }
    }
}