using System;
using Newtonsoft.Json;

namespace Usa.chili.Common.Converters
{
    public class JsonDateTimeConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.Value == null) return null;
            DateTime.TryParseExact(reader.Value.ToString(), "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime);
            return dateTime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            DateTime dateTime = (DateTime) value;
            writer.WriteValue(dateTime.ToString("MM/dd/yyyy HH:mm:ss"));
        }
    }
}