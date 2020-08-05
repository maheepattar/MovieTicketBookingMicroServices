using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DBEntities
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string SeatNo { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime DateToPresent { get; set; }
        public int Amount { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
    }
}
