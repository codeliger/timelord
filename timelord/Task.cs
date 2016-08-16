using System;

namespace timelord
{
    /// <summary>
    /// A timed task with a description and duration
    /// </summary>
    class Task
    {
        public string Description { get; set; }

        public TimeSpan Duration
        {
            get
            {
                if (EndDate == DateTime.MinValue)
                    return DateTime.Now.Subtract(this.BeginDate);
                else
                    return this.EndDate.Subtract(this.BeginDate);
            }
        }

        public DateTime BeginDate
        {
            get {
                if (this.BeginDate == DateTime.MinValue)
                    throw new Exception("The BeginDate of the task with description: " + Description + " is equal to DateTime.MinValue.");
                return BeginDate;
            }
            set
            {
                BeginDate = value;
            }
            
        }

        public DateTime EndDate
        {
            get
            {
                if (this.EndDate == DateTime.MinValue)
                    throw new Exception("The EndDate of the task with description: " + Description + " is equal to DateTime.MinValue.");
                return EndDate;
            }
            set
            {
                EndDate = value;
            }
        }

        public TaskStatus Status
        {
            get;
            set;
        }
    }

    public enum TaskStatus
    {
        UNINVOICED,
        INVOICED,
        PAID
    }
}
