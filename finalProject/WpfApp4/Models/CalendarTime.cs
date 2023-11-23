using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PersonalAgenda.Models
{
    // static class to store the calendar time display. Since it's used as a reference, there wont't be any instances of it needed
    public static class CalendarTime
    {
        private static int _day = DateTime.Now.Day;
        private static int _month = DateTime.Now.Month;
        private static int _year = DateTime.Now.Year;

        public static int Month
        {
            get { return _month; }
            set
            {
                if (value > 12 || value < 1)
                    throw new ArgumentOutOfRangeException("Invalid Month");
                _month = value;
            }
        }

        public static int Day
        {
            get { return _day; }
            set
            {
                if (value > DateTime.DaysInMonth(Year,Month) || value < 1)
                    throw new ArgumentOutOfRangeException("Invalid Day");
                _day = value;
            }
        }

        // No need for a year setter because technically any year is possible
        public static int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        // Returns a full date 
        public static DateTime FullDate
        {
            get { return new DateTime(Year, Month, Day, 23, 59, 59); }
        }

        #region Methods
        // Adds or substracts a month while updating the year
        public static void AddMonth()
        {
            if (Month == 12)
            {
                Month = 1;
                Year++;
            }
            else
                Month++;
        }

        // Returns a string version of the month
        public static string MonthString()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        }

        public static void RemoveMonth() 
        {
            if (Month == 1)
            {
                Month = 12;
                Year--;
            }
            else
                Month--;
        }
        #endregion
    }

}
