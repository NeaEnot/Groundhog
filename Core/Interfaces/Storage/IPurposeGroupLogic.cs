using Core.Models.Storage;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeGroupLogic"]/IPurposeGroupLogic/*'/>
    public interface IPurposeGroupLogic
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeGroupLogic"]/Create/*'/>
        void Create(PurposeGroup model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeGroupLogic"]/CreateList/*'/>
        void Create(List<PurposeGroup> models);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeGroupLogic"]/Read/*'/>
        List<PurposeGroup> Read();
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeGroupLogic"]/Update/*'/>
        void Update(PurposeGroup model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="IPurposeGroupLogic"]/Delete/*'/>
        void Delete(string id);
    }
}
