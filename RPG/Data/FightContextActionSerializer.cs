using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RPG.Data
{
    internal class FightContextActionSerializer<T> : JsonConverter<FightContextAction<T>> where T : class, new()
    {
        public override bool CanWrite => true;
        public override bool CanRead => true;

        public override FightContextAction<T>? ReadJson(JsonReader reader, Type objectType, FightContextAction<T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            var customObject = token.ToObject<FightContextAction<T>>(serializer);

            customObject?.Init();

            return customObject;
        }

        public override void WriteJson(JsonWriter writer, FightContextAction<T>? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
