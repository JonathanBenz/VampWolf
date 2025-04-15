using UnityEngine;
using UnityEngine.Pool;

namespace Vampwolf.Battles
{
    public class InitiativeCardPool
    {
        private readonly InitiativeCard cardPrefab;
        private readonly Transform parent;
        private readonly ObjectPool<InitiativeCard> pool;

        public InitiativeCardPool(InitiativeCard cardPrefab, Transform parent)
        {
            this.cardPrefab = cardPrefab;
            this.parent = parent;

            pool = new ObjectPool<InitiativeCard>(
                CreateCard,
                OnGetCard,
                OnReleaseCard,
                OnDestroyCard,
                false,
                10,
                20
            );
        }

        /// <summary>
        /// Get an initiative card from the pool
        /// </summary>
        public InitiativeCard Get() => pool.Get();

        /// <summary>
        /// Release an initiative card back to the pool
        /// </summary>
        public void Release(InitiativeCard card) => pool.Release(card);

        /// <summary>
        /// Callback function for creating an initiative card within the pool
        /// </summary>
        private InitiativeCard CreateCard() => Object.Instantiate(cardPrefab, parent);

        /// <summary>
        /// Callback function for when an initiative card is received from the pool
        /// </summary>
        private void OnGetCard(InitiativeCard card) => card.gameObject.SetActive(true);

        /// <summary>
        /// Callback function for when an initiative card is released back to the pool
        /// </summary>
        private void OnReleaseCard(InitiativeCard card) => card.gameObject.SetActive(false);

        /// <summary>
        /// Callback function for when an initiative card is destroyed within the pool
        /// </summary>
        private void OnDestroyCard(InitiativeCard card) => Object.Destroy(card.gameObject);
    }
}
