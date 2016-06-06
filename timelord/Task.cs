using System;

namespace timelord
{
    class Task
    {
        public DateTime start   { get; private set; }

        public DateTime end     { get; private set; }

        public Task()
        {
            this.start = DateTime.Now;
        }

        public void finish()
        {
            this.end = DateTime.Now;
        }
    }
}
