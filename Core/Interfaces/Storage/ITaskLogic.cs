using Core.Models.Storage;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskLogic"]/ITaskLogic/*'/>
    public interface ITaskLogic
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskLogic"]/Create/*'/>
        void Create(Task model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskLogic"]/CreateList/*'/>
        void Create(List<Task> models);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskLogic"]/Read/*'/>
        List<Task> Read();
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskLogic"]/ReadOne/*'/>
        Task Read(string id);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskLogic"]/Update/*'/>
        void Update(Task model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskLogic"]/Delete/*'/>
        void Delete(string id);
    }
}
