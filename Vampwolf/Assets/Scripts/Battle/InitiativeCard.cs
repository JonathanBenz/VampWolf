using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vampwolf.Units;

namespace Vampwolf.Battles
{
    public class InitiativeCard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image healthFill;
        [SerializeField] private Text nameText;
        private RectTransform rectTransform;

        [Header("Fields")]
        [SerializeField] private BattleUnit attachedUnit;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        private float initialScale;

        [Header("Tweening Variables")]
        [SerializeField] private float selectScale = 1.2f;
        [SerializeField] private float selectDuration = 0.5f;
        [SerializeField] private float fillDuration = 0.5f;
        private Tween selectTween;
        private Tween fillTween;

        public BattleUnit Unit => attachedUnit;

        private void OnDestroy()
        {
            // Kill any existing tweens
            selectTween?.Kill();
            fillTween?.Kill();
        }

        /// <summary>
        /// Initialize the card
        /// </summary>
        public void Initialize(BattleUnit unit)
        {
            // Get components
            rectTransform = GetComponent<RectTransform>();

            // Set the attached unit and name
            attachedUnit = unit;
            nameText.text = unit.Name;

            // Set the health fill
            maxHealth = unit.Health;
            currentHealth = unit.Health;
            healthFill.fillAmount = currentHealth / maxHealth;

            // Set the initial scale
            initialScale = rectTransform.localScale.x;

            // Set the game object's name
            gameObject.name = $"{unit.Name} Card";
        }

        /// <summary>
        /// Update the health of the unit displayed by the card
        /// </summary>
        public void UpdateHealth(int currentHealth)
        {
            // Update the current health
            this.currentHealth = currentHealth;

            // Calculate the new fill amount
            float fillAmount = currentHealth / maxHealth;

            // Tween the health bar
            Fill(fillAmount, fillDuration);
        }

        /// <summary>
        /// Select the card
        /// </summary>
        public void Select() => Select(selectScale, selectDuration);

        /// <summary>
        /// Deselect the card
        /// </summary>
        public void Deselect() => Select(1f, selectDuration);

        /// <summary>
        /// Handle tweening for the card selection
        /// </summary>
        private void Select(float endValue, float duration)
        {
            // Kill the select tween if it exists
            selectTween?.Kill();

            // Create a new select tween
            selectTween = rectTransform.DOScale(endValue, duration);
        }

        /// <summary>
        /// Handle tweening for the health fill
        /// </summary>
        private void Fill(float endValue, float duration)
        {
            // Kill the fill tween if it exists
            fillTween?.Kill();

            // Create a new fill tween
            fillTween = healthFill.DOFillAmount(endValue, duration);
        }
    }
}
