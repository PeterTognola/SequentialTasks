using System;
using System.Threading.Tasks;

namespace SequentialTasks.TaskManager
{
    public class BackgroundTask
    {
        public int TaskId { get; set; }

        public Func<Task> Task { get; set; } // Might not have to be async
    }
}