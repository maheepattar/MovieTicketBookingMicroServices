using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DBEntities
{
    public class Multiplex
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MultiplexName { get; set; }


        [ForeignKey("CityId")]
        public City City { get; set; }
        public int CityId { get; set; }

        public ICollection<Booking> Bookings = new List<Booking>();
    }
}
