using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class HaltRepos : IRepository<Halt>
    {
        private WBSTOContext db;

        public HaltRepos(WBSTOContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Halt> GetList()
        {
            return db.Halts.Include(p => p.Locality).ToList();
        }

        public Halt GetItem(int id)
        {
            return db.Halts.Include(p => p.Locality).SingleOrDefault(p => p.HaltId == id);
        }

        public void Create(Halt item)
        {
            db.Halts.Add(item);
        }

        public void Update(Halt item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Halt item = db.Halts.Find(id);
            if (item != null)
                db.Halts.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
