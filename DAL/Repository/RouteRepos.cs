using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class RouteRepos : IRepository<Route>
    {
        private WBSTOContext db;

        public RouteRepos(WBSTOContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Route> GetList()
        {
            return db.Routes.Include(p => p.RouteHalts).Include(p => p.Cruises).ToList();
        }

        public Route GetItem(int id)
        {
            return db.Routes.Include(p => p.RouteHalts).Include(p => p.Cruises).SingleOrDefault(p => p.RouteId == id);
        }

        public void Create(Route item)
        {
            db.Routes.Add(item);
        }

        public void Update(Route item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Route item = db.Routes.Find(id);
            if (item != null)
                db.Routes.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
