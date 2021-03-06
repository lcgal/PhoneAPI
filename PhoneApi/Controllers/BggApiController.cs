﻿using System;
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

                            Game gameObj = new Game();
                            List<GameMechanic> mechanicslist = new List<GameMechanic>();
                            List<GameFamily> familieslist = new List<GameFamily>();

                            gameObj.Id = Convert.ToInt64(game.objectid);

                            var nameList = game.name.ToList();
                            foreach (var name in nameList)
                            {
                                if (name.primary != null && name.primary == "true")
                                {
                                    gameObj.Name = name.Value;
                                }
                            }

                            gameObj.MinPlayers = Convert.ToInt32(game.minplayers);
                            gameObj.MaxPlayers = Convert.ToInt32(game.maxplayers);
                            gameObj.Thumbnail = game.thumbnail;

                            var mechanicList = game.boardgamemechanic?.ToList();
                            if (mechanicList != null)
                            {
                                foreach (var mechanic in mechanicList)
                                {
                                    GameMechanic gameMechanic = new GameMechanic();
                                    gameMechanic.GameId = Convert.ToInt64(game.objectid);
                                    gameMechanic.Mechanic = mechanic.Value;
                                    mechanicslist.Add(gameMechanic);
                                }
                            }

                            var familyList = game.boardgamefamily?.ToList();
                            if (familyList != null)
                            {
                                foreach (var family in familyList)
                                {
                                    GameFamily gameFamily = new GameFamily();
                                    gameFamily.GameId = Convert.ToInt64(game.objectid);
                                    gameFamily.Family = family.Value;
                                    familieslist.Add(gameFamily);
                                }
                            }


                            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
                            {
                                connection.Query<Game>($"dbo.[InsertGameinList] @id, @name, @minPlayers, @maxPlayers, @thumbnail",
                                    new { gameObj.Id, gameObj.Name, gameObj.MinPlayers, gameObj.MaxPlayers, gameObj.Thumbnail });
                                foreach (GameMechanic mechanic in mechanicslist)
                                {
                                    connection.Query<GameMechanic>($"dbo.[InsertGameMechanics] @id, @mechanic",
                                        new { mechanic.GameId, mechanic.Mechanic });
                                }

                                foreach (GameFamily family in familieslist)
                                {
                                    connection.Query<GameFamily>($"dbo.[InsertGameFamilies] @id, @family",
                                        new { family.GameId, family.Family });
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
