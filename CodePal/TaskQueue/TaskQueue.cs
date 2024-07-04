using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePal.TaskQueue
{
    public class Task
    {
        public string BulletPoint { get; set; }
        public Task NextTask { get; set; }

        public Task(string bulletPoint, Task nextTask)
        {
            BulletPoint = bulletPoint;
            NextTask = null;
        }
    }

    public class TaskQueue
    {
        Task FirstTask;
        Task LastTask;
        public int NumberOfTasks;

        public TaskQueue()
        {
            FirstTask = null;
            LastTask = null;
            NumberOfTasks = 0;
        }

        public bool IsEmpty()
        {
            return NumberOfTasks == 0;
        }

        public IEnumerator<Task> GetEnumerator()
        {
            Task CurrentTask = FirstTask;

            while (CurrentTask != null)
            {
                yield return CurrentTask;
                CurrentTask = CurrentTask.NextTask;
            }
        }

        public void AddTask(string taskName)
        {
            // 1. Create and instance/object of Task to hold the information for the new task to be added.
            Task NewTask = new Task(taskName, null);

            // 2. Check if the task queue is currently empty.
            // 3. If it is, set the NewTask and the First (front) task.
            if (IsEmpty())
            {
                FirstTask = NewTask;
            }
            // 4. If the queue is not empty, set the new ask as the last task (rear).
            else
            {
                LastTask.NextTask = NewTask;
            }

            // 5. In all cases, the new task will be added to the back of the queue.
            LastTask = NewTask;
            NumberOfTasks++;
        }

        public void RemoveTask()
        {
            // 1. Check that the task queue is not currently empty.
            // 2. If it is empty, inform the user that there is nothing that can be removed.
            if (IsEmpty())
            {
                MessageBox.Show("The Task Queue is currently empty, there are no tasks to remove!");
            }
            else
            {
                string removedTask = FirstTask.BulletPoint;
                FirstTask = FirstTask.NextTask;
                NumberOfTasks--;

                if (IsEmpty())
                {
                    LastTask = null;
                    MessageBox.Show($"The task {removedTask} has been removed. The Task Queue is now empty!");
                }
                else
                {
                    MessageBox.Show($"The task {removedTask} has been removed from the Task Queue!");
                }
            }
        }
    }
}
