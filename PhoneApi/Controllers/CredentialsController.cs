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
    [RoutePrefix("credentials")]
    public class CredentialsController : ApiController
    {
        [HttpGet]
        [Route("test")]
        public Credentials Login()
        {
            var response = new ApiResponse<Credentials>();

            String username = "test";

            //Credentials credentials = GetCredentials(username);
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                Credentials newCredentials = connection.Query<Credentials>($"select * from credentials where usrname = 'test'").ToList().FirstOrDefault();
                return newCredentials;
            }
        }

        [HttpPost]
        [Route("fblogin")]
        public ApiResponse<Profile> FacebookLogin([FromBody]Profile profile) 
        {

            var response = new ApiResponse<Profile>();

            try
            {
                Credentials bdCredentials = new Credentials();

                User bdUser = new User();
                response.Result = false;

            try
            {
                bdCredentials = GetFacebookCredentials(profile.Credentials.FbId);
                bdUser = GetUser(profile.User, bdCredentials.UserId);
            } catch (Exception e)
            {
                response.Result = false;
                response.Error = e.Message;
            }

                Profile bdProfile = new Profile();
                bdProfile.UserId = bdCredentials.UserId;
                bdProfile.Credentials = bdCredentials;
                bdProfile.User = bdUser;


                response.Result = true;
                response.ReturnData = bdProfile;
         

                return response;
            }
            catch (Exception e)
            {
                response.Result = false;
                response.Error = e.Message;
                return response;
            }
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
                Credentials result = connection.Query<Credentials>("dbo.GetFacebookCredentials @fbid",new {fbid = facebookId }).ToList().FirstOrDefault();
                if (result == null)
                {
                    result = connection.Query<Credentials>("dbo.CreateFacebookCredentials @fbid", new { fbid = facebookId }).ToList().FirstOrDefault();
                }
                return result;
            }
        }

        private User GetUser(User user, Guid userid)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("App")))
            {
                User result = connection.Query<User>("dbo.getUser @userId", new { userId = userid }).ToList().FirstOrDefault();
                if (result == null)
                {
                    result = connection.Query<User>("dbo.CreateUser @userid , @firstname , @lastname , @email , @photoUrl", 
                        new { userid, firstname = user.FirstName, lastname = user.LastName, email = user.Email, photoUrl = user.PhotoUrl }).ToList().FirstOrDefault();
                }

                return result;
            }
        }




    }
}
