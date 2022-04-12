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
    }
}
