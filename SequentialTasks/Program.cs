using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SequentialTasks
{
    // --- SPEC ---
    // Write a C# class to execute a number of pieces of work (actions) in the background (i.e. without blocking the program’s execution):
    // The actions must be executed sequentially – one at a time;
    // The actions must be executed in the order that they were added to the class;
    // The actions are not necessarily added all at the same time;
    // The actions are not necessarily all executed on the same thread;

    public class Program
    {
        public static void Main(string[] args)
        {
            var tasks = new TaskOrganiser();

            while (Console.ReadLine()?.ToLower() != "exit")
            {
                //Thread.Sleep(1000)

                tasks.AddAction(new BackgroundTask
                {
                    Task = () => Task.Delay(new Random().Next(1, 10) * 1000)
                });
            }
        }

        public async Task DelayedTask()
        {
            await Task.Delay(new Random().Next(3, 6) * 1000);
        }
    }

    public class TaskOrganiser
    {
        private readonly List<BackgroundTask> _backLog = new List<BackgroundTask>();

        private int _taskCount = 0;

        public Task nt { get; set; }

        public TaskOrganiser()
        {
            nt = new Task(ActionRunner);
        }

        public void AddAction(BackgroundTask task)
        {
            task.TaskId = ++_taskCount;
            _backLog.Add(task);

            if (nt.Status == TaskStatus.Created) nt.Start();
        }

        public async void ActionRunner()
        {
            if (_backLog.Count > 0)
            {
                var currentTask = _backLog.First();
                Console.WriteLine($"Starting task #{currentTask.TaskId}.");
                await currentTask.Task()
                    .ContinueWith(p =>
                    {
                        Console.WriteLine($"Finished task #{currentTask.TaskId}.");
                        _backLog.Remove(currentTask);
                        ActionRunner();
                    }).ConfigureAwait(false);
            }
            else
                nt = new Task(ActionRunner);
        }
    }

    public class BackgroundTask
    {
        public int TaskId { get; set; }

        public Func<Task> Task { get; set; }
    }
}