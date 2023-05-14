using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class CruiseRepos : IRepository<Cruise>
    {
        private WBSTOContext db;

        public CruiseRepos(WBSTOContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Cruise> GetList()
        {
            return db.Cruises.Include(p => p.DayNavigation).ToList();
        }

        public Cruise GetItem(int id)
        {
            return db.Cruises.Include(p => p.DayNavigation).Where(p => p.CruiseId == id).SingleOrDefault();
        }

        public void Create(Cruise item)
        {
            db.Cruises.Add(item);
        }

        public void Update(Cruise item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Cruise item = db.Cruises.Find(id);
            if (item != null)
                db.Cruises.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
