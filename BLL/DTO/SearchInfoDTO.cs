using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class SearchInfoDTO // информация для поиска рейсов
    {
        // дата
        public DateTime Date { get; set; }
        // номер начальной остановки
        public int Start { get; set; }
        // номер конечной остановки
        public int End { get; set; }

        public SearchInfoDTO()
        {

        }

        public SearchInfoDTO(DateTime date, int start, int end)
        {
            Date = date;
            Start = start;
            End = end;
        }
    }
}
