using PhoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models.Responses
{
    public class GameListResponse
    {
        public bool Update { get; set; }

        public string Version { get; set; }

        public List<Game> Data { get; set; }
    }
}