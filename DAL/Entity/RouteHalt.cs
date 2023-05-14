using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class RouteHalt
    {
        public RouteHalt()
        {
            TicketEndHalts = new HashSet<Ticket>();
            TicketStartHalts = new HashSet<Ticket>();
        }

        public int RouteHaltId { get; set; }
        public int HaltId { get; set; }
        public int RouteId { get; set; }
        public int Cost { get; set; }
        public int Hidden { get; set; }
        public int NumberInRoute { get; set; }
        public int Time { get; set; }

        public virtual Halt Halt { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<Ticket> TicketEndHalts { get; set; }
        public virtual ICollection<Ticket> TicketStartHalts { get; set; }
    }
}
