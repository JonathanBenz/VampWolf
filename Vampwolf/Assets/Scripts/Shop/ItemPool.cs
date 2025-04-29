using UnityEngine;
using UnityEngine.Pool;

namespace Vampwolf.Shop
{
    public class ItemPool
    {
        private readonly ItemButton prefab;
        private readonly Transform parent;
        private readonly ObjectPool<ItemButton> pool;

        public ItemPool(ItemButton prefab, Transform parent)
        {
            // Set variables
            this.prefab = prefab;
            this.parent = parent;

            // Create the pool
            pool = new ObjectPool<ItemButton>(
                CreateItem,
                OnGetItem,
                OnReleaseItem,
                OnDestroyItem,
                false,
                10,
                20
            );
        }

        /// <summary>
        /// Get an item button from the pool
        /// </summary>
        public ItemButton Get() => pool.Get();

        /// <summary>
        /// Release an item button back to the pool
        /// </summary>
        public void Release(ItemButton button) => pool.Release(button);

        /// <summary>
        /// Create a new item button within the pool
        /// </summary>
        private ItemButton CreateItem() => Object.Instantiate(prefab, parent);

        /// <summary>
        /// Process an item button when it is taken from the pool
        /// </summary>
        private void OnGetItem(ItemButton item) => item.gameObject.SetActive(true);

        /// <summary>
        /// Process an item button when it is released back to the pool
        /// </summary>
        private void OnReleaseItem(ItemButton item) => item.gameObject.SetActive(false);

        /// <summary>
        /// Process an item button when it is destroyed
        /// </summary>
        private void OnDestroyItem(ItemButton item) => Object.Destroy(item);
    }
}
