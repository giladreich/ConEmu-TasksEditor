using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace ConEmu.TasksEditor.Utils
{
    public static class Utility
    {
        public static IEnumerable<T> Where<T>(this T src, Func<T, bool> predicate)
        {
            List<T> list = new List<T>();
            src.ForEach(n =>
            {
                if (predicate.Invoke(n))
                    list.Add(n);
            });

            return list;
        }

        public static void ForEach<T>(this T src, Action<T> action)
        {
            IEnumerable collection = (IEnumerable) src;
            foreach (T itr in collection)
                action.Invoke(itr);
        }

        public static void ForEach<T>(this T[] src, Action<T> action)
        {
            foreach (T itr in src)
                action.Invoke(itr);
        }

        public static int ToInt32(this string src)
        {
            Debug.Assert(Int32.TryParse(src, out int result), "Failed to convert string to Int32.");
            return result;
        }
    }
}