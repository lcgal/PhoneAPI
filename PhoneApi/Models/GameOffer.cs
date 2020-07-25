using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models
{
    public class GameOffer
    {
        public Guid Id { get; set; }

        public long Gameid { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Guid UserId { get; set; }

        public int Status { get; set; }
    }
}