using BLL.DTO;
using DAL;
using DAL.Interfaces;
using DAL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Operations
{
    public class DBOperations
    {
        IDbRepos db; // реализация Repository
        ILogger logger; // логгер

        public DBOperations()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<DBOperations>();
            try
            {
                db = new DBRepos();
            }
            catch
            {
                logger.LogError("Ошибка подключения к базе данных");
            }
        }

        #region Day R operations
        public DayDTO GetDay(int id) // получение дня по номеру
        {
            try
            {
                Day l = db.Day.GetItem(id);
                return l == null ? null : new DayDTO(l);
            }
            catch
            {
                logger.LogError("Ошибка получения дня с номером " + id);
                return null;
            }
        }

        public List<DayDTO> GetDays() // получение списка дней
        {
            try
            {
                return db.Day.GetList().Select(i => new DayDTO(i)).ToList();
            }
            catch
            {
                logger.LogError("Ошибка получения списка дней");
                return new List<DayDTO>();
            }
        }

        #endregion

        #region Locality R operations
        public LocalityDTO GetLocality(int id) // получение населенного пункта по номеру
        {
            try
            {
                Locality l = db.Locality.GetItem(id);
                return l == null ? null : new LocalityDTO(l);
            }
            catch
            {
                logger.LogError("Ошибка получения населенного пункта с номером " + id);
                return null;
            }
        }

        public List<LocalityDTO> GetLocalities() // получение списка населенных пунктов
        {
            try
            {
                return db.Locality.GetList().Select(i => new LocalityDTO(i)).ToList();
            }
            catch
            {
                logger.LogError("Ошибка получения списка населенных пунктов");
                return null;
            }
        }

        #endregion

        #region Halt CRUD operations
        public List<HaltDTO> GetHalts() // получение списка остановок
        {
            try
            {
                return db.Halt.GetList().Select(i => new HaltDTO(i)).ToList();
            }
            catch
            {
                logger.LogError("Ошибка получения списка остановок");
                return null;
            }
        }

        public HaltDTO GetHalt(int id) // получение остановки
        {
            try
            {
                Halt h = db.Halt.GetItem(id);
                return h == null ? null : new HaltDTO(h);
            }
            catch
            {
                logger.LogError("Ошибка получения остановки с номером " + id);
                return null;
            }
        }

        public int AddHalt(HaltDTO halt) // добавление новой остановки
        {
            Halt newHalt = new Halt();
            newHalt.Adress = halt.adress;
            newHalt.LocalityId = halt.locality_model.locality_id;
            newHalt.Hidden = halt.hidden;
            try
            {
                db.Halt.Create(newHalt);
                db.Halt.Save();
                return newHalt.HaltId;
            }
            catch
            {
                logger.LogError("Ошибка добавления остановки");
                return -1;
            }
        }

        public bool UpdateHalt(HaltDTO halt) // обновление информации об остановке
        {
            try
            {
                Halt updatedHalt = db.Halt.GetItem(halt.halt_id);
                updatedHalt.Adress = halt.adress;
                updatedHalt.LocalityId = halt.locality_model.locality_id;
                updatedHalt.Hidden = halt.hidden;
                db.Halt.Update(updatedHalt);
                db.Halt.Save();
            }
            catch
            {
                logger.LogError("Ошибка обновления остановки");
                return false;
            }
            return true;
        }
        #endregion

        #region RouteHalt CRUD operations
        public List<RouteHaltDTO> GetRouteHalts() // получение списка остановок маршрута
        {
            try
            {
                return db.RouteHalt.GetList().Select(i => new RouteHaltDTO(i)).ToList();
            }
            catch
            {
                logger.LogError("Ошибка получения списка остановок маршрута");
                return null;
            }
        }

        public RouteHaltDTO GetRouteHalt(int id) // получение остановки маршрута по номеру
        {
            try
            {
                RouteHalt h = db.RouteHalt.GetItem(id);
                return h == null ? null : new RouteHaltDTO(h);
            }
            catch
            {
                logger.LogError("Ошибка получения остановки маршрута с номером " + id);
                return null;
            }   
        }

        public int AddRouteHalt(RouteHaltDTO halt) // добавление остановки маршрута
        {
            RouteHalt newHalt = new RouteHalt();
            newHalt.HaltId = halt.Halt.halt_id;
            newHalt.Cost = halt.Cost;
            newHalt.Hidden = halt.Hidden;
            newHalt.RouteId = halt.RouteId;
            newHalt.NumberInRoute = halt.NumberInRoute;
            newHalt.Time = halt.Time;
            try
            {
                db.RouteHalt.Create(newHalt);
                db.RouteHalt.Save();
                return newHalt.HaltId;
            }
            catch
            {
                logger.LogError("Ошибка добавления остановки маршрута");
                return -1;
            }  
        }

        public bool UpdateRouteHalt(RouteHaltDTO halt) // обновление остановки маршрута
        {
            try
            {
                RouteHalt updatedHalt = db.RouteHalt.GetItem(halt.RouteHaltId);
                updatedHalt.HaltId = halt.Halt.halt_id;
                updatedHalt.Cost = halt.Cost;
                updatedHalt.Hidden = halt.Hidden;
                updatedHalt.RouteId = halt.RouteId;
                updatedHalt.NumberInRoute = halt.NumberInRoute;
                updatedHalt.Time = halt.Time;
                db.RouteHalt.Update(updatedHalt);
                db.RouteHalt.Save();
            }
            catch
            {
                logger.LogError("Ошибка обновления остановки маршрута с номером " + halt.RouteHaltId);
                return false;
            }
            return true;
        }
        #endregion

        #region Cruise CRUD operations

        public List<CruiseDTO> GetCruises() // получение списка рейсов
        {
            try
            {
                return db.Cruise.GetList().Select(i => new CruiseDTO(i)).ToList();
            }
            catch
            {
                logger.LogError("Ошибка получения списка рейсов");
                return new List<CruiseDTO>();
            } 
        }

        public CruiseDTO GetCruise(int id) // получение рейса по номеру
        {
            try
            {
                Cruise h = db.Cruise.GetItem(id);
                return h == null ? null : new CruiseDTO(h);
            }
            catch
            {
                logger.LogError("Ошибка получения рейса с номером " + id);
                return null;
            }
        }

        public int AddCruise(CruiseDTO cruise) // добавление нового рейса
        {
            Cruise newCruise = new Cruise();
            newCruise.Day = cruise.Day.Id;
            newCruise.Places = cruise.Places;
            newCruise.Time = cruise.Time;
            newCruise.RouteId = cruise.RouteId;
            newCruise.Hidden = cruise.Hidden;
            try
            {
                db.Cruise.Create(newCruise);
                db.Cruise.Save();
                return newCruise.CruiseId;
            }
            catch
            {
                logger.LogError("Ошибка добавления рейса");
                return -1;
            }
        }

        public bool UpdateCruise(CruiseDTO cruise) // обновление рейса
        {
            try
            {
                Cruise updatedCruise = db.Cruise.GetItem(cruise.CruiseId);
                updatedCruise.Day = cruise.Day.Id;
                updatedCruise.Time = cruise.Time;
                updatedCruise.Places = cruise.Places;
                updatedCruise.RouteId = cruise.RouteId;
                updatedCruise.Hidden = cruise.Hidden;
                db.Cruise.Update(updatedCruise);
                db.Cruise.Save();
            }
            catch
            {
                logger.LogError("Ошибка обновления рейса с номером " + cruise.CruiseId);
                return false;
            }
            return true;
        }

        #endregion

        #region Route CRUD operations

        public List<RouteDTO> GetRoutes() // получение списка маршрутов
        {
            try
            {
                return db.Route.GetList().Select(i => new RouteDTO(i)).ToList();
            }
            catch
            {
                logger.LogError("Ошибка получения списка маршрутов");
                return new List<RouteDTO>();
            }
        }

        public RouteDTO GetRoute(int id) // получение маршрута по номеру
        {
            try
            {
                Route h = db.Route.GetItem(id);
                return h == null ? null : new RouteDTO(h);
            }
            catch
            {
                logger.LogError("Ошибка получения маршрута с номером " + id);
                return null;
            }
        }

        public int AddRoute(RouteDTO Route) // добавление маршрута
        {
            try
            {
                Route newRoute = new Route();
                newRoute.Hidden = Route.Hidden;
                db.Route.Create(newRoute);
                db.Route.Save();
                return newRoute.RouteId;
            }
            catch
            {
                logger.LogError("Ошибка добавления маршрута");
                return -1;
            }
        }

        public bool UpdateRoute(RouteDTO Route) // обновление маршрута
        {
            try
            {
                Route updatedRoute = db.Route.GetItem(Route.RouteId);
                updatedRoute.Hidden = Route.Hidden;
                db.Route.Update(updatedRoute);
                db.Route.Save();
            }
            catch
            {
                logger.LogError("Ошибка обновления маршрута с номером " + Route.RouteId);
                return false;
            }
            return true;
        }

        #endregion

        #region Ticket CRUD operations

        public List<TicketDTO> GetTickets() // получение списка билетов
        {
            try
            {
                return db.Ticket.GetList().Select(i => new TicketDTO(i)).ToList();
            }
            catch
            {
                logger.LogError("Ошибка получения списка билетов");
                return new List<TicketDTO>();
            }
        }

        public TicketDTO GetTicket(int id) // получение билета по номеру
        {
            try
            {
                Ticket h = db.Ticket.GetItem(id);
                return h == null ? null : new TicketDTO(h);
            }
            catch
            {
                logger.LogError("Ошибка получения билета с номером " + id);
                return null;
            }
        }

        public int AddTicket(TicketDTO Ticket) // добавление билета
        {
            Ticket newTicket = new Ticket();
            newTicket.Email = Ticket.Email;
            newTicket.RouteId = Ticket.RouteId;
            newTicket.CruiseId = Ticket.CruiseId;
            newTicket.Date = Ticket.Date;
            newTicket.Place = Ticket.Place;
            newTicket.Returned = false;
            newTicket.SellingTime = DateTime.Now;
            newTicket.StartDate = Ticket.StartDate;
            newTicket.Cost = Ticket.Cost;
            newTicket.StartHaltId = Ticket.StartHalt.RouteHaltId;
            newTicket.EndHaltId = Ticket.EndHalt.RouteHaltId;
            newTicket.Rplace = false;
            newTicket.Rtime = false;
            newTicket.Closed = false;
            try
            {
                db.Ticket.Create(newTicket);
                db.Ticket.Save();
                return newTicket.TicketId;
            }
            catch
            {
                logger.LogError("Ошибка добавления билета");
                return -1;
            }
        }

        public void UpdateTicket(TicketDTO Ticket) // обновление билета
        {
            try
            {
                Ticket updatedTicket = db.Ticket.GetItem(Ticket.TicketId);
                updatedTicket.Email = Ticket.Email;
                updatedTicket.RouteId = Ticket.RouteId;
                updatedTicket.CruiseId = Ticket.CruiseId;
                updatedTicket.Date = Ticket.Date;
                updatedTicket.Place = Ticket.Place;
                updatedTicket.Returned = Ticket.Returned;
                updatedTicket.SellingTime = Ticket.SellingTime;
                updatedTicket.StartDate = Ticket.StartDate;
                updatedTicket.Cost = Ticket.Cost;
                updatedTicket.StartHaltId = Ticket.StartHalt.RouteHaltId;
                updatedTicket.EndHaltId = Ticket.EndHalt.RouteHaltId;
                updatedTicket.Rplace = Ticket.Rplace;
                updatedTicket.Rtime = Ticket.Rtime;
                updatedTicket.Closed = Ticket.Closed;
                db.Ticket.Update(updatedTicket);
                db.Ticket.Save();
            }
            catch
            {
                logger.LogError("Ошибка обновления билета с номером " + Ticket.TicketId + 
                    "\nНовые данные: " + Ticket.ToString());
            }
            finally 
            {
                EmailOperations emailOperations = new EmailOperations();
                emailOperations.Ticket(Ticket, Ticket.Email); // отправка обновленного билета
            }
        }

        public void RemoveTicket(int id) // удаление билета
        {
            try
            {
                db.Ticket.Delete(id);
                db.Ticket.Save();
            }
            catch
            {
                logger.LogError("Ошибка удаления билета");
            }
        }

        #endregion
    }
}
