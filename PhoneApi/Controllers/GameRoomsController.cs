using Dapper;
using PhoneApi.Models;
using PhoneApi.Models.Responses;
using PhoneAPI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PhoneApi.Controllers
{
    [RoutePrefix("gamerooms")]
    public class GameRoomsController : ApiController
    {
        [HttpPost]
        [Route("offergame")]
        public ApiResponse<Guid> OfferGame([FromBody]GameOffer offer)
        {
            ApiResponse<Guid> response = new ApiResponse<Guid>();
            try
            {
                Guid id = InsertGameOffer(offer);
                if (id != null)
                {
                    response.ReturnData = id;
                    response.Result = true;
                }

            } 
            catch (Exception e)
            {
                response.Error = e.Message;
            }
            return response;

        }
        private Guid InsertGameOffer(GameOffer offer)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                return connection.Query<Guid>($"dbo.[CreateGameRoom] @gameid, @latitude, @longitude, @userid", new { offer.Gameid, offer.Latitude, offer.Longitude, offer.UserId }).FirstOrDefault();
            }
        }
    }
}
