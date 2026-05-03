using Core;
using Core.Enums;
using Core.Logic.DateTimeHelpers;
using Core.Models.Storage;
using Task = Core.Models.Storage.Task;

namespace Test.DateTimeTests
{
    internal class DayOfMonthTest
    {
        private RepeatMode repeatMode = RepeatMode.DayOfMonth;

        #region CheckIsValueCorrect

        [Test]
        public void TestCheckCorrectValues()
        {
            DateTimeHelper.CheckIsValueCorrect("1", repeatMode);
            DateTimeHelper.CheckIsValueCorrect("15", repeatMode);
            DateTimeHelper.CheckIsValueCorrect("28", repeatMode);
            DateTimeHelper.CheckIsValueCorrect("29", repeatMode);
            DateTimeHelper.CheckIsValueCorrect("30", repeatMode);
            DateTimeHelper.CheckIsValueCorrect("31", repeatMode);
        }

        [Test]
        public void TestCheckIncerrectValues()
        {
            Assert.Throws<Exception>(() => DateTimeHelper.CheckIsValueCorrect("May", repeatMode));
            Assert.Throws<Exception>(() => DateTimeHelper.CheckIsValueCorrect("-1", repeatMode));
            Assert.Throws<Exception>(() => DateTimeHelper.CheckIsValueCorrect("32", repeatMode));
        }

        #endregion

        #region FillRepeatedTasks

        [Test]
        public void TestRepeat15DayOfMonth()
        {
            DateTime dateStart = new DateTime(2021, 8, 15);

            Task task = new Task
            {
                RepeatValue = "15",
                RepeatMode = repeatMode,
                PlanningRange = 180
            };

            GroundhogContext.TaskLogic.Create(task);

            TaskInstance taskInstance = new TaskInstance
            {
                TaskId = task.Id,
                Completed = false,
                Date = dateStart
            };

            GroundhogContext.TaskInstanceLogic.Create(taskInstance);

            DateTimeHelper.FillRepeatedTasks(dateStart);
            var taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);

            Assert.AreEqual(7, taskInstances.Count);
        }

        [Test]
        public void TestRepeat28DayOfMonth()
        {
            DateTime dateStart = new DateTime(2022, 2, 28);

            Task task = new Task
            {
                RepeatValue = "28",
                RepeatMode = repeatMode,
                PlanningRange = 1000
            };

            GroundhogContext.TaskLogic.Create(task);

            TaskInstance taskInstance = new TaskInstance
            {
                TaskId = task.Id,
                Completed = false,
                Date = dateStart
            };

            GroundhogContext.TaskInstanceLogic.Create(taskInstance);

            DateTimeHelper.FillRepeatedTasks(dateStart);
            var taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            var taskInstancesCheck = taskInstances.Where(x => x.Date.Day != 28);

            Assert.IsEmpty(taskInstancesCheck);
        }

        [Test]
        public void TestRepeat29DayOfMonth()
        {
            DateTime dateStart = new DateTime(2022, 2, 28);
            DateTime dateEnd = dateStart.AddDays(1000);

            Task task = new Task
            {
                RepeatValue = "29",
                RepeatMode = repeatMode,
                PlanningRange = 1000
            };

            GroundhogContext.TaskLogic.Create(task);

            TaskInstance taskInstance = new TaskInstance
            {
                TaskId = task.Id,
                Completed = false,
                Date = dateStart
            };

            GroundhogContext.TaskInstanceLogic.Create(taskInstance);

            DateTimeHelper.FillRepeatedTasks(dateStart);
            var taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            var taskInstances28Check = taskInstances.Where(x => x.Date.Day == 28).ToList();
            var taskInstances29Check = taskInstances.Where(x => x.Date.Day != 29).ToList();
            var taskInstances30Check = taskInstances.Where(x => x.Date.Day >= 30).ToList();

            Assert.IsEmpty(taskInstances30Check);
            Assert.AreEqual(2, taskInstances28Check.Count);
            Assert.AreEqual(2, taskInstances29Check.Count);
        }

        [Test]
        public void TestRepeat30DayOfMonth()
        {
            DateTime dateStart = new DateTime(2022, 8, 30);
            DateTime dateEnd = dateStart.AddDays(1000);

            Task task = new Task
            {
                RepeatValue = "30",
                RepeatMode = repeatMode,
                PlanningRange = 1000
            };

            GroundhogContext.TaskLogic.Create(task);

            TaskInstance taskInstance = new TaskInstance
            {
                TaskId = task.Id,
                Completed = false,
                Date = dateStart
            };

            GroundhogContext.TaskInstanceLogic.Create(taskInstance);

            DateTimeHelper.FillRepeatedTasks(dateStart);
            var taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            var taskInstances28Check = taskInstances.Where(x => x.Date.Day == 28).ToList();
            var taskInstances29Check = taskInstances.Where(x => x.Date.Day == 29).ToList();
            var taskInstances30Check = taskInstances.Where(x => x.Date.Day != 30).ToList();

            Assert.AreEqual(2, taskInstances28Check.Count);
            Assert.AreEqual(1, taskInstances29Check.Count);
            Assert.AreEqual(3, taskInstances30Check.Count);
        }

        [Test]
        public void TestRepeat31DayOfMonth()
        {
            DateTime dateStart = new DateTime(2022, 8, 30);
            DateTime dateEnd = dateStart.AddDays(1000);

            Task task = new Task
            {
                RepeatValue = "31",
                RepeatMode = repeatMode,
                PlanningRange = 1000
            };

            GroundhogContext.TaskLogic.Create(task);

            TaskInstance taskInstance = new TaskInstance
            {
                TaskId = task.Id,
                Completed = false,
                Date = dateStart
            };

            GroundhogContext.TaskInstanceLogic.Create(taskInstance);

            DateTimeHelper.FillRepeatedTasks(dateStart);
            var taskInstances = GroundhogContext.TaskInstanceLogic.Read(task.Id);
            var taskInstances28Check = taskInstances.Where(x => x.Date.Day == 28).ToList();
            var taskInstances29Check = taskInstances.Where(x => x.Date.Day == 29).ToList();
            var taskInstances30Check = taskInstances.Where(x => x.Date.Day == 30).ToList();

            Assert.AreEqual(2, taskInstances28Check.Count);
            Assert.AreEqual(1, taskInstances29Check.Count);
            Assert.AreEqual(12, taskInstances30Check.Count);
        }

        #endregion

        #region GetDateForTask

        #region GetDateForTask 15

        [Test]
        public void TestGetDateFor15DayOfMonth_WithSameSelected()
        {
            DateTime selectedDate = new DateTime(2021, 8, 15);

            Task task = new Task
            {
                RepeatValue = "15",
                RepeatMode = repeatMode,
                PlanningRange = 180
            };

            DateTime result = DateTimeHelper.GetDateForTask(task, selectedDate, selectedDate);

            Assert.AreEqual(new DateTime(2021, 8, 15), result);
        }

        [Test]
        public void TestGetDateFor15DayOfMonth_WithEarlerSelected()
        {
            DateTime selectedDate = new DateTime(2021, 8, 14);

            Task task = new Task
            {
                RepeatValue = "15",
                RepeatMode = repeatMode,
                PlanningRange = 180
            };

            DateTime result = DateTimeHelper.GetDateForTask(task, selectedDate, selectedDate);

            Assert.AreEqual(new DateTime(2021, 8, 15), result);
        }

        [Test]
        public void TestGetDateFor15DayOfMonth_WithLaterSelected()
        {
            DateTime selectedDate = new DateTime(2021, 8, 16);

            Task task = new Task
            {
                RepeatValue = "15",
                RepeatMode = repeatMode,
                PlanningRange = 180
            };

            DateTime result = DateTimeHelper.GetDateForTask(task, selectedDate, selectedDate);

            Assert.AreEqual(new DateTime(2021, 9, 15), result);
        }

        #endregion

        #region GetDateForTask 31

        [Test]
        public void TestGetDateFor31DayOfMonth_WithSameSelected()
        {
            DateTime selectedDate = new DateTime(2021, 8, 31);

            Task task = new Task
            {
                RepeatValue = "31",
                RepeatMode = repeatMode,
                PlanningRange = 180
            };

            DateTime result = DateTimeHelper.GetDateForTask(task, selectedDate, selectedDate);

            Assert.AreEqual(new DateTime(2021, 8, 31), result);
        }

        [Test]
        public void TestGetDateFor31DayOfMonth_WithEarlerSelected()
        {
            DateTime selectedDate = new DateTime(2021, 8, 30);

            Task task = new Task
            {
                RepeatValue = "31",
                RepeatMode = repeatMode,
                PlanningRange = 180
            };

            DateTime result = DateTimeHelper.GetDateForTask(task, selectedDate, selectedDate);

            Assert.AreEqual(new DateTime(2021, 8, 31), result);
        }

        [Test]
        public void TestGetDateFor31DayOfMonth_WithLaterSelected()
        {
            DateTime selectedDate = new DateTime(2021, 9, 1);

            Task task = new Task
            {
                RepeatValue = "31",
                RepeatMode = repeatMode,
                PlanningRange = 180
            };

            DateTime result = DateTimeHelper.GetDateForTask(task, selectedDate, selectedDate);

            Assert.AreEqual(new DateTime(2021, 9, 30), result);
        }

        #endregion

        #endregion
    }
}
