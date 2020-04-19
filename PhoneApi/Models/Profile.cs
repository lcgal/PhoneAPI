using PhoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models
{
    public class Profile
    {
        public Guid UserId { get; set; }
        public Credentials credentials { get; set; }
        public User User { get; set; }

    }
}