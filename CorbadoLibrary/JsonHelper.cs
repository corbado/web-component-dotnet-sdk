using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorbadoLibrary
{
    internal static class JsonHelper
    {
        public static JsonNode ParseJSONObject(string json)
        {
            JsonNode? parsed = JsonNode.Parse(json);
            if (parsed == null)
            {
                throw new JsonException("Unable to parse json string: '" + json + "'");
            }

            return parsed;
        }

        public static JsonNode GetJsonNode(JsonNode obj, string key)
        {
            var value = obj[key];
            if (value == null)
            {
                throw new JsonException("Unable to get '" + key + "' from json object: '" + obj.ToString() + "'");
            }
            return value;
        }
    }
}
