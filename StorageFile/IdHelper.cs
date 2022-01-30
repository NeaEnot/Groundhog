using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        internal static string GetId()
        {
            string answer = "";
            int i = 2;
            Random random = new Random();

            while (true)
            {
                StringBuilder builder = new StringBuilder();

                alphabet
                    .ToArray()
                    .OrderBy(e => Guid.NewGuid())
                    .Take(random.Next(1, i))
                    .ToList().ForEach(e => builder.Append(e));

                answer = builder.ToString();

                if (Context.Instanse.TaskInstances.Count(req => req.Id.Contains(answer)) == 0 &&
                    Context.Instanse.Tasks.Count(req => req.Id.Contains(answer)) == 0)
                    break;

                i++;
            }

            return answer;
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
