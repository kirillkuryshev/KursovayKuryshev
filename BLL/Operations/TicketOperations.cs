using BLL.DTO;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Operations
{
    public class TicketOperations
    {
        DBOperations db = new DBOperations();
        public TicketOperations()
        {

        }
        public void ChangeTime(List<TicketDTO> tickets, int time, bool start) // смена времени отправки
        {
            foreach (TicketDTO t in tickets)
            {
                t.Rtime = true; // установка флага смены времени на случай возврата
                t.StartDate.AddMinutes(time);
                if (start) // изменилось ли время отправки рейса
                {
                    t.Date.AddMinutes(time);
                }
                db.UpdateTicket(t);
            }
        }

        public List<int> GetFreePlaces(int cruiseId, DateTime date, RouteHaltDTO startHalt, 
            RouteHaltDTO endHalt, int places) // получение свободных мест для рейса на конкретную дату
        {
            List<TicketDTO> buffer = db.GetTickets().ToList().Where(i => i.Closed == false 
            && i.CruiseId == cruiseId && i.Date.Date == date.Date 
            && i.StartHalt.NumberInRoute < endHalt.NumberInRoute 
            && i.EndHalt.NumberInRoute > startHalt.NumberInRoute 
            && !i.Returned).ToList();
            List<int> result = new List<int>();
            for (int i = 1; i <= places; i++) // перебор мест от 1 до последнего
            {
                // место не занято
                if (buffer.Where(j => j.Place == i).FirstOrDefault() == null)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public ReturnModel CheckReturn(int id, string email) // определить сумму к возврату билета
        {
            var ticket = db.GetTicket(id); // определение билета
            ReturnModel result = new ReturnModel(); // данные по возврату билета
            if (ticket != null && ticket.Email == email) // билет найден и покупатель совпадает
            {
                result.Cost = ticket.Cost; // сумма к возврату
                // проверка уважительных причин отмены билета
                if (ticket.Closed || ticket.Rplace || ticket.Rtime || ticket.Returned)
                {
                    if (ticket.Rplace)
                    {
                        result.Status = "Изменение места";
                    }
                    if (ticket.Rtime)
                    {
                        result.Status = "Изменение времени";
                    }
                    if (ticket.Closed)
                    {
                        result.Status = "Поездка отменена";
                    }
                    if (ticket.Returned)
                    {
                        result.Status = "Выполнен возврат";
                        result.Cost = 0;
                    }
                }
                else
                {
                    TimeSpan time = ticket.StartDate - DateTime.Now; // определение времени до отправки
                    if (time.TotalMinutes >= 120)
                    {
                        result.Status = "Более 2 часов до отправки рейса. Возврат 95%.";
                        result.Cost = (int)(ticket.Cost * 0.95);
                    }
                    else
                    {
                        if (time.TotalMinutes >= 0)
                        {
                            result.Status = "Менее 2 часов до отправки рейса. Возврат 85%.";
                            result.Cost = (int)(ticket.Cost * 0.85);
                        }
                        else
                        {
                            result.Status = "Рейс уже отправлен";
                            result.Cost = 0;
                        }
                    }
                }
            }
            else
            {
                result.Cost = 0;
                result.Status = "Билет не найден";
            }
            return result;
        }

        public void Return(int id, int newCost) // выполнение возврата билета
        {
            TicketDTO ticket = db.GetTicket(id);
            if (ticket != null)
            {
                ticket.Cost -= newCost;
                ticket.Returned = true;
                db.UpdateTicket(ticket);
            }
        }
        // получение списка возможных рейсов для выбранных даты и начальных / конечных остановок
        public List<TravellDTO> getTravells(SearchInfoDTO info) 
        {
            List<TravellDTO> result = new List<TravellDTO>();
            int fDay = ((int)info.Date.DayOfWeek + 6) % 7 + 1; // перевод нумерации дней недели к формату 1 - понедельник...
            List<RouteDTO> routes = db.GetRoutes().Where(i => i.Hidden == 0).ToList();
            var routeHalts = db.GetRouteHalts();
            if (routeHalts == null)
            {
                return result;
            }
            foreach (RouteDTO r in routes) // перебор всех маршрутов
            {
                RouteHaltDTO start = routeHalts.Where(i => i.RouteId == r.RouteId  // поиск в маршруте начальной остановки
                && i.Halt.halt_id == info.Start && i.Hidden == 0).SingleOrDefault(); // поиск в маршруте конечной остановки
                RouteHaltDTO end = routeHalts.Where(i => i.RouteId == r.RouteId 
                && i.Halt.halt_id == info.End && i.Hidden == 0).SingleOrDefault();
                // обе остановки найдены и конечная в маршруте после начальной
                if (start != null && end != null && end.NumberInRoute > start.NumberInRoute)
                {
                    var rCruises = r.Cruises.Where(i => i.RouteId == r.RouteId).ToList();
                    foreach (CruiseDTO c in rCruises)
                    {
                        // определение разницы в дне отправки рейса и дне, выбранном пользователем
                        int days = c.Day.Id < fDay ? fDay - c.Day.Id : 7 - (c.Day.Id - fDay);
                        if (days == 7)
                        {
                            days = 0;
                        }
                        DateTime bufDate = info.Date.Date.AddDays(-days);
                        bufDate = bufDate.AddHours(Int32.Parse(c.Time.Substring(0,2))); // добавление к времени время отправки рейса
                        bufDate = bufDate.AddMinutes(Int32.Parse(c.Time.Substring(3, 2)));
                        bufDate = bufDate.AddMinutes(start.Time); // добавление времени прибытия к начальной остановке
                        if (bufDate.Date == info.Date && bufDate > DateTime.Now) // рейс отправляется в нужную дату
                        {
                            result.Add(new TravellDTO(c, start, end, bufDate));
                        }
                    }
                }
            }
            return result;
        }

        public int Buy(TravellDTO travell, string email, int place) // покупка билета
        {
            DBOperations db = new DBOperations();
            TicketDTO ticket = new TicketDTO()
            {
                Closed = false,
                Cost = travell.Cost,
                CruiseId = travell.CruiseId,
                Date = travell.Date,
                Email = email,
                EndHalt = travell.End,
                StartHalt = travell.Start,
                Place = place,
                Returned = false,
                RouteId = travell.RouteId,
                Rplace = false,
                Rtime = false,
                SellingTime = DateTime.Now,
                StartDate = travell.StartDate
            };
            if (ticket.StartDate > DateTime.Now) // рейс не просрочен
            {
                ticket.TicketId = db.AddTicket(ticket);  // внутренняя ошибка добавления билета
                if (ticket.TicketId == -1)
                {
                    return -1;
                }
                var tickets = db.GetTickets();
                // проверка, что место в билете не было занято во время оформления
                if (tickets.Where(i => i.StartDate == ticket.StartDate && i.Place == ticket.Place 
                && i.Closed == false && i.Returned == false 
                && i.StartHalt.NumberInRoute < ticket.EndHalt.NumberInRoute 
                && i.EndHalt.NumberInRoute > ticket.StartHalt.NumberInRoute).Count() > 1)
                {
                    db.RemoveTicket(ticket.TicketId); // удаление дублирующегося билета
                    return 2;
                }
            }
            else
            {
                return 0;
            }
            // отправка билета покупателю по почте
            EmailOperations emailOperations = new EmailOperations();
            emailOperations.Ticket(ticket, email);
            return 1;
        }
    }
}
