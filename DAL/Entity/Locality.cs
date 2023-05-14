using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Locality
    {
        public Locality()
        {
            Halts = new HashSet<Halt>();
        }

        public int LocalityId { get; set; }
        public string LocalityName { get; set; }

        public virtual ICollection<Halt> Halts { get; set; }
    }
}
