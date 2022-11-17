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

        public AuthMethodsResult AuthMethodsList(string username, Func<string, AuthMethodsResult> getAuthMethodsForUser)
        {
            return sdk.AuthMethodsList(username, getAuthMethodsForUser);
        }
        public async Task<UserData> AuthSuccessRedirect(string token, string remoteAddress, string userAgent)
        {
            return await sdk.AuthSuccessRedirect(token, remoteAddress, userAgent);
        }
        public PasswordVerifyResult PasswordVerify(string body, Func<AuthData, PasswordVerifyResult> handlePasswordAuth)
        {
            var bodyParsed = JsonHelper.ParseJSONObject(body);
            var username = JsonHelper.GetJsonNode(bodyParsed, "username").Value<string>();
            var password = JsonHelper.GetJsonNode(bodyParsed, "password").Value<string>();

            var pwVerifyResult = sdk.PasswordVerify(new AuthData(username, password), handlePasswordAuth);

            return pwVerifyResult;
        }
    }
}
