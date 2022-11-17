using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CorbadoWidgetBackend
{
    public class CorbadoSdk
    {
        private readonly HttpClient client;
        private string corbadoServer;

        /// <summary>
        /// Constructor of the CorbadoSdk
        /// </summary>
        /// <param name="projectID">Your projectID, taken from the Corbado developer panel</param>
        /// <param name="apiSecret">Your apiSecret, taken from the Corbado developer panel</param>
        public CorbadoSdk(string corbadoServer, string projectID, string apiSecret)
        {
            this.corbadoServer = corbadoServer;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(projectID + ":" + apiSecret);
            string auth = "Basic " + System.Convert.ToBase64String(plainTextBytes);

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", auth);
        }

        /// <summary>
        /// Handles calls of the getLoginInfo endpoint.
        /// Parses the request data and serializes the response data which is returned by getAuthMethodsForUser
        /// </summary>
        /// <param name="body">The request body of the getLoginInfo request</param>
        /// <param name="getAuthMethodsForUser">A function which receives a username and returns the corresponding authentication methods
        /// in the form of an UserAuthMethods object</param>
        /// <returns>The json response body for the getLoginInfo response</returns>
        /// <exception cref="JsonException">Is thrown if an error occurs during parsing of the request body</exception>
        public AuthMethodsResult AuthMethodsList(string username, Func<string, AuthMethodsResult> getAuthMethodsForUser)
        {
            //Get authentication methods for user
            return getAuthMethodsForUser(username);
        }

        /// <summary>
        /// Handles calls of the sessionToken endpoint.
        /// Receives a session token from the corbado web component, verifies it using the corbado
        /// api and extracts the details of the corresponding user
        /// </summary>
        /// <param name="token">The session token which gets sent by the Corbado web component</param>
        /// <returns>username and userFullName of the session token owner</returns>
        /// <exception cref="Exception">Gets thrown if the api call to the Corbado backend is not successful</exception>
        public async Task<UserData> AuthSuccessRedirect(string token, string remoteAddress, string userAgent)
        {
            //Call the Corbado backend to verify the session token
            JObject reqBody = new JObject
        {
            { "token", token },
                {"clientInfo", new JObject{
                    {"remoteAddress", remoteAddress},
                    {"userAgent", userAgent}
                } 
            }
        };

            var content = new StringContent(JsonHelper.serialize(reqBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://" + corbadoServer + "/v1/sessions/verify", content);

            var responseBodyRaw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("the corbado api call to /sessions/verify failed. response body: '" + responseBodyRaw + "'");
            }

            //Extract user data
            JObject responseBody = JsonHelper.ParseJSONObject(responseBodyRaw);
            JObject data = JsonHelper.GetJsonObject(responseBody, "data");

            string userDataRaw = JsonHelper.GetJsonNode(data, "userData").Value<string>();
            JObject userData = JsonHelper.ParseJSONObject(userDataRaw);
            string username = JsonHelper.GetJsonNode(userData, "username").Value<string>();
            string userFullName = JsonHelper.GetJsonNode(userData, "userFullName").Value<string>();

            return new UserData(username, userFullName);
        }

        /// <summary>
        /// Handles calls of the verifyPassword endpoint
        /// Parses the request data and serializes the response data which is returned by handlePasswordAuth
        /// </summary>
        /// <param name="body">The request body of the verifyPassword request</param>
        /// <param name="handlePasswordAuth">A function which takes username and password of a user and returns
        /// either a PasswordVerifySuccess or a PasswordVerifyError object containing their individual data</param>
        /// <returns>The response body for the verifyPassword response</returns>
        public PasswordVerifyResult PasswordVerify(AuthData authData, Func<AuthData, PasswordVerifyResult> handlePasswordAuth)
        {
            var result = handlePasswordAuth(authData);
            return result;
        }
    }
}
