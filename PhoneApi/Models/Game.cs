using System;
using System.Collections.Generic;

namespace PhoneAPI.Models
{
    public class Game
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public String Thumbnail { get; set; }

        public List<GameMechanic> Mechanics { get; set; }
        public List<GameFamily> Families { get; set; }
    }
}
