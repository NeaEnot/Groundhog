using Core.Models.Storage;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INoteLogic"]/INoteLogic/*'/>
    public interface INoteLogic
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INoteLogic"]/Create/*'/>
        void Create(Note model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INoteLogic"]/CreateList/*'/>
        void Create(List<Note> models);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INoteLogic"]/Read/*'/>
        List<Note> Read();
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INoteLogic"]/Update/*'/>
        void Update(Note model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INoteLogic"]/Delete/*'/>
        void Delete(string id);
    }
}
