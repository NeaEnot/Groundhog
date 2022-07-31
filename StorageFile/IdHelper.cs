using System;
using System.Linq;
using System.Text;

namespace StorageFile
{
    internal static class IdHelper
    {
        private static string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        internal static string GetId(string prefix)
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

                if (IsUniq(prefix, answer))
                    break;

                i++;
            }

            return prefix + answer;
        }

        private static bool IsUniq(string prefix, string id)
        {
            switch (prefix)
            {
                case "ti_":
                    if (Context.Instanse.TaskInstances.Count(req => req.Id == prefix + id) == 0)
                        return true;
                    break;
                case "t_":
                    if (Context.Instanse.Tasks.Count(req => req.Id == prefix + id) == 0)
                        return true;
                    break;
                case "p_":
                    if (Context.Instanse.Purposes.Count(req => req.Id == prefix + id) == 0)
                        return true;
                    break;
                case "pg_":
                    if (Context.Instanse.PurposeGroups.Count(req => req.Id == prefix + id) == 0)
                        return true;
                    break;
                case "n_":
                    if (Context.Instanse.Notes.Count(req => req.Id == prefix + id) == 0)
                        return true;
                    break;
            }

            return false;
        }
    }
}
