using System;

using Newtonsoft.Json;

namespace MineLib.Core.IO
{
    public sealed class Base64JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var text = (string) reader.Value;

            // Cut non-used data
            text = text.Replace("data:image/png;base64,", "");

            return Convert.FromBase64String(text);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bytes = (byte[]) value;
            writer.WriteValue(Convert.ToBase64String(bytes));
        }
    }
}