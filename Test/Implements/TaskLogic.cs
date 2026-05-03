using Core;
using Core.Interfaces.Storage;
using Task = Core.Models.Storage.Task;

namespace Test.Implements
{
    internal class TaskLogic : ITaskLogic
    {
        private List<Task> _models = new List<Task>();

        public void Create(Task model)
        {
            model.Id = GetNextId();
            _models.Add(new Task
            {
                Id = model.Id,
                Text = model.Text,
                RepeatMode = model.RepeatMode,
                RepeatValue = model.RepeatValue,
                ToNextDay = model.ToNextDay,
                OffsetAll = model.OffsetAll,
                PlanningRange = model.PlanningRange,
                OptimizationRange = model.OptimizationRange
            });
        }

        public void Create(List<Task> models)
        {
            foreach (Task model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                    model.Id = GetNextId();

                _models.Add(new Task
                {
                    Id = model.Id,
                    Text = model.Text,
                    RepeatMode = model.RepeatMode,
                    RepeatValue = model.RepeatValue,
                    ToNextDay = model.ToNextDay,
                    OffsetAll = model.OffsetAll,
                    PlanningRange = model.PlanningRange,
                    OptimizationRange = model.OptimizationRange
                });
            }
        }

        public List<Task> Read()
        {
            return _models
                .Select(req => new Task
                {
                    Id = req.Id,
                    Text = req.Text,
                    RepeatMode = req.RepeatMode,
                    RepeatValue = req.RepeatValue,
                    ToNextDay = req.ToNextDay,
                    OffsetAll = req.OffsetAll,
                    PlanningRange = req.PlanningRange,
                    OptimizationRange = req.OptimizationRange
                })
                .ToList();
        }

        public Task Read(string id)
        {
            Task task = _models.FirstOrDefault(req => req.Id == id);

            //if (task == null)
            //    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {id}.");

            return new Task
            {
                Id = task.Id,
                Text = task.Text,
                RepeatMode = task.RepeatMode,
                RepeatValue = task.RepeatValue,
                ToNextDay = task.ToNextDay,
                OffsetAll = task.OffsetAll,
                PlanningRange = task.PlanningRange,
                OptimizationRange = task.OptimizationRange
            };
        }

        public void Update(Task model)
        {
            Task task = _models.FirstOrDefault(req => req.Id == model.Id);

            //if (task == null)
            //    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {model.Id}.");

            task.Text = model.Text;
            task.RepeatMode = model.RepeatMode;
            task.RepeatValue = model.RepeatValue;
            task.ToNextDay = model.ToNextDay;
            task.OffsetAll = model.OffsetAll;
            task.PlanningRange = model.PlanningRange;
            task.OptimizationRange = model.OptimizationRange;
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                _models.Clear();
            }
            else
            {
                Task task = _models.FirstOrDefault(req => req.Id == id);

                if (task == null)
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {id}.");

                _models.Remove(task);
            }
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
