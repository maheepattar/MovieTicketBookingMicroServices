using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.Utilities
{
    public class AppSetting
    {
        public static string Secret => "this_is_our_supper_long_security_key_for_token_validation_project_movieticket$smesk.in";
        public string ConnectionStrings { get; set; }
    }
}
