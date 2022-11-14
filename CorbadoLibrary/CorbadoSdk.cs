using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace CorbadoLibrary
{
    public class CorbadoSdk
    {
        private readonly HttpClient client;

        /// <summary>
        /// Constructor of the CorbadoSdk
        /// </summary>
        /// <param name="projectID">Your projectID, taken from the Corbado developer panel</param>
        /// <param name="apiSecret">Your apiSecret, taken from the Corbado developer panel</param>
        public CorbadoSdk(string projectID, string apiSecret)
        {
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
        public JsonObject GetLoginInfo(string body, Func<string, UserAuthMethods> getAuthMethodsForUser)
        {
            //get username from body
            var bodyParsed = JsonHelper.ParseJSONObject(body);
            var username = JsonHelper.GetJsonNode(bodyParsed, "username");

            //Get authentication methods for user
            UserAuthMethods authMethods = getAuthMethodsForUser(username.GetValue<String>());

            JsonObject data = new JsonObject();
            JsonArray methods = new JsonArray();

            //Convert authentication methods to valid json
            if (authMethods.emails.Any())
            {
                methods.Add("email");
                JsonArray emails = new JsonArray();
                authMethods.emails.ForEach(email => { emails.Add(email); });
                data.Add("emails", emails);
            }
            if (authMethods.phoneNumbers.Any())
            {
                methods.Add("phone_number");
                JsonArray phoneNumbers = new JsonArray();
                authMethods.emails.ForEach(phoneNumber => { phoneNumbers.Add(phoneNumber); });
                data.Add("phone_numbers", phoneNumbers);
            }
            if (authMethods.hasPassword)
            {
                methods.Add("password");
            }

            data.Add("methods", methods);
            JsonObject responseBody = new JsonObject()
        {
            {"data", data }
        };

            return responseBody;
        }

        /// <summary>
        /// Handles calls of the sessionToken endpoint.
        /// Receives a session token from the corbado web component, verifies it using the corbado
        /// api and extracts the details of the corresponding user
        /// </summary>
        /// <param name="token">The session token which gets sent by the Corbado web component</param>
        /// <returns>username and userFullName of the session token owner</returns>
        /// <exception cref="Exception">Gets thrown if the api call to the Corbado backend is not successful</exception>
        public async Task<UserData> ReceiveSessionToken(string token)
        {
            //Call the Corbado backend to verify the session token
            JsonObject reqBody = new JsonObject
        {
            { "token", token },

            //ClientInfo? 
        };

            var content = new StringContent(reqBody.ToJsonString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.corbado.com/v1/sessions/verify", content);

            var responseBodyRaw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("the corbado api call to /sessions/verify failed. response body: '" + responseBodyRaw + "'");
            }

            //Extract user data
            var responseBody = JsonHelper.ParseJSONObject(responseBodyRaw);
            var userDataRaw = JsonHelper.GetJsonNode(responseBody, "userData").GetValue<string>();
            var userData = JsonHelper.ParseJSONObject(userDataRaw);
            var username = JsonHelper.GetJsonNode(userData, "username").GetValue<string>();
            var userFullName = JsonHelper.GetJsonNode(userData, "userFullName").GetValue<string>();

            return new UserData(username, userFullName);
        }

        /// <summary>
        /// Creates a session token using the Corbado API.
        /// Receives a session token from the corbado web component, verifies it using the corbado
        /// api and extracts the details of the corresponding user
        /// </summary>
        /// <param name="userData">The user</param>
        /// <returns>the Corbado api token for the given user</returns>
        /// <exception cref="Exception">Gets thrown if the api call to the Corbado backend is not successful</exception>
        public async Task<string> CreateSessionToken(UserData userData)
        {
            //Call the Corbado backend to verify the session token
            JsonObject reqBody = new JsonObject
            {
            { "userData", userData.ToString() },
            
            //ClientInfo? 
        };

            var content = new StringContent(reqBody.ToJsonString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.corbado.com/v1/sessions", content);

            var responseBodyRaw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("the corbado api call to /sessions failed. response body: '" + responseBodyRaw + "'");
            }

            //Extract user data
            var responseBody = JsonHelper.ParseJSONObject(responseBodyRaw);
            var dataRaw = JsonHelper.GetJsonNode(responseBody, "userData").GetValue<string>();
            var data = JsonHelper.ParseJSONObject(dataRaw);
            var token = JsonHelper.GetJsonNode(data, "username").GetValue<string>();

            return token;
        }

        /// <summary>
        /// Handles calls of the verifyPassword endpoint
        /// Parses the request data and serializes the response data which is returned by handlePasswordAuth
        /// </summary>
        /// <param name="body">The request body of the verifyPassword request</param>
        /// <param name="handlePasswordAuth">A function which takes username and password of a user and returns
        /// either a PasswordVerifySuccess or a PasswordVerifyError object containing their individual data</param>
        /// <returns>The response body for the verifyPassword response</returns>
        public PasswordVerifyResult VerifyPassword(AuthData authData, Func<AuthData, PasswordVerifyResult> handlePasswordAuth)
        {
            var result = handlePasswordAuth(authData);
            return result;
        }
    }
}