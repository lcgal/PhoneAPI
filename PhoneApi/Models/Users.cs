using System;

namespace PhoneAPI.Models
{
    public class User
    {
        Guid UserId { get; set; }
        String Email { get; set; }
        long Latitude { get; set; }
        long Longitude { get; set; }
        String Description { get; set; }
    }
}


