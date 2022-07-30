using Core.Interfaces;
using System.Collections.Generic;

namespace StorageFile.Extensions
{
    internal static class ListExtension
    {
        public static int GetHash<T>(this List<T> list) where T : IHashable
        {
            string hashs = "";

            foreach (T item in list)
                hashs += item.GetHashCode().ToString();

            return hashs.GetHashCode();
        }
    }
}
