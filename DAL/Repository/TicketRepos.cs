using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class TicketRepos : IRepository<Ticket>
    {
        private WBSTOContext db;

        public TicketRepos(WBSTOContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Ticket> GetList()
        {
            return db.Tickets.Include(p => p.StartHalt).Include(p => p.EndHalt).ToList();
        }

        public Ticket GetItem(int id)
        {
            return db.Tickets.Include(p => p.StartHalt).Include(p => p.EndHalt).SingleOrDefault(p => p.TicketId == id);
        }

        public void Create(Ticket item)
        {
            db.Tickets.Add(item);
        }

        public void Update(Ticket item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Ticket item = db.Tickets.Find(id);
            if (item != null)
                db.Tickets.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
