using System.Collections.Generic;

namespace Core.Models.Settings
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ColorSchema"]/ColorSchema/*'/>
    public class ColorSchema
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ColorSchema"]/Colors/*'/>
        public Dictionary<string, string> Colors { get; set; }

        public ColorSchema()
        {
            Colors = new Dictionary<string, string>();
        }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ColorSchema"]/ColorSchemaAbsent/*'/>
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
