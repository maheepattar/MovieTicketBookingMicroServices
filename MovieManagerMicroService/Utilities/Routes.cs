using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.Utilities
{
    /// <summary>
    /// Defines routes
    /// </summary>
    public class Routes
    {
        public const string GetCities = "city";

        public const string MultiplexesByCity = "multiplex/{cityId}";

        public const string MoviesByMultiplex = "movie/multiplex/{multiplexId}";

        public const string MoviesByLanguage = "movie/language/{language}";

        public const string MoviesByGenre = "movie/genre/{genre}";
    }
}
