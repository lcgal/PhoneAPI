using System;

namespace PhoneAPI.Models
{
    public class GamesList
    {
        public long id { get; set; }
        public String name { get; set; }
        public int minPlayers { get; set; }
        public int maxPlayers { get; set; }
        public String thumbnail { get; set; }
    }
}
