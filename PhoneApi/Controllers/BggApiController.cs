using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Dapper;
using PhoneApi.Models;
using PhoneApi.Utils;
using PhoneAPI.Models;
using PhoneAPI.Utils;

namespace PhoneApi.Controllers
{
    [RoutePrefix("bggapi")]
    public class BggApiController : ApiController
    {
        [HttpGet]
        [Route("updateGamesList")]
        public async System.Threading.Tasks.Task<ApiResponse<Credentials>> updateGamesList()
        {
            var response = new ApiResponse<Credentials>();

            List<long> gamesIds = null;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                gamesIds = connection.Query<long>($"select * from test").ToList();
            }

            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            string currentRequest = "";
            int numberOfGames = 0;
            foreach (long id in gamesIds)
            {
                if (numberOfGames >= 99)
                {
                    currentRequest = currentRequest + id;

                    string url = "https://www.boardgamegeek.com/xmlapi/boardgame/" + currentRequest;

                    HttpResponseMessage test = _client.GetAsync(new Uri(url)).Result;

                    EncodingProvider provider = new CustomUtf8EncodingProvider();
                    Encoding.RegisterProvider(provider);

                    if (test.StatusCode == HttpStatusCode.OK)
                    {
                        XDocument xdoc = XDocument.Parse(test.Content.ReadAsStringAsync().Result);

                        StringReader sr = new StringReader(xdoc.ToString());

                        XmlSerializer serializer = new XmlSerializer(typeof(boardgames));

                        boardgames result = (boardgames)serializer.Deserialize(sr);

                        var responsegameslist = result.boardgame.ToList();
                        foreach (var game in responsegameslist)
                        {

                            GamesList gamelist = new GamesList();
                            List<GamesMechanic> mechanicslist = new List<GamesMechanic>();
                            List<GamesFamily> familieslist = new List<GamesFamily>();

                            gamelist.id = Convert.ToInt64(game.objectid);

                            var nameList = game.name.ToList();
                            foreach (var name in nameList)
                            {
                                if (name.primary != null && name.primary == "true")
                                {
                                    gamelist.name = name.Value;
                                }
                            }

                            gamelist.minPlayers = Convert.ToInt32(game.minplayers);
                            gamelist.maxPlayers = Convert.ToInt32(game.maxplayers);
                            gamelist.thumbnail = game.thumbnail;

                            var mechanicList = game.boardgamemechanic?.ToList();
                            if (mechanicList != null)
                            {
                                foreach (var mechanic in mechanicList)
                                {
                                    GamesMechanic gameMechanic = new GamesMechanic();
                                    gameMechanic.id = Convert.ToInt64(game.objectid);
                                    gameMechanic.mechanic = mechanic.Value;
                                    mechanicslist.Add(gameMechanic);
                                }
                            }

                            var familyList = game.boardgamefamily?.ToList();
                            if (familyList != null)
                            {
                                foreach (var family in familyList)
                                {
                                    GamesFamily gameFamily = new GamesFamily();
                                    gameFamily.id = Convert.ToInt64(game.objectid);
                                    gameFamily.family = family.Value;
                                    familieslist.Add(gameFamily);
                                }
                            }


                            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
                            {
                                connection.Query<GamesList>($"dbo.[InsertGameinList] @id, @name, @minPlayers, @maxPlayers, @thumbnail",
                                    new { gamelist.id, gamelist.name, gamelist.minPlayers, gamelist.maxPlayers, gamelist.thumbnail });
                                foreach (GamesMechanic mechanic in mechanicslist)
                                {
                                    connection.Query<GamesMechanic>($"dbo.[InsertGameMechanics] @id, @mechanic",
                                        new { mechanic.id, mechanic.mechanic });
                                }

                                foreach (GamesFamily family in familieslist)
                                {
                                    connection.Query<GamesFamily>($"dbo.[InsertGameFamilies] @id, @family",
                                        new { family.id, family.family });
                                }
                            }

                        }

                    }

                    currentRequest = "";
                    numberOfGames = 0;
                    Thread.Sleep(5000);
                }
                else
                {
                    currentRequest = currentRequest + id + ",";
                    numberOfGames += 1;
                }

            }
            return response;
        }

    }
}
