using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Route
    {
        public Route()
        {
            Cruises = new HashSet<Cruise>();
            RouteHalts = new HashSet<RouteHalt>();
        }

        public int RouteId { get; set; }
        public int Hidden { get; set; }

        public virtual ICollection<Cruise> Cruises { get; set; }
        public virtual ICollection<RouteHalt> RouteHalts { get; set; }
    }
}
