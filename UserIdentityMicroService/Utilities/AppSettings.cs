using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserIdentityMicroService.Utilities
{
    public class AppSettings
    {
        public string Secret => "secret key for movie ticket booking application";
        public string ConnectionStrings { get; set; }
    }
}
