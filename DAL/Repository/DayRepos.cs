using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repository
{
    public class DayRepos : IRepository<Day>
    {
        private WBSTOContext db;

        public DayRepos(WBSTOContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Day> GetList()
        {
            return db.Days.ToList();
        }

        public Day GetItem(int id)
        {
            return db.Days.Find(id);
        }

        public void Create(Day item)
        {
            db.Days.Add(item);
        }

        public void Update(Day item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Day item = db.Days.Find(id);
            if (item != null)
                db.Days.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
