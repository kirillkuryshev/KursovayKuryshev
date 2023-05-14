using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Operations
{
    public class RouteHaltOperations
    {
        DBOperations db = new DBOperations();
        public RouteHaltOperations()
        {
            
        }

        public void Update(RouteHaltDTO selectedHalt) // обновление остановки маршрута
        {
            var old = db.GetRouteHalt(selectedHalt.RouteHaltId); // старая остановка
            if (old != null && old.Time != selectedHalt.Time) // сменилось время
            {
                // пересчет времени отправки для билетов
                TicketOperations t = new TicketOperations();
                var tickets = db.GetTickets().Where(p => 
                p.RouteId == selectedHalt.RouteId && !p.Closed && 
                p.StartDate > DateTime.Now).ToList();
                t.ChangeTime(tickets, selectedHalt.Time - old.Time, false);
            }
        }

        public void Hide(RouteHaltDTO selectedHalt) // скрытие остановки маршрута
        {
            EmailOperations emailOperations = new EmailOperations();
            TicketOperations t = new TicketOperations();
            var tickets = db.GetTickets().Where(p => // отмена билетов с и на данную остановку
            p.RouteId == selectedHalt.RouteId && !p.Closed &&
            p.StartDate > DateTime.Now && (p.StartHalt.RouteHaltId == selectedHalt.RouteHaltId
            || p.EndHalt.RouteHaltId == selectedHalt.RouteHaltId)).ToList();
            foreach (TicketDTO ticket in tickets)
            {
                ticket.Closed = true;
                db.UpdateTicket(ticket);
                emailOperations.Close(ticket, ticket.Email); // отправка сообщения об отмене
            }
        }
    }
}
