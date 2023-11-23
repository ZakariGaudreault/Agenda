using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalAgenda.Models
{
    public class TaskToDo
    {
        #region Data members
        private string _title;
        private string _details;
        private DateTime _dueDate;
        private bool _isCompleted;
        #endregion

        #region Constructors
        public TaskToDo(string title, DateTime dueDate)
        {
            Title = title;
            DueDate = dueDate;
        }

        public TaskToDo(string title,DateTime dueDate, bool isCompleted, string details):this(title,dueDate)
        {
            Details = details;
            IsCompleted = isCompleted;
        }


        #endregion


        #region Properties
        public string Title
        {
            get { return _title; }
            set
            {
                if (value is null)
                    throw new ArgumentException("string must not be null or be empty");

                _title = value;
            }
        }

        public string Details
        {
            get { return _details; }
            set
            {
                if (value is null)
                    _details = "";

                else
                    _details = value;
            }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }

            set
            {
                //// if input is still smaller, after adding a month, means it is older than 4 month. The choice 4 month comes from that a semester is 4 month
                if (value.AddMonths(4) < DateTime.Today)
                    throw new ArgumentException("Cannot add a due time that has already passed");

                _dueDate = value;
            }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }

            set { _isCompleted = value; }

            // No need to add validation since both possibilities of bool are accepted. If it's a wrong type, it will
            // already throw an exception by itself
        }

        public bool IsOverDue
        {
            get { return _dueDate < DateTime.Today && IsCompleted; }
        }

        public string Info
        {
            get { return $"Task: {Title}"; } 
        }
        #endregion

        #region Method Override
        public override string ToString()
        {
            return Info;
        }
        #endregion
    }
}
