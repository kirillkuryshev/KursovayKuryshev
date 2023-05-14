using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Scaffold-DbContext "Server=DESKTOP-TSE3JH5\SQLEXPRESS;Database=WBSTO;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer

namespace DAL.Interfaces
{
    public interface IDbRepos // интерфейс для взаимодействия с репозиториями
    {
        IRepository<Cruise> Cruise { get; }
        IRepository<Halt> Halt { get; }
        IRepository<Locality> Locality { get; }
        IRepository<Route> Route { get; }
        IRepository<RouteHalt> RouteHalt { get; }
        IRepository<Ticket> Ticket { get; }
        IRepository<Day> Day { get; }
    }
}
