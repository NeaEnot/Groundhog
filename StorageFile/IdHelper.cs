using System.Collections.Generic;

namespace StorageFile
{
    internal static class IdHelper
    {
        private static string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        internal static string GetMaxId(List<string> ids)
        {
            string max = "";

            for (int i = 0; i < ids.Count; i++)
            {
                if (string.Compare(max, ids[i]) < 0)
                    max = ids[i];
            }

            return max;
        }

        internal static string GetNextId(string prevId)
        {
            string id = "";

            /* Надо ли увеличивать символ текущей позиции.
             * Так как начинаем смотреть с последней буквы, 
             * которую всегда надо увеличивать, инициализируем как true.*/
            bool isNeedIncrement = true;

            for (int i = prevId.Length - 1; i >= 0; i--)
            {
                if (isNeedIncrement)
                {
                    if (prevId[i] == alphabet[alphabet.Length - 1])
                    {
                        id = alphabet[0] + id;
                    }
                    else
                    {
                        int charIndex = alphabet.IndexOf(prevId[i]);
                        id = alphabet[charIndex + 1] + id;
                        isNeedIncrement = false;
                    }
                }
                else
                {
                    id = prevId[i] + id;
                }
            }

            if (isNeedIncrement)
                id = alphabet[0] + id;

            return id;
        }
    }
}
