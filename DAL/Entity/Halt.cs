using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Halt
    {
        public Halt()
        {
            RouteHalts = new HashSet<RouteHalt>();
        }

        public int HaltId { get; set; }
        public string Adress { get; set; }
        public int LocalityId { get; set; }
        public int Hidden { get; set; }

        public virtual Locality Locality { get; set; }
        public virtual ICollection<RouteHalt> RouteHalts { get; set; }
    }
}
