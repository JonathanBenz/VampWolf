using UnityEngine;

namespace Vampwolf.Persistence
{
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// Serialize the object to a JSON string
        /// </summary>
        public string Serialize<T>(T obj) where T : GameData
        {
            return JsonUtility.ToJson(obj, true);
        }

        /// <summary>
        /// Deserialize a JSON string to a data object
        /// </summary>
        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}
