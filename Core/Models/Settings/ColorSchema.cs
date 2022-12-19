using System.Collections.Generic;

namespace Core.Models.Settings
{
    public class ColorSchema
    {
        public Dictionary<string, string> Colors { get; set; }

        public ColorSchema()
        {
            Colors = new Dictionary<string, string>();
        }

        public List<string> ColorSchemaAbsent(List<string> keys)
        {
            List<string> absent = new List<string>();

            foreach (string key in keys)
                if (!Colors.ContainsKey(key))
                    absent.Add(key);

            return absent;
        }
    }
}
