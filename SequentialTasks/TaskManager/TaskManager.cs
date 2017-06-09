using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SequentialTasks.TaskManager
{
    public class TaskManager
    {
        private readonly List<BackgroundTask> _backLog;

        private int _taskCount;
        private Task _task;

        public TaskManager()
        {
            _task = new Task(TaskRunner);
            _backLog = new List<BackgroundTask>();
        }

        public void AddTask(BackgroundTask task)
        {
            task.TaskId = ++_taskCount;
            _backLog.Add(task);

            if (_task.Status == TaskStatus.Created) _task.Start();
        }

        public async void TaskRunner()
        {
            if (_backLog.Count == 0)
            {
                // Once there are no BackgroundTasks in _backLog we will make
                // task ready for more actions to be added to the list.
                _task = new Task(TaskRunner);
                return;
            }
            
            var currentTask = _backLog.First();

            Console.WriteLine($"Starting task #{currentTask.TaskId}!");

            // Researched locking in C# and async, found a few solutions (like AsyncSemaphore)
            // but went with a "chaining/recursive" solution using ContinueWith instead (feels simpler).
            // Used Google, Stackoverflow and MSDN.
            await currentTask.Task()
                .ContinueWith(p =>
                {
                    Console.WriteLine($"Finished task #{currentTask.TaskId}!");
                    _backLog.Remove(currentTask);
                    TaskRunner();
                });
        }
    }
}