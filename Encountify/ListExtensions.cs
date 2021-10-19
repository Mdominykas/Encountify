using System;
using System.Collections.Generic;
using System.Text;

namespace Encountify
{
    public static class ListExtensions
    {
        public static void SortdescendingOrder<T>(this List<T> list)
        {
            list.Sort();
            list.Reverse();
        }
    }
}
