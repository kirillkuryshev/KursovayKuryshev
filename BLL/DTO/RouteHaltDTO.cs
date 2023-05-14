using BLL.Operations;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class RouteHaltDTO
    {
        // номер остановки маршрута
        public int RouteHaltId { get; set; }
        // стоимость проезда от начала маршрута
        public int Cost { get; set; }
        public int Hidden { get; set; }
        // номер остановки на маршруте
        public int NumberInRoute { get; set; }
        // время от первой остановки
        public int Time { get; set; }

        public HaltDTO Halt { get; set; }
        public int RouteId { get; set; }

        public RouteHaltDTO()
        {

        }

        public RouteHaltDTO(RouteHalt routeHalt)
        {
            RouteHaltId = routeHalt.RouteHaltId;
            Cost = routeHalt.Cost;
            Hidden = routeHalt.Hidden;
            NumberInRoute = routeHalt.NumberInRoute;
            Time = routeHalt.Time;
            if (routeHalt.Halt != null)
            {
                Halt = new HaltDTO(routeHalt.Halt);
            }
            else
            {
                var db = new DBOperations();
                Halt = db.GetHalt(routeHalt.HaltId);
            }
            RouteId = routeHalt.RouteId;
        }
    }
}
