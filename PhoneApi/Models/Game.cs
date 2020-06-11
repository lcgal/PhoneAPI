using System;
using System.Collections.Generic;

namespace PhoneAPI.Models
{
    public class Game
    {
        public long id { get; set; }
        public String name { get; set; }
        public int minPlayers { get; set; }
        public int maxPlayers { get; set; }
        public String thumbnail { get; set; }

        public List<GameMechanic> mechanics { get; set; }
        public List<GameFamily> families { get; set; }
    }
}
