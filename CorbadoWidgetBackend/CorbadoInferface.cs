using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace CorbadoWidgetBackend
{
    public class CorbadoInterface
    {

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
        /// handles the authMethodsList call which you get from Corbado;
        /// Gives information about all possible authentication methods for a certain user
        /// </summary>
        /// <param name="username">The user</param>
        /// <param name="getAuthMethodsForUser">A function which takes a username and returns either
        /// [an AuthMethodsSuccess object containing emails and phone numbers] or [an AuthMethodsError object]</param>
        /// <returns>Either an AuthMethodsSuccess or AuthMethodsError object</returns>
        public AuthMethodsResult AuthMethodsList(string username, Func<string, AuthMethodsResult> getAuthMethodsForUser)
        {
            return sdk.AuthMethodsList(username, getAuthMethodsForUser);
        }

        /// <summary>
        /// handles the authSuccessRedirect call which you get from Corbado;
        /// Extracts user data from a given session token
        /// </summary>
        /// <param name="token">The given session token</param>
        /// <param name="remoteAddress">Remote address of the request</param>
        /// <param name="userAgent">Useragent of the request</param>
        /// <returns>An object containing username and userFullName of the user which the token
        /// belongs to</returns>
        public async Task<UserData> AuthSuccessRedirect(string token, string remoteAddress, string userAgent)
        {
            return await sdk.AuthSuccessRedirect(token, remoteAddress, userAgent);
        }

        /// <summary>
        /// handles the passwordVerify call which you get from Corbado;
        /// Checks if a password is valid for a given username
        /// </summary>
        /// <param name="body">The request body</param>
        /// <param name="handlePasswordAuth">A function which receives authentication data and
        /// returns either [a PasswordVerifySuccess containing a redirectUrl and a sessionToken]
        /// or [a PasswordVerifyError object]</param>
        /// <returns></returns>
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
