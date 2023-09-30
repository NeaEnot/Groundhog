using Core.Models.Storage;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/IPurposeLogic/*'/>
    public interface IPurposeLogic
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/Create/*'/>
        void Create(Purpose model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/CreateList/*'/>
        void Create(List<Purpose> models);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/Read/*'/>
        List<Purpose> Read(string groupId);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/Update/*'/>
        void Update(Purpose model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/DeleteAll/*'/>
        void Delete();
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/Delete/*'/>
        void Delete(string id);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeLogic"]/DeleteList/*'/>
        void Delete(List<string> ids);
    }
}
