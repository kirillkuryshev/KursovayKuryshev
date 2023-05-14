using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class LocalitiesRepos : IRepository<Locality>
    {
        private WBSTOContext db;

        public LocalitiesRepos(WBSTOContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Locality> GetList()
        {
            return db.Localities.ToList();
        }

        public Locality GetItem(int id)
        {
            return db.Localities.Find(id);
        }

        public void Create(Locality item)
        {
            db.Localities.Add(item);
        }

        public void Update(Locality item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Locality item = db.Localities.Find(id);
            if (item != null)
                db.Localities.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
