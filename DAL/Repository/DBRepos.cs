using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class DBRepos : IDbRepos
    {

        private WBSTOContext db;
        private CruiseRepos cruiseRepos;
        private HaltRepos haltRepos;
        private LocalitiesRepos localitiesRepos;
        private RouteHaltRepos routeHaltRepos;
        private RouteRepos routeRepos;
        private TicketRepos ticketRepos;
        private DayRepos dayRepos;

        public DBRepos()
        {
            db = new WBSTOContext();
        }

        public IRepository<Cruise> Cruise
        {
            get
            {
                if (cruiseRepos == null)
                    cruiseRepos = new CruiseRepos(db);
                return cruiseRepos;
            }
        }
        public IRepository<Day> Day
        {
            get
            {
                if (dayRepos == null)
                    dayRepos = new DayRepos(db);
                return dayRepos;
            }
        } // аналогично Cruise

        public IRepository<Halt> Halt
        {
            get
            {
                if (haltRepos == null)
                    haltRepos = new HaltRepos(db);
                return haltRepos;
            }
        }

        public IRepository<Locality> Locality
        {
            get
            {
                if (localitiesRepos == null)
                    localitiesRepos = new LocalitiesRepos(db);
                return localitiesRepos;
            }
        }
        public IRepository<RouteHalt> RouteHalt
        {
            get
            {
                if (routeHaltRepos == null)
                    routeHaltRepos = new RouteHaltRepos(db);
                return routeHaltRepos;
            }
        }
        public IRepository<Route> Route
        {
            get
            {
                if (routeRepos == null)
                    routeRepos = new RouteRepos(db);
                return routeRepos;
            }
        }

        public IRepository<Ticket> Ticket
        {
            get
            {
                if (ticketRepos == null)
                    ticketRepos = new TicketRepos(db);
                return ticketRepos;
            }
        }

        public int Save() // сохранение изменений
        {
            return db.SaveChanges();
        }
    }
}
