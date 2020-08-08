using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.DTO
{
    public class MovieDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Movie_Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Movie_Description { get; set; }
        
        [Required]
        public DateTime DateAndTime { get; set; }
        
        [Required]
        public string MovieLanguage { get; set; }
        
        [Required]
        public int MultiplexId { get; set; }
        
        [Required]
        public string Genre { get; set; }
    }
}
