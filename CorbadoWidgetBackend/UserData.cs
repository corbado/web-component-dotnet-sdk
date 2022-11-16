using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CorbadoWidgetBackend
{
    public class UserData
    {
        public string username;
        public string userFullName;
        public UserData(string username, string userFullName)
        {
            this.username = username;
            this.userFullName = userFullName;
        }

        public static UserData fromBody(string body)
        {
            var bodyParsed = JsonHelper.ParseJSONObject(body);
            var username = JsonHelper.GetJsonNode(bodyParsed, "username").Value<string>();
            var password = JsonHelper.GetJsonNode(bodyParsed, "password").Value<string>();

            return new UserData(username, password);
        }


        override
            public string ToString()
        {
            var result = new JObject()
            {
                {"username", username }
            };

            if (userFullName != null)
            {
                result.Add(new JProperty("userFullName", userFullName));
            }
            return JsonHelper.serialize(result);
        }
    }
}
