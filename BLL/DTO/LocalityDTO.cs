using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class LocalityDTO
    {
        public int locality_id { get; set; }
        public string locality_name { get; set; }

        public LocalityDTO()
        {

        }

        public LocalityDTO(Locality locality)
        {
            locality_id = locality.LocalityId;
            locality_name = locality.LocalityName;
        }
    }
}
