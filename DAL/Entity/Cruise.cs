using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Cruise
    {
        public Cruise()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int CruiseId { get; set; }
        public string Time { get; set; }
        public int Hidden { get; set; }
        public int RouteId { get; set; }
        public DateTime? EndingDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int Day { get; set; }
        public int Places { get; set; }

        public virtual Day DayNavigation { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
