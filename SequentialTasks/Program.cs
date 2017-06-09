using System;
using System.Threading.Tasks;
using SequentialTasks.TaskManager;

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
            var tasks = new TaskManager.TaskManager();

            while (Console.ReadLine()?.ToLower() != "exit")
            {
                tasks.AddTask(new BackgroundTask
                {
                    Task = () => Task.Delay(new Random().Next(1, 5) * 1000)
                });
            }
        }
    }
}