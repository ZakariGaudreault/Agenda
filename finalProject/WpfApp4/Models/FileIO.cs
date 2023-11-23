using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PersonalAgenda.Models
{
    public static class FileIO
    {
        private const int ONLY_SUMMARY = 1;
        private const int ALL_TASK_INFO = 4;

        // the goal is not to load all the tasks, only the ones that arent completed.
        // This method will be used to append the list of tasks from the reading of a file
        // to the already existing list of task, in TaskList.
        public static List<TaskToDo> LoadTasks(string path)
        {
            try
            {   
                // initializes a list of tasks 
                List<TaskToDo> tasks = new List<TaskToDo>();
                if (File.Exists(path)) 
                {
                    // Treats each line as an element of an array 
                    string[] lines = File.ReadAllLines(path);
                    TaskToDo taskToAdd = null;
                    foreach (string line in lines)
                    {
                        // Splits every line by their comma to get the different values
                        string[] taskElements = line.Split(',');

                        switch (taskElements.Length)
                        {
                            case ONLY_SUMMARY:
                                tasks.Add(taskToAdd = new TaskToDo(taskElements[0], DateTime.Today));
                                break;

                            case ALL_TASK_INFO:
                                tasks.Add(taskToAdd = new TaskToDo(taskElements[0], DateTime.Parse(taskElements[1]), bool.Parse(taskElements[2]), taskElements[3] ));
                                break;

                             default:
                                throw new Exception("One or many lines in file has incorrect about of arugments");
                        }

                    }
                }
                return tasks;
            }
            catch
            { 
                throw new Exception("Error in reading file.");
            }
        }

        // Takes in a set of tasks and writes them into the file. TODO might crash if the date is older than today
        public static void SaveTasks(string path, TaskList taskList)
        {
            StringBuilder sb = new StringBuilder();
            foreach(TaskToDo task in taskList.Tasks)
            {
                sb.AppendLine($"{task.Title},{task.DueDate},{task.IsCompleted},{task.Details}");
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}
