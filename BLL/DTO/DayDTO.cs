using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class DayDTO
    {
        public int Id { get; set; }
        public string Day { get; set; }

        public DayDTO()
        {

        }

        public DayDTO(Day day)
        {
            Id = day.Id;
            Day = day.Day1;
        }
    }
}
