using BLL.Operations;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class HaltDTO
    {
        // номер остановки
        public int halt_id { get; set; }
        // название остановки
        public string adress { get; set; }
        // населенный пункт
        public LocalityDTO locality_model { get; set; }
        // скрыта или нет
        public int hidden { get; set; }
        public HaltDTO()
        {

        }

        public HaltDTO(Halt halt)
        {
            halt_id = halt.HaltId;
            adress = halt.Adress;
            hidden = halt.Hidden;
            if (halt.Locality == null)
            {
                DBOperations db = new DBOperations();
                locality_model = db.GetLocality(halt.LocalityId);
            }
            else
            {
                locality_model = new LocalityDTO(halt.Locality);
            }
        }
    }
}
