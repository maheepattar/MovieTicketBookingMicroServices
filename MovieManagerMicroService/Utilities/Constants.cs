using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.Utilities
{
    public class Constants
    {
        public const string MovieExist = "Another movie in this multiplex has been scheduled at the same time";

        public const string UnknownError = "Error occured while processing request. Try again.";

        public const string NoCities = "No Cities found currently.";

        public const string NoMultiplexes = "No Multiplexes found currently for the selected city";

        public const string NoMovies = "No Movies found currently for the selected Multiplex";

        public const string NoMoviesByLanguage = "No Movies found currently for the selected Language";

        public const string NoMoviesByGenre = "No Movies found currently for the selected Genre";

        public const string MissingOrInvalidBody = "Missing or invalid. Some of the values may be incorrect";

        public static string InvalidId(string idType)
        {
            return $"Invalid or missing {idType}";
        }

    }
}