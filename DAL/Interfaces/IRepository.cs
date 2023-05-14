using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class // интерфейс репозитория
    {
        List<T> GetList(); // получить все элементы таблицы
        T GetItem(int id); // получить элемент таблицы по id
        void Create(T item); // добавить элемент в таблицу
        void Update(T item); // обновить элемент в таблице
        void Delete(int id); // удалить элемент из таблицы
        int Save();
    }
}
