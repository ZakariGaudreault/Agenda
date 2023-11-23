using PersonalAgenda.Models;
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

namespace PersonalAgenda
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        string _monthReport;
        int _yearReport;
        public ReportWindow(List<TaskToDo> tasksInMonth)
        {
            InitializeComponent();
            // The source is a list ordered by overdue and then by due date, since you would want the overdue tasks on top
            // and the dates that are the nearest to become due on top. This is a report with the overdue tasks, but also all the tasks in 
            // the month taht aren't completed.
            lvTasksInMonth.ItemsSource = tasksInMonth.OrderBy(t => t.IsOverDue).ThenBy(t => t.DueDate).ToList();
            _yearReport = CalendarTime.Year;
            _monthReport = CalendarTime.MonthString();
            txbReportTitle.Text = $"Report for {_monthReport} {_yearReport}";
        }
    }
}
