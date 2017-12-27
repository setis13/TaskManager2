using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaskManager.Common.Extensions {
    public static class JsonExtension {
        public static string SerializeObject<T>(T obj) {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            settings.NullValueHandling = NullValueHandling.Ignore;
            var result = JsonConvert.SerializeObject(obj, settings);
            return result;
        }

        public static T DeserializeObject<T>(string str) {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<T>(str, settings);
        }

        public static bool IsValidJson(string strInput) {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try {
                    var obj = JToken.Parse(strInput);
                    return true;
                } catch (JsonReaderException jex) {
                    return false;
                } catch (Exception ex) {
                    return false;
                }
            } else {
                return false;
            }
        }
    }
}
