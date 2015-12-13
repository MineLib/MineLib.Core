using System;

using Aragas.Core.Data;

using Newtonsoft.Json;

namespace MineLib.Core.IO
{
    public class VarIntJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is VarInt)
                writer.WriteValue((VarInt) value);
            else
                writer.WriteValue(new VarInt(0));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return (VarInt) reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (VarInt);
        }
    }
}