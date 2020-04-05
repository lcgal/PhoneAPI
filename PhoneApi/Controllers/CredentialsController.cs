using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using PhoneAPI.Models;
using PhoneAPI.Utils;

namespace PhoneApi.Controllers
{
    [RoutePrefix("Credentials")]
    public class CredentialsController : ApiController
    {
        [HttpPost]
        [Route("Login/{username}/{password}")]
        public ApiResponse<Credentials> Login(string username, string password)
        {
            var response = new ApiResponse<Credentials>();

            Credentials credentials = GetCredentials(username);



            return response;
        }


        private Credentials GetCredentials(String username)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                return connection.Query<Credentials>($"select * from credentials where usrname = '{username}'").ToList().FirstOrDefault();
            }
        }
    }
}
