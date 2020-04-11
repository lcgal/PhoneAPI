using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models
{
    public class FacebookInfo
    {
        public String id { get; set; }
        public String email { get; set; }
        public long first_name { get; set; }
        public long last_name { get; set; }
    }
}