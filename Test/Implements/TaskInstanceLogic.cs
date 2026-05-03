using Core.Interfaces.Storage;
using Core.Models.Storage;

namespace Test.Implements
{
    internal class TaskInstanceLogic : ITaskInstanceLogic
    {
        private List<TaskInstance> _models = new List<TaskInstance>();

        public void Create(TaskInstance model)
        {
            model.Id = GetNextId();
            _models.Add(new TaskInstance
            {
                Id = model.Id,
                Date = model.Date,
                TaskId = model.TaskId,
                Completed = model.Completed,
                Comment = model.Comment
            });
        }

        public void Create(List<TaskInstance> models)
        {
            foreach (TaskInstance model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                    model.Id = GetNextId();

                _models.Add(new TaskInstance
                {
                     Id = model.Id,
                     Date = model.Date,
                     TaskId = model.TaskId,
                     Completed = model.Completed,
                     Comment = model.Comment
                });
            }
        }

        public List<TaskInstance> Read(DateTime date)
        {
            List<TaskInstance> instances =
                _models
                .Where(req => req.Date.Date == date.Date)
                .Select(req => new TaskInstance
                {
                    Id = req.Id,
                    Date = req.Date,
                    TaskId = req.TaskId,
                    Completed = req.Completed,
                    Comment = req.Comment
                })
                .ToList();

            return instances;
        }

        public List<TaskInstance> Read(string taskId)
        {
            return _models
                .Where(req => req.TaskId == taskId)
                .Select(req => new TaskInstance
                {
                    Id = req.Id,
                    Date = req.Date,
                    TaskId = req.TaskId,
                    Completed = req.Completed,
                    Comment = req.Comment
                })
                .ToList();
        }

        public void Update(TaskInstance model)
        {
            TaskInstance instance = _models.FirstOrDefault(req => req.Id == model.Id);

            //if (instance == null)
            //    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {model.Id}.");

            instance.Date = model.Date;
            instance.TaskId = model.TaskId;
            instance.Completed = model.Completed;
            instance.Comment = model.Comment;
        }

        public void Update(List<TaskInstance> models)
        {
            foreach (TaskInstance model in models)
            {
                TaskInstance instance = models.FirstOrDefault(req => req.Id == model.Id);

                //if (instance == null)
                //    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {instance.Id}.");

                instance.Date = model.Date;
                instance.TaskId = model.TaskId;
                instance.Completed = model.Completed;
                instance.Comment = model.Comment;
            }
        }

        public void Delete()
        {
            _models.Clear();
        }

        public void Delete(string id)
        {
            TaskInstance instance = _models.FirstOrDefault(req => req.Id == id);

            //if (instance == null)
            //    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {id}.");

            _models.Remove(instance);
        }

        public void Delete(List<string> ids)
        {
            IEnumerable<TaskInstance> instances = _models.Where(req => ids.Contains(req.Id)).ToList();

            foreach (TaskInstance instance in instances)
                _models.Remove(instance);
        }

        private string GetNextId()
        {
            return
                _models.Count == 
                    0
                ?
                    "1"
                :
                    (_models.Select(x => int.Parse(x.Id)).Max() + 1).ToString();
        }
    }
}
