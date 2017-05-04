using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssigningTickets
{
    class Ticket
    {
        public int Number;
        public int TimeWindow;

        private static int nextNumber = 1;

        public Ticket()
        {
            this.Number = nextNumber;
            nextNumber += 1;
        }

        public static void SetNextTicketNumber(int i)
        {
            nextNumber = i;
        }
    }
}
