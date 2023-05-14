using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class TicketDTO
    {
        // номер билета
        public int TicketId { get; set; }
        // номер маршрута
        public int RouteId { get; set; }
        // номер рейса
        public int CruiseId { get; set; }
        // время отправки рейса
        public DateTime Date { get; set; }
        // место
        public int Place { get; set; }
        // возвращен
        public bool Returned { get; set; }
        // время продажи
        public DateTime SellingTime { get; set; }
        // время посадки
        public DateTime StartDate { get; set; }
        // цена
        public int Cost { get; set; }
        // отменен
        public bool Closed { get; set; }
        // сменилось место
        public bool Rplace { get; set; }
        // сменилось время
        public bool Rtime { get; set; }
        // почта покупателя
        public string Email { get; set; }
        // конечная остановка
        public RouteHaltDTO EndHalt { get; set; }
        // начальная остановка
        public RouteHaltDTO StartHalt { get; set; }

        public TicketDTO()
        {

        }

        public TicketDTO(Ticket ticket)
        {
            Email = ticket.Email;
            TicketId = ticket.TicketId;
            RouteId = ticket.RouteId;
            CruiseId = ticket.CruiseId;
            Date = ticket.Date;
            Place = ticket.Place;
            Returned = ticket.Returned;
            SellingTime = ticket.SellingTime;
            StartDate = ticket.StartDate;
            Cost = ticket.Cost;
            Closed = ticket.Closed;
            Rtime = ticket.Rtime;
            Rplace = ticket.Rplace;
            EndHalt = new RouteHaltDTO(ticket.EndHalt);
            StartHalt = new RouteHaltDTO(ticket.StartHalt);
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
