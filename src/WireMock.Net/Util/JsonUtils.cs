using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WireMock.Serialization;

namespace WireMock.Util
{
    internal static class JsonUtils
    {
        public static bool TryParseAsComplexObject(string strInput, out JToken token)
        {
            token = null;

            if (string.IsNullOrWhiteSpace(strInput))
            {
                return false;
            }

            strInput = strInput.Trim();
            if ((!strInput.StartsWith("{") || !strInput.EndsWith("}")) && (!strInput.StartsWith("[") || !strInput.EndsWith("]")))
            {
                return false;
            }

            try
            {
                // Try to convert this string into a JToken
                token = JToken.Parse(strInput);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, JsonSerializationConstants.JsonSerializerSettingsIncludeNullValues);
        }

        /// <summary>
        /// Load a Newtonsoft.Json.Linq.JObject from a string that contains JSON.
        /// Using : DateParseHandling = DateParseHandling.None
        /// </summary>
        /// <param name="json">A System.String that contains JSON.</param>
        /// <returns>A Newtonsoft.Json.Linq.JToken populated from the string that contains JSON.</returns>
        public static JToken Parse(string json)
        {
            return JsonConvert.DeserializeObject<JToken>(json, JsonSerializationConstants.JsonDeserializerSettingsWithDateParsingNone);
        }

        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// Using : DateParseHandling = DateParseHandling.None
        /// </summary>
        /// <param name="json">A System.String that contains JSON.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject(json, JsonSerializationConstants.JsonDeserializerSettingsWithDateParsingNone);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// Using : DateParseHandling = DateParseHandling.None
        /// </summary>
        /// <param name="json">A System.String that contains JSON.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSerializationConstants.JsonDeserializerSettingsWithDateParsingNone);
        }

        public static T ParseJTokenToObject<T>(object value)
        {
            switch (value)
            {
                case JToken tokenValue:
                    return tokenValue.ToObject<T>();

                default:
                    return default(T);
            }
        }

        public static string GenerateDynamicLinqStatement(JToken jsonObject)
        {
            var lines = new List<string>();
            WalkNode(jsonObject, null, null, lines);

            return lines.First();
        }

        private static void WalkNode(JToken node, string path, string propertyName, List<string> lines)
        {
            if (node.Type == JTokenType.Object)
            {
                ProcessObject(node, propertyName, lines);
            }
            else if (node.Type == JTokenType.Array)
            {
                ProcessArray(node, propertyName, lines);
            }
            else
            {
                ProcessItem(node, path ?? "it", propertyName, lines);
            }
        }

        private static void ProcessObject(JToken node, string propertyName, List<string> lines)
        {
            var items = new List<string>();
            var text = new StringBuilder("new (");

            // In case of Object, loop all children. Do a ToArray() to avoid `Collection was modified` exceptions.
            foreach (JProperty child in node.Children<JProperty>().ToArray())
            {
                WalkNode(child.Value, child.Path, child.Name, items);
            }

            text.Append(string.Join(", ", items));
            text.Append(")");

            if (!string.IsNullOrEmpty(propertyName))
            {
                text.AppendFormat(" as {0}", propertyName);
            }

            lines.Add(text.ToString());
        }

        private static void ProcessArray(JToken node, string propertyName, List<string> lines)
        {
            var items = new List<string>();
            var text = new StringBuilder("(new [] { ");

            // In case of Array, loop all items. Do a ToArray() to avoid `Collection was modified` exceptions.
            int idx = 0;
            foreach (JToken child in node.Children().ToArray())
            {
                WalkNode(child, $"{node.Path}[{idx}]", null, items);
                idx++;
            }

            text.Append(string.Join(", ", items));
            text.Append("})");

            if (!string.IsNullOrEmpty(propertyName))
            {
                text.AppendFormat(" as {0}", propertyName);
            }

            lines.Add(text.ToString());
        }

        private static void ProcessItem(JToken node, string path, string propertyName, List<string> lines)
        {
            string castText;
            switch (node.Type)
            {
                case JTokenType.Boolean:
                    castText = $"bool({path})";
                    break;

                case JTokenType.Date:
                    castText = $"DateTime({path})";
                    break;

                case JTokenType.Float:
                    castText = $"double({path})";
                    break;

                case JTokenType.Guid:
                    castText = $"Guid({path})";
                    break;

                case JTokenType.Integer:
                    castText = $"long({path})";
                    break;

                case JTokenType.Null:
                    castText = "null";
                    break;

                case JTokenType.String:
                    castText = $"string({path})";
                    break;

                case JTokenType.TimeSpan:
                    castText = $"TimeSpan({path})";
                    break;

                case JTokenType.Uri:
                    castText = $"Uri({path})";
                    break;

                default:
                    throw new NotSupportedException($"JTokenType '{node.Type}' cannot be converted to a Dynamic Linq cast operator.");
            }

            if (!string.IsNullOrEmpty(propertyName))
            {
                castText += $" as {propertyName}";
            }

            lines.Add(castText);
        }
    }
}