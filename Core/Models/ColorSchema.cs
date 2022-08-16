using System.Collections.Generic;

namespace Core.Models
{
    public class ColorSchema
    {
        public Dictionary<string, string> Colors { get; set; }

        public ColorSchema()
        {
            Colors = new Dictionary<string, string>();
        }

        public bool IsColorSchemaExist(List<string> keys)
        {
            foreach (string key in keys)
                if (!Colors.ContainsKey(key))
                    return false;

            return true;
        }
    }
}
