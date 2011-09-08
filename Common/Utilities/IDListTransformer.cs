using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public class IDListTransformer<T>
    {
        public string TransformIDs(IEnumerable<T> entitiesCollection, Func<T, int> idExtractFunc)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var e in entitiesCollection)
            {
                int id = idExtractFunc(e);
                sb.Append(id);
                sb.Append(',');
            }

            string result = sb.ToString().TrimEnd(trimChars);

            return result;
        }

        public object[] TransformIDs(string idsList)
        {
            if (String.IsNullOrEmpty(idsList))
                return new object[] { };

            string[] idStrs = idsList.Split(trimChars);

            if (idStrs.Length == 0)
                return new object[] { };

            List<object> list = new List<object>();

            foreach (var idStr in idStrs)
            {
                list.Add(Convert.ToInt32(idStr));
            }

            return list.ToArray();
        }

        private static readonly char[] trimChars = new char[] { ',' };
    }
}
