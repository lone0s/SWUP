using System;

using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assets.DataClasses
{
    public class UsableObjectConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(UsableObject);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var result = (Planet) existingValue ?? new Planet();

            serializer.Populate(obj.CreateReader(), result);

            return result;
        }



        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            var jObject = new JObject
            {
                { "x", value.x },
                { "y", value.y },
                { "z", value.z }
            };
            jObject.WriteTo(writer);
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            return existingValue;
        }
    }
}