using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.DTO
{
    /// <summary>
    /// City DTO
    /// </summary>
    public class CityDTO
    {
        /// <summary>
        /// Gets or sets City id
        /// </summary>
        [Key]
        public int CityId { get; set; }

        /// <summary>
        /// Gets or sets City name
        /// </summary>
        public string CityName { get; set; }
    }
}
