using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalAgenda.Models
{   
    //TODO ADD Color interface so that task list and task can have a color scheme based on completion
    // Composition relationship with Tasks, where this class stores multiple instances of a task inside of a list
    public class TaskList
    {
        #region Data members
        private readonly List<TaskToDo> _taskList;
        #endregion

        #region Constructors
        public TaskList()
        {
            _taskList = new List<TaskToDo>();
        }

        public TaskList(List<TaskToDo> tasks)
        {
            if (tasks is null)
                throw new ArgumentNullException("tasks", "List cannot be null");
            _taskList = tasks;
        }
        #endregion

        #region Properties

        public List<TaskToDo> Tasks
        {
            get { return _taskList; }
        }

        // Gets the number of tasks, whether done or not
        public int Count
        {
            get { return Tasks.Count; }
        }

        // Gets the list of tasks that are completed
        public List<TaskToDo> CompletedTasks
        {
            get { return GetCount(true); }
        }

        // Gets the list of tasks that are not completed
        public List<TaskToDo> UncompletedTasks
        {
            get { return GetCount(false); }
        }

        // Gets a list with all the tasks that are overdue - not completed and late.
        public List<TaskToDo> OverDueTasks
        {
            get
            {
                List<TaskToDo> tempTasks = new List<TaskToDo>();
                foreach (TaskToDo task in Tasks)
                {
                    if (task.IsOverDue)
                        tempTasks.Add(task);
                };
                return tempTasks;
            }
        }
        #endregion

        #region Methods

        // Adds a new task to the list 
        public void AddTask(TaskToDo newTask)
        {
            if (newTask != null)
                _taskList.Add(newTask);
        }

        // Removes a task in the list that has the same properties as the passed in task
        public void RemoveTask(TaskToDo taskToRemove)
        {
            if (taskToRemove != null)
                for (int i = 0; i < _taskList.Count; i++)
                {
                    if(taskToRemove == _taskList[i])
                    {
                        _taskList.RemoveAt(i);
                        break;
                    }
                }
        }

        // Adds a list of new tasks to the existing list, and returns it
        public void AddTasks(List<TaskToDo> tasks)
        {
            _taskList.AddRange(tasks);
        }

        public void MarkTaskAsDone(TaskToDo taskDone)
        {
            if (taskDone != null)
                for (int i = 0; i < _taskList.Count; i++)
                {
                    if (taskDone == _taskList[i])
                    {
                        _taskList[i].IsCompleted= true;
                        break;
                    }
                }
        }
        public void ClearScheduler()
        {
            _taskList.Clear();
        }
        // Returns a list based on the boolean passed to it. It either returns the completed tasks, or 
        // the uncompleted ones. 
        public List<TaskToDo> GetCount(bool isCompleted)
        {
            List<TaskToDo> tempTasks = new List<TaskToDo>();
            foreach (TaskToDo task in Tasks)
            {
                if (task.IsCompleted == isCompleted)
                    tempTasks.Add(task);
            };
            return tempTasks;
        }

        // Gets a list of the tasks in a month
        public List<TaskToDo> GetTasksInMonth(DateTime date)
        {
            List<TaskToDo> tempTasks = new List<TaskToDo>();
            foreach (TaskToDo task in Tasks)
            {
                if (task.DueDate.Month == date.Month && task.DueDate.Year == date.Year)
                    tempTasks.Add(task);
            };
            return tempTasks;
        }

        // Gets a list of the tasks of a chosen date, without taking into account the hours
        public List<TaskToDo> GetTasksOfTargetDate(DateTime date)
        {
            List<TaskToDo> tempTasks = new List<TaskToDo>();
            foreach (TaskToDo task in Tasks)
            {
                if (task.DueDate.Date == date.Date )
                    tempTasks.Add(task);
            };
            return tempTasks;
        }
        #endregion
    }
}
