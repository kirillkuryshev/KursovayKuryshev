using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.DTO
{
    public class RouteDTO
    {
        // номер маршрута
        public int RouteId { get; set; }
        public int Hidden { get; set; }
        // рейсы маршрута
        public List<CruiseDTO> Cruises { get; set; }
        // остановки маршрута
        public List<RouteHaltDTO> RouteHalts { get; set; }

        public RouteDTO()
        {

        }

        public RouteDTO(Route route)
        {
            RouteId = route.RouteId;
            Hidden = route.Hidden;
            Cruises = route.Cruises.Select(i => new CruiseDTO(i)).ToList();
            RouteHalts = route.RouteHalts.Select(i => new RouteHaltDTO(i)).ToList();
        }
    }
}
