using System.Collections.Generic;

namespace YandexDisk.Language
{
    internal class LaguagesList
    {
        public Dictionary<string, string> Langs { get; set; }

        public LaguagesList()
        {
            Langs = new Dictionary<string, string>();
        }
    }
}
