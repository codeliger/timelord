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
                return this.EndDate.Subtract(this.BeginDate);
            }
        }

        public DateTime BeginDate
        {
            private get;
            set;
        }

        public DateTime EndDate
        {
            private get;
            set;
        }

        public TaskState State
        {
            get;
            set;
        }
    }

    public enum TaskState
    {
        UNINVOICED,
        INVOICED,
        PAID
    }
}
