using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Extensions
{
    public static class EnumerableExtensions
    {

        public static string ToDisplayList(
            this IEnumerable<string> source,
            string header,
            string lineInitialzierChar = "-",
            bool tab = true)
        {
            return ToDisplayList(source, (item) => { return item; }, header, lineInitialzierChar, tab);
        }
        public static string ToDisplayList<T>(
            this IEnumerable<T> source, 
            Func<T, string> func, 
            string header, 
            string lineInitialzierChar = "-",
            bool tab = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);
            bool enumerated = false;
            if (lineInitialzierChar.ToLowerInvariant() == "i")
            {
                enumerated = true;
            }
            int counter = 1;
            foreach (var item in source)
            {
                StringBuilder sbLine = new StringBuilder(tab ? "\t" : "");
                sbLine.Append(enumerated ? $"{counter++.ToString()}- " : $"{lineInitialzierChar} ");
                sbLine.Append(func(item));
                sb.AppendLine(sbLine.ToString());
            }
            return sb.ToString();
        }


        public static IEnumerable<T> SkipLastCustom<T>(this IEnumerable<T> source,
        int count)
        {
            Queue<T> saveList = new Queue<T>();
            int saved = 0;
            foreach (T item in source)
            {
                if (saved < count)
                {
                    saveList.Enqueue(item);
                    ++saved;
                    continue;
                }
                saveList.Enqueue(item);
                yield return saveList.Dequeue();
            }
            yield break;
        }

        public static IEnumerable<T> TakeAllButFirst<T>(this IEnumerable<T> source)
        {
            var it = source.GetEnumerator();
            bool hasRemainingItems = false;
            bool isFirst = true;
            T item = default(T);

            do
            {
                hasRemainingItems = it.MoveNext();
                if (hasRemainingItems)
                {
                    if (!isFirst) yield return item;
                    item = it.Current;
                    isFirst = false;
                }
            } while (hasRemainingItems);
        }


        public static IEnumerable<T> TakeAllButLast<T>(this IEnumerable<T> source)
        {
            var it = source.GetEnumerator();
            bool hasRemainingItems = false;
            bool isFirst = true;
            T item = default(T);

            do
            {
                hasRemainingItems = it.MoveNext();
                if (hasRemainingItems)
                {
                    if (!isFirst) yield return item;
                    item = it.Current;
                    isFirst = false;
                }
            } while (hasRemainingItems);
        }

    }
}
