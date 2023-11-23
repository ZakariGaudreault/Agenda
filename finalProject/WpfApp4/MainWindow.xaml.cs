using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PersonalAgenda.Models;

namespace PersonalAgenda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Enum that represents the name of the buttons, which allows us to use them as a sort of index
        private enum LetterNumber
        {
            ONE, TWO, THREE,
            FOUR, FIVE, SIX, SEVEN, EIGHT,
            NINE, TEN, ELEVEN, TWELVE,
            THIRTEEN, FOURTEEN, FIFTEEN,
            SIXTEEN, SEVENTEEN, EIGHTEEN,
            NINETEEN, TWENTY, TWENTY_ONE,
            TWENTY_TWO, TWENTY_THREE, TWENTY_FOUR,
            TWENTY_FIVE, TWENTY_SIX, TWENTY_SEVEN,
            TWENTY_EIGHT, TWENTY_NINE, THIRTY, THIRTY_ONE,
            THIRTY_TWO, THIRTY_THREE, THIRTY_FOUR, THIRTY_FIVE,
            THIRTY_SIX,THIRTY_SEVEN
        };

        private int NUMBER_DAY_BUTTONS = Enum.GetNames(typeof(LetterNumber)).Length;
        TaskList _taskList = new TaskList();
        public MainWindow()
        {
            InitializeComponent();
            PopulateCalendar();
        }
  
        private void PopulateCalendar()
        {
            txbDate.Text = $"{CalendarTime.MonthString()} {CalendarTime.Year}";
            DateTime date = new DateTime(CalendarTime.Year, CalendarTime.Month, 1); //It has to be the first day, otherwise sometimes the day stored in CalendarTime doesn't exist in the current month
            int numberOfDays = DateTime.DaysInMonth(CalendarTime.Year, CalendarTime.Month);

            // Makes so that the day is from 1 to 7 where 1 represents sunday
            int firstDayPositionOfMonth = (int)date.DayOfWeek;
            int lastDayPositionOfMonth = firstDayPositionOfMonth + numberOfDays;

            // A variable that keeps track of the buttons that needs a date
            int buttonPosition = firstDayPositionOfMonth;
            int dayDate = 1;

            // Resets all the buttons in the calendar so that their content is equal to nothing
            for (int day = 0; day < NUMBER_DAY_BUTTONS; day++)
            {
                // Sets all the days in the calendar if the button position is between the first day and last day. 
                if (day >= firstDayPositionOfMonth && day < lastDayPositionOfMonth )
                {
                    string buttonName = Enum.GetName(typeof(LetterNumber), buttonPosition++).ToString();
                    Button button = calendar.FindName(buttonName) as Button;
                    button.Content = $"{dayDate++}";
                    button.IsEnabled = true;
                }
                else
                {
                    (calendar.FindName(Enum.GetName(typeof(LetterNumber), day).ToString()) as Button).Content = "";
                    string btnNameToDisable = Enum.GetName(typeof(LetterNumber), day).ToString();
                    Button btnToDisable = calendar.FindName(btnNameToDisable) as Button;
                    btnToDisable.IsEnabled= false;

                }
            }
        }
        //Adds a month to the calendar, and refreshes it
        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            CalendarTime.AddMonth();
            PopulateCalendar();
        }

        // Changes the month and refreshes it
        private void btnPreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            CalendarTime.RemoveMonth();
            PopulateCalendar();
        }

        // Will show the tasks on the left of the selected day for the ones that are not completed
        private void btnShowMoreInfo_Click(object sender, EventArgs e)
        {
            CalendarTime.Day = int.Parse((sender as Button).Content.ToString());
            txbDayTaskDate.Text = CalendarTime.FullDate.ToShortDateString();
            PopulateTaskInfo();
        }

        // Will load a new window about the tasks of the day and options to customize it
        private void btnUpdateInfo_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CalendarTime.Day = int.Parse((sender as Button).Content.ToString());
            DailyTaskWindow dailyTaskWindow = new DailyTaskWindow(_taskList);
            dailyTaskWindow.ShowDialog();
            PopulateTaskInfo();
        }

        // Opens a window with a report of all the tasks of the month
        private void btnGenrateReport_Click(object sender, EventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(_taskList.GetTasksInMonth(CalendarTime.FullDate));
            reportWindow.ShowDialog();
            PopulateTaskInfo();
        }

        // Populates the quick task info on the left with the appropriate data
        private void PopulateTaskInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TaskToDo task in _taskList.GetTasksOfTargetDate(CalendarTime.FullDate))
                if (!task.IsCompleted)
                    sb.AppendLine(task.Info);
            txbTaskSummary.Text = sb.ToString();
        }

        // Loads task from file and appends it to the current tasklist
        private void btnLoadCalendar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                string _openLocation = openFileDialog.FileName;
                _taskList.ClearScheduler();
                _taskList.AddTasks(FileIO.LoadTasks(_openLocation));
            }

        }
        // Save the current tasks inside of a csv file
        private void btnSaveCalendar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                string _saveLocation = saveFileDialog.FileName;
                FileIO.SaveTasks(_saveLocation, _taskList);
            }

        }

        // A popup that guides the user to the different features of the app
        private void btninfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Click once on a button inside the calendar to see all task for that day. You can also click twice to add a task for the selected day, " +
                $"Report shows all the task of the month and you can use the arrow buttons to naviguate through the months", "Features", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Opens a report will all the overdue tasks
        private void btnShowODTask_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(_taskList.OverDueTasks);
            reportWindow.ShowDialog();
        }
    }
}
