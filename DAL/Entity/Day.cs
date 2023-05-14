using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Day
    {
        public Day()
        {
            Cruises = new HashSet<Cruise>();
        }

        public int Id { get; set; }
        public string Day1 { get; set; }

        public virtual ICollection<Cruise> Cruises { get; set; }
    }
}
