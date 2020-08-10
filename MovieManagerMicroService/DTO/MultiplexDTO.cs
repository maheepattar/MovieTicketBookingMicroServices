using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.DTO
{
    public class MultiplexDTO
    {
        public int Id { get; set; }

        [Required]
        public string MultiplexName { get; set; }
        
        [Required]        
        public int CityId { get; set; }
    }
}
