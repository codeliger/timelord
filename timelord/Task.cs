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
                if (BeginDate == DateTime.MinValue) // BeginDate has not been set
                    return new TimeSpan(0, 0, 0);
                else if (EndDate == DateTime.MinValue)
                    return DateTime.Now.Subtract(this.BeginDate);
                else
                    return this.EndDate.Subtract(this.BeginDate);
            }
        }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public TaskStatus Status { get; set; }

        public Task()
        {
            this.Description = string.Empty;
            this.BeginDate = DateTime.MinValue;
            this.EndDate = DateTime.MinValue;
            this.Status = TaskStatus.UNINVOICED;
        }
    }

    public enum TaskStatus
    {
        UNINVOICED,
        INVOICED,
        PAID
    }
}
