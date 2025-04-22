namespace Vampwolf.Persistence
{
    public interface ISerializer
    {
        /// <summary>
        /// Serialize an object to a string
        /// </summary>
        string Serialize<T>(T obj) where T : GameData;


        /// <summary>
        /// Deserialize a string into an object
        /// </summary>
        T Deserialize<T>(string json);
    }
}
