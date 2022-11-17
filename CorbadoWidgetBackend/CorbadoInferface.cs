using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace CorbadoWidgetBackend
{
    public class CorbadoInterface
    {

        //Doku
        public CorbadoSdk sdk;

        /// <summary>
        /// Creates an instance of the CorbadoInterface
        /// </summary>
        /// <param name="corbadoServer">e.g. api.corbado.com</param>
        /// <param name="projectID">Your project id, taken from the Corbado dev panel</param>
        /// <param name="apiSecret">Your api secret, taken from the Corbado dev panel</param>
        public CorbadoInterface(string corbadoServer, string projectID, string apiSecret)
        {
            sdk = new CorbadoSdk(corbadoServer, projectID, apiSecret);
        }

        /// <summary>
        /// handles the authMethodsList call which you backend receives from Corbado;
        /// </summary>
        /// <param name="username">The user</param>
        /// <param name="getAuthMethodsForUser">A method which takes a username and returns either
        /// an AuthMethodsSuccess or an AuthMethodsError object</param>
        /// <returns></returns>
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
