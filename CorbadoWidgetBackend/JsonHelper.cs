using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorbadoWidgetBackend
{
    internal static class JsonHelper
    {
        public static JObject ParseJSONObject(string json)
        {
            JObject parsed = JObject.Parse(json);
            if (parsed == null)
            {
                throw new Exception("Unable to parse json string: '" + json + "'");
            }

            return parsed;
        }

        public static JObject GetJsonObject(JObject obj, string key)
        {
            var value = obj[key];
            if (value == null)
            {
                throw new Exception("Unable to get object '" + key + "' from json object: '" + obj.ToString() + "'");
            }
            return (JObject)value;
        }

        public static JToken GetJsonNode(JObject obj, string key)
        {
            var value = obj[key];
            if (value == null)
            {
                throw new Exception("Unable to get token '" + key + "' from json object: '" + obj.ToString() + "'");
            }
            return value;
        }
    }
}
