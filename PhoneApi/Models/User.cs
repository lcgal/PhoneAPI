using System;

namespace PhoneAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public long Latitude { get; set; }

        public long Longitude { get; set; }

        public string Description { get; set; }

        public string PhotoUrl { get; set; }
    }
}


