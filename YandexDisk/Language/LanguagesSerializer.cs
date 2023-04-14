using Newtonsoft.Json;
using System.IO;

namespace YandexDisk.Language
{
    internal static class LanguagesSerializer
    {
        internal static string Serialize(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles("*.lng");
            LanguagesList languagesList = new LanguagesList();
            
            foreach (FileInfo file in files)
            {
                string text = File.ReadAllText(file.FullName);
                languagesList.Langs.Add(file.Name, text);
            }

            string answer = JsonConvert.SerializeObject(languagesList);

            return answer;
        }
    }
}
