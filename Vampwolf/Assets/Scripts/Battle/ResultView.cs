using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vampwolf.EventBus;

namespace Vampwolf
{
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup winGroup;

        private EventBinding<BattleWon> onBattleWon;

        private void Awake()
        {
            // Hide the win screen
            winGroup.alpha = 0f;
            winGroup.interactable = false;
            winGroup.blocksRaycasts = false;

            // Get the win button
            Button winButton = winGroup.GetComponentInChildren<Button>();
            winButton.onClick.AddListener(() => SceneManager.LoadScene(1)); // Load into hub
        }

        private void OnEnable()
        {
            onBattleWon = new EventBinding<BattleWon>(ShowWinScreen);
            EventBus<BattleWon>.Register(onBattleWon);
        }

        private void OnDisable()
        {
            EventBus<BattleWon>.Deregister(onBattleWon);
        }

        /// <summary>
        /// Show the win screen
        /// </summary>
        private void ShowWinScreen()
        {
            winGroup.alpha = 1f;
            winGroup.interactable = true;
            winGroup.blocksRaycasts = true;

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2) // Forest Level
                ProgressTracker.Instance.level1Complete = true;
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 3) // Castle Level
                ProgressTracker.Instance.level2Complete = true;
        }
    }
}
