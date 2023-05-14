using BLL.Operations;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class CruiseDTO
    {
        // дата окончания рейса, в базовой версии не используется
        public DateTime? EndingDate { get; set; }
        // дата начала рейса, в базовой версии не используется
        public DateTime? StartDate { get; set; }
        // номер рейса
        public int CruiseId { get; set; }
        // время отправки
        public string Time { get; set; }
        // скрыт или нет
        public int Hidden { get; set; }
        // день отправки
        public DayDTO Day { get; set; }
        // номер маршрута
        public int RouteId { get; set; }
        // количество мест
        public int Places { get; set; }
        public CruiseDTO()
        {

        }
        public CruiseDTO(Cruise cruise)
        {
            CruiseId = cruise.CruiseId;
            Time = cruise.Time;
            Hidden = cruise.Hidden;
            if (cruise.DayNavigation == null)
            {
                DBOperations db = new DBOperations();
                Day = db.GetDay(cruise.Day);
            }
            else
            {
                Day = new DayDTO(cruise.DayNavigation);
            }
            RouteId = cruise.RouteId;
            StartDate = cruise.StartDate;
            EndingDate = cruise.EndingDate;
            Places = cruise.Places;
        }
    }
}
