using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Operations
{
    public class CruiseOperations
    {
        DBOperations db = new DBOperations();
        public bool Update(CruiseDTO selectedCruise) // обновление рейса
        {
            TicketOperations t = new TicketOperations();
            var oldCruise = db.GetCruise(selectedCruise.CruiseId);
            if (oldCruise == null)
            {
                return false;
            }
            if (oldCruise.Time != selectedCruise.Time) // сменилось время отправки
            {
                // билеты, которые затронула смена времени
                var tickets = db.GetTickets().Where(p => p.CruiseId == 
                selectedCruise.RouteId && !p.Closed && p.StartDate > DateTime.Now).ToList();
                if (tickets.Count > 0)
                {
                    // выполнение изменения времени
                    t.ChangeTime(tickets, (int)(DateTime.Parse(selectedCruise.Time).TimeOfDay.TotalMinutes 
                        - DateTime.Parse(oldCruise.Time).TimeOfDay.TotalMinutes), true);
                }
            }
            if (oldCruise.Places > selectedCruise.Places) // сменилось число мест
            {
                // билеты, которые затронула смена числа мест
                var tickets = db.GetTickets().Where(p => p.CruiseId == selectedCruise.RouteId 
                && !p.Closed && p.StartDate > DateTime.Now).ToList();
                foreach (TicketDTO ticket in tickets)
                {
                    // получение свободных мест на рейс
                    var freePlaces = t.GetFreePlaces(ticket.CruiseId, ticket.Date, 
                        ticket.StartHalt, ticket.EndHalt, selectedCruise.Places);
                    // смена места или отмена билета
                    if (freePlaces.Count > 0)
                    {
                        ticket.Place = freePlaces.First();
                        ticket.Rplace = true;
                    }
                    else
                    {
                        ticket.Closed = true;
                    }
                    db.UpdateTicket(ticket);
                }
            }
            // если сменилась дата окончания рейса, не реализовано в базовой версии
            if (selectedCruise.EndingDate != null && selectedCruise.EndingDate.Value.Date >= 
                DateTime.Now.Date)
            {
                // дата окончания задана впервые или сдвинулась влево
                if (oldCruise.EndingDate == null || oldCruise.EndingDate > selectedCruise.EndingDate)
                {
                    // список билетов, которые оформелены на даты после окончания работы рейса
                    var tickets = db.GetTickets().Where(p => p.CruiseId == selectedCruise.RouteId && 
                    !p.Closed && p.StartDate > selectedCruise.EndingDate).ToList();
                    // отмена билетов
                    foreach (TicketDTO ticket in tickets)
                    {
                        ticket.Closed = true;
                        db.UpdateTicket(ticket);
                    }
                }
            }
            return db.UpdateCruise(selectedCruise);
        }
    }
}
