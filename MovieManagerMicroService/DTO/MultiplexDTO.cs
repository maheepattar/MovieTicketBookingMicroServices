using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.DTO
{
    public class MultiplexDTO
    {
        public int Id { get; set; }
        public string MultiplexName { get; set; }
        public int CityId { get; set; }
    }
}
