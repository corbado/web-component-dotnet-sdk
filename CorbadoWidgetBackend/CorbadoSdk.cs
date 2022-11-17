using Newtonsoft.Json;
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

        public CorbadoSdk(string corbadoServer, string projectID, string apiSecret)
        {
            this.corbadoServer = corbadoServer;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(projectID + ":" + apiSecret);
            string auth = "Basic " + System.Convert.ToBase64String(plainTextBytes);

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", auth);
        }

        public AuthMethodsResult AuthMethodsList(string username, Func<string, AuthMethodsResult> getAuthMethodsForUser)
        {
            //Get authentication methods for user
            return getAuthMethodsForUser(username);
        }

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

            var content = new StringContent(JsonConvert.SerializeObject(reqBody), Encoding.UTF8, "application/json");
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

        public PasswordVerifyResult PasswordVerify(AuthData authData, Func<AuthData, PasswordVerifyResult> handlePasswordAuth)
        {
            var result = handlePasswordAuth(authData);
            return result;
        }
    }
}
