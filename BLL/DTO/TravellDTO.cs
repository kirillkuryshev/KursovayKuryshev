using BLL.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class TravellDTO // рейс, подходящий для покупателя с дополнительной информацией
    {
        public int CruiseId { get; set; }  // номер рейса
        public List<int> Places { get; set; } // список свободных мест
        public int Cost { get; set; } // цена
        public string Time { get; set; } // время отправки
        public DateTime StartDate; // дата отправки
        public DateTime Date { get; set; } // дата отправки рейса
        public int RouteId { get; set; } // номер маршрута
        // начальная остановка
        public RouteHaltDTO Start { get; set; }
        // конечная остановка
        public RouteHaltDTO End { get; set; }


        public TravellDTO()
        {

        }

        public TravellDTO(CruiseDTO cruise, RouteHaltDTO start, RouteHaltDTO end, DateTime date)
        {
            TicketOperations t = new TicketOperations();
            DBOperations db = new DBOperations();
            CruiseId = cruise.CruiseId;
            Start = start;
            End = end;
            Cost = End.Cost - Start.Cost;
            StartDate = date;
            Date = date.AddMinutes(-start.Time);
            Time = StartDate.TimeOfDay.ToString().Substring(0, 5);
            Places = t.GetFreePlaces(CruiseId, date, Start, End, cruise.Places);
            RouteId = cruise.RouteId;
        }
    }
}
