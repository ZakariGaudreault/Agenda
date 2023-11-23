using PersonalAgenda.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonalAgenda
{
    /// <summary>
    /// Interaction logic for DailyTaskWindow.xaml
    /// </summary>
    public partial class DailyTaskWindow : Window
    {
        #region Backing Fields
        private int _selectedIndex = -1;
        private TaskList _selectedDayTaskList;
        private List<TaskToDo> _taskList; //Local list that will be used to display the tasks of the day. 
        TaskToDo _selectedTaskToDelete;
        TaskToDo _selectedTaskToMarkComplete;
        #endregion

        #region Constructor
        public DailyTaskWindow(TaskList selectedTaskList)
        {
            InitializeComponent();
            // Local variable assigned so that the reference value is passed. Hence modifying the original will be possible
            _selectedDayTaskList = selectedTaskList;
 

            // Storing the daily task list into a variable so that there's no need to recalculate the values when it comes to accessing the tasks.
            _taskList = _selectedDayTaskList.GetTasksOfTargetDate(CalendarTime.FullDate);
            lvTasksList.ItemsSource = _taskList;

            // Disables the adding button for the dates that have passed
            if (CalendarTime.FullDate < DateTime.Today)
                btnAddTask.IsEnabled = false;

            txbDateTitle.Text = $"Tasks for {CalendarTime.FullDate.ToShortDateString()}";
        }
        #endregion

        #region Button Click Events
        //TODO check if it's possible to either put the completed tasks on the bottom, or make the not completed ones in a certain color
        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (!InputValidation())
            {
                MessageBox.Show($"You did not select anything to complete", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Finds the task to mark as completed
            _selectedTaskToMarkComplete = _taskList[_selectedIndex];

            _selectedDayTaskList.MarkTaskAsDone(_selectedTaskToMarkComplete);

            // Updates it from the list view 
            _taskList[_selectedIndex].IsCompleted = true;
            lvTasksList.Items.Refresh();
        }

        // Deletes selected task
        private void btnTrash_Click(object sender, RoutedEventArgs e)
        {
            if (!InputValidation())
            {
                MessageBox.Show($"You did not select anything to delete", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gets the selected index of the task
            _selectedTaskToDelete = _taskList[_selectedIndex];

            // Deletes it from the original list
            _selectedDayTaskList.RemoveTask(_selectedTaskToDelete);

            // Removes it from the list view, since it's not directly linked to the original list
            _taskList.RemoveAt(_selectedIndex);
            lvTasksList.Items.Refresh();
        }

        // Adds a task 
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txbTitle.Text))
            {
                MessageBox.Show($"A title is mandatory to add a task", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            TaskToDo taskAdded;
            // Makes it so that the due date is at 23:59 of the selected day
            taskAdded = new TaskToDo(txbTitle.Text, CalendarTime.FullDate);
            taskAdded.Details = txbDetails.Text;
            _selectedDayTaskList.AddTask(taskAdded);

            _taskList.Add(taskAdded);
            lvTasksList.Items.Refresh();
        }
        #endregion

        #region Helper Methods
        private bool InputValidation()
        {
            return (_selectedIndex != -1);
        }

        private void lvTasksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedIndex = lvTasksList.SelectedIndex;
        }
        #endregion

    }
}
