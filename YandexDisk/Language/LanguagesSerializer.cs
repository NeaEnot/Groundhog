using Newtonsoft.Json;
using System.IO;
using System.Linq;

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

        internal static void Deserialize(DirectoryInfo dir, string json)
        {
            FileInfo[] files = dir.GetFiles("*.lng");
            LanguagesList languagesList = JsonConvert.DeserializeObject< LanguagesList>(json);

            foreach (string lang in languagesList.Langs.Keys)
            {
                if (files.Count(req => req.Name == lang) == 0)
                    File.Create(dir.FullName+ "/" + lang).Close();

                File.WriteAllText(dir.FullName + "/" + lang, languagesList.Langs[lang]);
            }
        }
    }
}
