using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssigningTickets
{
    class TimeSlot
    {
        public DateTime StartTime;
        public int Interval;
        public int GuestsAssigned;

        // creates a time slot 
        public TimeSlot(DateTime startTime, int interval)
        {
            this.StartTime = startTime;
            this.Interval = interval;
            this.GuestsAssigned = 0;
        }
    }
}
