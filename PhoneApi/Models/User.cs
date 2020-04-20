using System;

namespace PhoneAPI.Models
{
    public class User
    {
        public Guid userId { get; set; }

        public String firstName { get; set; }

        public String lastName { get; set; }

        public String email { get; set; }

        public long latitude { get; set; }

        public long longitude { get; set; }

        public String description { get; set; }

        public String photoUrl { get; set; }
    }
}


