using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace CorbadoWidgetBackend
{
    public class CorbadoInterface
    {

        //Doku
        public CorbadoSdk sdk;

        //Client info als parameter

        //Schreiben dass das die Daten von Corbado
        public CorbadoInterface(string projectID, string apiSecret)
        {
            sdk = new CorbadoSdk(projectID, apiSecret);
        }

        //Umbauen kein body nur username
        public JObject GetLoginInfo(string body, Func<string, UserAuthMethods> getAuthMethodsForUser)
        {
            return sdk.GetLoginInfo(body, getAuthMethodsForUser);
        }
        public async Task<UserData> ReceiveSessionToken(string token)
        {
            return await sdk.ReceiveSessionToken(token);
        }
        public async Task<JObject> VerifyPassword(string body, Func<AuthData, PasswordVerifyResult> handlePasswordAuth)
        {
            var bodyParsed = JsonHelper.ParseJSONObject(body);
            var username = JsonHelper.GetJsonNode(bodyParsed, "username").Value<string>();
            var password = JsonHelper.GetJsonNode(bodyParsed, "password").Value<string>();

            var pwVerifyResult = sdk.VerifyPassword(new AuthData(username, password), handlePasswordAuth);
            var responseBody = pwVerifyResult.GetBody();

            if (pwVerifyResult is PasswordVerifySuccess)
            {
                string token = await sdk.CreateSessionToken(new UserData(username, null));
  
        //        responseBody.Add(new JProperty("token", token));
            }

            return responseBody;
        }
    }
}
