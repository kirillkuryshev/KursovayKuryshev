using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Ticket
    {
        public int TicketId { get; set; }
        public int RouteId { get; set; }
        public int CruiseId { get; set; }
        public DateTime Date { get; set; }
        public int Place { get; set; }
        public int StartHaltId { get; set; }
        public bool Returned { get; set; }
        public int EndHaltId { get; set; }
        public DateTime SellingTime { get; set; }
        public DateTime StartDate { get; set; }
        public int Cost { get; set; }
        public bool Closed { get; set; }
        public bool Rplace { get; set; }
        public bool Rtime { get; set; }
        public string Email { get; set; }

        public virtual Cruise Cruise { get; set; }
        public virtual RouteHalt EndHalt { get; set; }
        public virtual RouteHalt StartHalt { get; set; }
    }
}
