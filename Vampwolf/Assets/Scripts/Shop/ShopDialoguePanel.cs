using UnityEngine;
using UnityEngine.UI;
using Vampwolf.Utilities.Typewriter;

namespace Vampwolf.Shop
{
    public class ShopDialoguePanel : MonoBehaviour
    {
        [SerializeField] private Text text;
        private Typewriter typewriter;

        [Header("Fields")]
        [SerializeField] private float characterSpeed;
        [SerializeField] private string[] welcomeDialogues;

        /// <summary>
        /// Initialize the shop dialogue panel
        /// </summary>
        public void Initialize()
        {
            // Create the typewriter
            typewriter = new Typewriter(text, characterSpeed);
        }

        /// <summary>
        /// Create an instance of dialogue to display
        /// </summary>
        public void CreateWelcomeDialogue()
        {
            // Get a random piece of dialogue
            int randomIndex = Random.Range(0, welcomeDialogues.Length - 1);
            string randomDialogue = welcomeDialogues[randomIndex];

            // Write the dialogue using the typewriter
            typewriter.Write(randomDialogue);
        }

        /// <summary>
        /// Clear the dialogue box
        /// </summary>
        public void ClearDialogue() => typewriter.Clear();
    }
}
