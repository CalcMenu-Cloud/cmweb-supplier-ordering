using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;



namespace OrderingAPI.Security
{
    public class LoginSecurity : Controller
    {
        public string ClientId = "";
        public string CodeUser = "";
        public string SessionId = "";
        public string Tokencode = "";
        private readonly string _connectionString;
        public string baseURL = "";

        public LoginSecurity(string connectionString)
        {
            _connectionString = connectionString;
        }


        public Models.LoginInfoHG getRequestInfo(string state)
        {
            try
            {
                // Decode the Base64 string to a byte array
                byte[] bytes = Convert.FromBase64String(state);

                // Convert the byte array to a string using UTF-8 encoding
                string jsonString = Encoding.UTF8.GetString(bytes);

                // Deserialize the JSON string into an instance of MyData class
                //MyData data = JsonSerializer.Deserialize<MyData>(jsonString);
                // Deserialize the JSON string into an instance of MyData class

                Models.LoginInfoHG loginfo = JsonSerializer.Deserialize<Models.LoginInfoHG>(jsonString);

                loginfo.callbackurl = loginfo.callbackurl ?? "";

                return loginfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


            public bool HogashopLogin(string code,string state)
          {
            try
            {
                // Decode the Base64 string to a byte array
                byte[] bytes = Convert.FromBase64String(state);

                // Convert the byte array to a string using UTF-8 encoding
                string jsonString = Encoding.UTF8.GetString(bytes);

                // Deserialize the JSON string into an instance of MyData class
                //MyData data = JsonSerializer.Deserialize<MyData>(jsonString);
                // Deserialize the JSON string into an instance of MyData class

                Models.LoginInfoHG loginfo = JsonSerializer.Deserialize<Models.LoginInfoHG>(jsonString);


                baseURL = loginfo.baseurl??"";
                Tokencode = code;

                Services.ThirdPartyAPI.HogashopOauth hogoauth = new Services.ThirdPartyAPI.HogashopOauth();

                string refreshtoken = hogoauth.GetAccessToken(Tokencode);
           
                Data.LoginSessionDataService logsession = new Data.LoginSessionDataService(_connectionString);

                if (logsession.setSNUserTokenWithId(loginfo.codeuser, loginfo.sessionKey, refreshtoken))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;

            }

        }


        public bool ValidateLogin(string code,string state,string flag)
        {
            try
            {
                // Decode the Base64 string to a byte array
                byte[] bytes = Convert.FromBase64String(state);

                // Convert the byte array to a string using UTF-8 encoding
                string jsonString = Encoding.UTF8.GetString(bytes);

                // Deserialize the JSON string into an instance of MyData class
                //MyData data = JsonSerializer.Deserialize<MyData>(jsonString);
                // Deserialize the JSON string into an instance of MyData class

                Models.LoginInfo loginfo = JsonSerializer.Deserialize<Models.LoginInfo>(jsonString);
                string decodedString = System.Net.WebUtility.UrlDecode(loginfo.returnUrl);

                // Parse the query parameters into a NameValueCollection
                NameValueCollection parameters;
               int indexcount= decodedString.Split("?").Count();

                if (indexcount < 2)
                {
                    parameters = System.Web.HttpUtility.ParseQueryString(decodedString.Split('?')[0]);
                }
                else
                {
                    parameters = System.Web.HttpUtility.ParseQueryString(decodedString.Split('?')[1]);
                }

                // Access individual parameters
                string id = parameters["id"];
                ClientId = "1";//parameters["clientid"];
                CodeUser = "1";
                Tokencode = code;
                SessionId = loginfo.sessionKey;


                Services.ThirdPartyAPI.HogashopOauth hogoauth = new Services.ThirdPartyAPI.HogashopOauth();

                string refreshtoken = hogoauth.GetAccessToken(Tokencode);

                Data.LoginSessionDataService logsession = new Data.LoginSessionDataService(_connectionString);

                 if (logsession.setSNUserSession(ClientId, CodeUser, SessionId, refreshtoken))
                 {
                    return true;
                 }
                else
                {
                    return false;
                }           
            }
            catch(Exception ex)
            {
                return false;

            }

        }


        public bool ValidateLogin(string logininfobs64,ref Models.UserSession usersession)
        {
            try
            {
                string base64Credentials = logininfobs64;
                byte[] base64Bytes = Convert.FromBase64String(base64Credentials);
                string jsonCredentials = Encoding.UTF8.GetString(base64Bytes);
                Models.UserLoginInfo credentials = JsonSerializer.Deserialize<Models.UserLoginInfo>(jsonCredentials);
                Data.LoginSessionDataService logindata = new Data.LoginSessionDataService(_connectionString);
                int CodeUser = 0;

                if(!logindata.ValidateCredential(credentials.username, credentials.password,ref @CodeUser))
                {
                    return false;
                }

                logindata.setSNUserSession(ClientId: "", CodeUser: CodeUser.ToString(), SessionKey: credentials.sessionid, RefreshToken:"");
                usersession = logindata.getSNUserSession(credentials.sessionid);

                if(usersession==null)
                {
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }




    }
}
