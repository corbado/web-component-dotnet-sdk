using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CorbadoLibrary
{
    public class UserData
    {
        public string username;
        public string? userFullName;
        public UserData(string username, string? userFullName)
        {
            this.username = username;
            this.userFullName = userFullName;
        }

       public static UserData fromBody(string body)
        {
            var bodyParsed = JsonHelper.ParseJSONObject(body);
            var username = JsonHelper.GetJsonNode(bodyParsed, "username").GetValue<string>();
            var password = JsonHelper.GetJsonNode(bodyParsed, "password").GetValue<string>();

            return new UserData(username, password);
        }


        override
            public string ToString()
        {
            var result = new JsonObject()
            {
                {"username", username }
            };

            if(userFullName != null)
            {
                result.Add("userFullName", userFullName);
            }
            return result.ToString();
        }
    }
}
