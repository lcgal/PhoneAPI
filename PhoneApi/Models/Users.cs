using System;

namespace PhoneAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public String Email { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public String Description { get; set; }
    }
}


