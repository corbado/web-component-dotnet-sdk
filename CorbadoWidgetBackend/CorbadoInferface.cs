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
        public CorbadoInterface(string corbadoServer, string projectID, string apiSecret)
        {
            sdk = new CorbadoSdk(corbadoServer, projectID, apiSecret);
        }

        public JObject authMethods(string username, Func<string, UserAuthMethods> getAuthMethodsForUser)
        {
            return sdk.authMethods(username, getAuthMethodsForUser);
        }
        public async Task<UserData> ReceiveSessionToken(string token, string remoteAddress, string userAgent)
        {
            return await sdk.ReceiveSessionToken(token, remoteAddress, userAgent);
        }
        public int VerifyPassword(string body, Func<AuthData, int> handlePasswordAuth)
        {
            var bodyParsed = JsonHelper.ParseJSONObject(body);
            var username = JsonHelper.GetJsonNode(bodyParsed, "username").Value<string>();
            var password = JsonHelper.GetJsonNode(bodyParsed, "password").Value<string>();

            var pwVerifyResult = sdk.VerifyPassword(new AuthData(username, password), handlePasswordAuth);

            return pwVerifyResult;
        }
    }
}
