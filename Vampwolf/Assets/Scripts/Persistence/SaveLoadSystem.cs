using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vampwolf.Utilities.Singletons;

namespace Vampwolf.Persistence
{
    public interface ISaveable
    {
        SerializableGuid ID { get; set; }
    }

    public interface IBind<TData> where TData : ISaveable
    {
        SerializableGuid ID { get; set; }
        void Bind(TData data);
    }

    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
    {
        [SerializeField] private GameData selectedData;
        [SerializeField] private Dictionary<string, GameData> saves;
        private FileDataService gameDataService;

        public GameData GameData { get => selectedData; }
        public Dictionary<string, GameData> Saves { get { return saves; } }

        protected override void Awake()
        {
            // Call the parent Awake()
            base.Awake();

            // Create a File Data Service using a JSON Serializer
            gameDataService = new FileDataService(new JsonSerializer());
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            // Bind data
            Bind<VampireSaveHandler, VampireData>(selectedData.Vampire);
            Bind<WerewolfSaveHandler, WerewolfData>(selectedData.Werewolf);
        }

        /// <summary>
        /// Bind a specific type of entity
        /// </summary>
        private void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            // Get the first or default entity of the given type
            T entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();

            // Exit case - no entity was found
            if (entity == null) return;

            // Check if the data is null
            if (data == null) 
                // Create a new instance of the data and set the ID
                data = new TData { ID = entity.ID };

            // Bind the data to the entity
            entity.Bind(data);
        }

        /// <summary>
        /// Bind a list of entities of a given type
        /// </summary>
        private void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            // Get the list of entities of the given type
            T[] entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            // Iterate through each entity
            foreach (T entity in entities)
            {
                // Get the data that matches the entity by comapring their IDs
                TData data = datas.FirstOrDefault(d => d.ID == entity.ID);

                // Check if the data is null
                if (data == null)
                {
                    // If so, create a new instance of the data and set the ID to the
                    // entity's ID
                    data = new TData { ID = entity.ID };

                    // Add the data to the list of datas to be tracked
                    datas.Add(data);
                }

                // Bind the data to the entity
                entity.Bind(data);
            }
        }

        /// <summary>
        /// Create a New Game
        /// </summary>
        public void NewGame()
        {
            // Start building the name
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append("Vampwolf--");

            // Append the date
            DateTime date = DateTime.Now;
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            nameBuilder.Append(month);
            nameBuilder.Append("-");
            nameBuilder.Append(day);
            nameBuilder.Append("-");
            nameBuilder.Append(year);

            // Append the number
            nameBuilder.Append("--");
            nameBuilder.Append(GetSaveCount() + 1);

            // Create a new Game Data
            selectedData = new GameData
            {
                Name = nameBuilder.ToString(),
                Vampire = new VampireData(),
                Werewolf = new WerewolfData()
            };

            SaveGame();
        }

        /// <summary>
        /// Save the current state of the Game Data
        /// </summary>
        public void SaveGame() => gameDataService.Save(selectedData);

        /// <summary>
        /// Delete a persisted Game Data file
        /// </summary>
        public void DeleteGame(string gameName) => gameDataService.Delete(gameName);

        /// <summary>
        /// List all of the saved files by name
        /// </summary>
        public IEnumerable<string> ListSaves() => gameDataService.ListSaves();

        /// <summary>
        /// Get the number of current saves
        /// </summary>
        public int GetSaveCount() => ListSaves().Count();
    }
}
