using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Dapper;
using PhoneAPI.Models;
using PhoneAPI.Utils;

namespace PhoneApi.Controllers
{
    [RoutePrefix("credentials")]
    public class CredentialsController : ApiController
    {
        [HttpGet]
        [Route("login/{username}/{password}")]
        public ApiResponse<Credentials> Login(string username, string password)
        {
            var response = new ApiResponse<Credentials>();

            //Credentials credentials = GetCredentials(username);



            return response;
        }

        [HttpPost]
        [Route("fblogin")]
        public ApiResponse<Credentials> FacebookLogin([FromBody]Credentials creds) 
        { 
            var response = new ApiResponse<Credentials>();
            response.Result = false;

            Credentials credentials = GetFacebookCredentials(creds.FbId);
            
            if (credentials != null)
            {
                response.Result = true;
                response.ReturnData = credentials;
            }

            return response;
        }


        private Credentials GetCredentials(String username)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                var result = connection.Query<Credentials>($"dbo.getFacebookCredentials @fbid'{username}'").ToList().FirstOrDefault();
                if (result == null)
                {
                    Credentials newCredentials = connection.Query<Credentials>($"select * from credentials where usrname = '{username}'").ToList().FirstOrDefault();
                }
                return result;
            }
        }

        private Credentials GetFacebookCredentials(String facebookId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                Credentials result = connection.Query<Credentials>("dbo.getFacebookCredentials @fbid",new {fbid = facebookId }).ToList().FirstOrDefault();
                if (result == null)
                {
                    result = connection.Query<Credentials>("dbo.CreateFacebookCredentials @fbid", new { fbid = facebookId }).ToList().FirstOrDefault();
                }
                return result;
            }
        }

        private Credentials CreateFacebookCredentials(String facebookId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                Guid userId = new Guid();
                return connection.Query<Credentials>($"insert into credentials (UserId,FbId) values ('{userId}','{facebookId}'").ToList().FirstOrDefault();
            }
        }
    }
}
