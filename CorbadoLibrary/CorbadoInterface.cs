using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CorbadoLibrary
{
    public class CorbadoInterface
    {
        public CorbadoSdk sdk;
        public CorbadoInterface(string projectID, string apiSecret)
        {
            sdk = new CorbadoSdk(projectID, apiSecret);
        }
        public JsonObject GetLoginInfo(string body, Func<string, UserAuthMethods> getAuthMethodsForUser)
        {
            return sdk.GetLoginInfo(body, getAuthMethodsForUser);
        }
        public async Task<UserData> ReceiveSessionToken(string token)
        {
            return await sdk.ReceiveSessionToken(token);
        }
        public async Task<JsonObject> VerifyPassword(string body, Func<AuthData, PasswordVerifyResult> handlePasswordAuth)
        {
            var bodyParsed = JsonHelper.ParseJSONObject(body);
            var username = JsonHelper.GetJsonNode(bodyParsed, "username").GetValue<string>();
            var password = JsonHelper.GetJsonNode(bodyParsed, "password").GetValue<string>();
            
            var pwVerifyResult = sdk.VerifyPassword(new AuthData(username, password), handlePasswordAuth);
            var responseBody = pwVerifyResult.GetBody();

            if (pwVerifyResult is PasswordVerifySuccess)
            {
                var token = await sdk.CreateSessionToken(new UserData(username, null));
                responseBody.Add("token", token);
            }

            return responseBody;
        }
    }
}
