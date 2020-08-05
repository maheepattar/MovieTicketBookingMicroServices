using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        [Required]
        public string SeatNo { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string DateToPresent { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int MovieId { get; set; }
    }
}
