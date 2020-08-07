using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserIdentityMicroService.Utilities
{
    public class Constants
    {
        public const string WrongCredentials = "Username or password is not correct";

        public const string InvalidObject = "Invalid or missing userdata values in the body";

        public const string UnknownError = "Error occured while processing request. Try again.";
    }
}
