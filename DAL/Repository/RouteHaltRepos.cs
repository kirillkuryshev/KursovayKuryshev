using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class RouteHaltRepos : IRepository<RouteHalt>
    {
        private WBSTOContext db;

        public RouteHaltRepos(WBSTOContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<RouteHalt> GetList()
        {
            return db.RouteHalts.Include(p => p.Halt).ToList();
        }

        public RouteHalt GetItem(int id)
        {
            return db.RouteHalts.Include(p => p.Halt).SingleOrDefault(p => p.RouteHaltId == id);
        }

        public void Create(RouteHalt item)
        {
            db.RouteHalts.Add(item);
        }

        public void Update(RouteHalt item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            RouteHalt item = db.RouteHalts.Find(id);
            if (item != null)
                db.RouteHalts.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
