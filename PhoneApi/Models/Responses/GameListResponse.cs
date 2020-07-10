using PhoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models.Responses
{
    public class GameListResponse
    {
        public bool update { get; set; }

        public String version { get; set; }

        public List<Game> data { get; set; }
    }
}