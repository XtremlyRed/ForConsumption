using System;

using Newtonsoft.Json;

namespace ForConsumption.Common.Common
{
    public static class JsonMapper
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {

            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTime,
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
        public static object Deserialize(string jsonString, Type targetType)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new ArgumentNullException(nameof(jsonString));
            }
            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            return JsonConvert.DeserializeObject(jsonString, targetType, JsonSerializerSettings);
        }

        public static Target Deserialize<Target>(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new ArgumentNullException(nameof(jsonString));
            }

            return JsonConvert.DeserializeObject<Target>(jsonString, JsonSerializerSettings);
        }

        public static string Serialize<Target>(Target targetObject)
        {
            string jsonString = JsonConvert.SerializeObject(targetObject, Formatting.None, JsonSerializerSettings);
            return jsonString;
        }
    }
}
