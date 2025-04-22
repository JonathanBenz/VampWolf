using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf
{
    /// <summary>
    /// This script handles the night time to day time mechanic
    /// </summary>
    public class MoonToSunTimeBar : MonoBehaviour
    {
        [SerializeField] int totalTurns = 20;
        private int currentTurn = -1;
        private float amountToSlide;
        private RectTransform graphic;
        private BattleUnit vampire;
        private BattleUnit werewolf;

        private EventBinding<TurnStarted> onTurnStarted;

        private void Awake()
        {
            graphic = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            vampire = FindObjectOfType<Vampire>();
            werewolf = FindObjectOfType<Werewolf>();
        }

        private void OnEnable()
        {
            onTurnStarted = new EventBinding<TurnStarted>(UpdateGraphic);
            EventBus<TurnStarted>.Register(onTurnStarted);
        }

        private void OnDisable()
        {
            EventBus<TurnStarted>.Deregister(onTurnStarted);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get the total amount of the image which is outside of the mask --> This is what we need to slide into the mask
            float imageWidth = graphic.rect.width;
            float maskWidth = ((RectTransform)graphic.parent).rect.width;

            // Normalize the amount we want to slide by the amount of turns we want to take
            amountToSlide = (imageWidth - 2*maskWidth) / totalTurns;

            graphic.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Every time a new turn starts, slide the graphic
        /// </summary>
        public void UpdateGraphic()
        {
            if (currentTurn < totalTurns)
            {
                currentTurn++;

                Vector2 newPos = graphic.anchoredPosition;
                newPos.x = -amountToSlide * currentTurn; // Increment by sliding to the left
                graphic.anchoredPosition = newPos;
            }
            else 
            {
                Debug.Log("AHHH! THE SUN IT BURNS!!!");
                vampire.DealDamage(999);
                werewolf.DealDamage(999);
            }
        }
    }
}
