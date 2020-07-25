using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models.Payloads
{
    public class UserLocationPayload
    {
        public Guid UserId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}