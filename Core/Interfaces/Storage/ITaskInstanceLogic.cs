using Core.Models.Storage;
using System;
using System.Collections.Generic;

namespace Core.Interfaces.Storage
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/ITaskInstanceLogic/*'/>
    public interface ITaskInstanceLogic
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/Create/*'/>
        void Create(TaskInstance model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/CreateList/*'/>
        void Create(List<TaskInstance> models);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/ReadDate/*'/>
        List<TaskInstance> Read(DateTime date);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/ReadTaskId/*'/>
        List<TaskInstance> Read(string taskId);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/Update/*'/>
        void Update(TaskInstance model);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/UpdateList/*'/>
        void Update(List<TaskInstance> models);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/DeleteAll/*'/>
        void Delete();
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/Delete/*'/>
        void Delete(string id);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="ITaskInstanceLogic"]/DeleteList/*'/>
        void Delete(List<string> ids);
    }
}
