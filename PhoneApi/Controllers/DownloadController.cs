using Dapper;
using Newtonsoft.Json;
using PhoneApi.Models.Responses;
using PhoneAPI.Models;
using PhoneAPI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PhoneApi.Controllers
{
    [RoutePrefix("download")]
    public class DownloadController : ApiController
    {
        [HttpGet]
        [Route("gamelist/{version}")]
        public GameListResponse GetGameList(string version)
        {
            GameListResponse response = new GameListResponse();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                string bdVersion = connection.Query<string>($"select value from settings where [key] = 'GamesListVersion'").ToList().FirstOrDefault();

                if (bdVersion == version)
                {
                    response.update = false;
                    return response;
                }

                StreamReader r = new StreamReader(@"D:\Repositorios Pessoais\games.json");
                string json = r.ReadToEnd();

                List<Game> games = JsonConvert.DeserializeObject<List<Game>>(json);
                response.version = bdVersion;
                response.data = games;
                response.update = true;
                return response;
            }
        }

        [HttpGet]
        [Route("updategamelist")]
        public void createNewGameList(string version)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {


                var results = connection.QueryMultiple(@"
                    select * from games;
                    select * from gamesmechanics;
                    select * from gamesfamilies");

                var games = results.Read<Game>();
                var mechanics = results.Read<GameMechanic>();
                var families = results.Read<GameFamily>();

                foreach (var game in games)
                {
                    game.mechanics = new List<GameMechanic>();
                    game.families = new List<GameFamily>();
                    game.mechanics.AddRange(mechanics.Where(x => x.gameId == game.id));
                    game.families.AddRange(families.Where(x => x.gameId == game.id));
                }

                string json = JsonConvert.SerializeObject(games);

                System.IO.File.WriteAllText(@"D:\Repositorios Pessoais\games.json", json);

                return;
            }
        }

    }
}
