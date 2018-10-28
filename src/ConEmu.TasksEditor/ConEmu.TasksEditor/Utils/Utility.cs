using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace ConEmu.TasksEditor.Utils
{
    public static class Utility
    {
        public static IEnumerable<XmlNode> Where(this XmlNode src, Func<XmlNode, bool> predicate)
        {
            List<XmlNode> list = new List<XmlNode>();
            src.ForEach(n =>
            {
                if (predicate.Invoke(n))
                    list.Add(n);
            });

            return list;
        }

        public static void ForEach(this XmlNode src, Action<XmlNode> action)
        {
            foreach (XmlNode child in src)
                action.Invoke(child);
        }

        public static int ToInt32(this string src)
        {
            Debug.Assert(Int32.TryParse(src, out int result), "Failed to convert string to Int32.");
            return result;
        }
    }
}