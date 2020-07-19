using PhoneApi.Models;
using PhoneApi.Models.Responses;
using PhoneAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PhoneApi.Controllers
{
    public class GameRoomsController : ApiController
    {
        [HttpPost]
        [Route("offergame")]
        public BooleanResponse OfferGame([FromBody]Profile profile)
        {


            return null;

        }
    }
}
