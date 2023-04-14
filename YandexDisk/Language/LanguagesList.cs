using System.Collections.Generic;

namespace YandexDisk.Language
{
    internal class LanguagesList
    {
        public Dictionary<string, string> Langs { get; set; }

        internal LanguagesList()
        {
            Langs = new Dictionary<string, string>();
        }
    }
}
