using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.DBEntities
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Movie_Name { get; set; }
        public string Movie_Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public string MovieLanguage { get; set; }
        [ForeignKey("MultiplexId")]
        public Multiplex Multiplex { get; set; }
        public int MultiplexId { get; set; }
        [Required]
        public string Genre { get; set; }
    }
}
