using UnityEngine;
using UnityEngine.Pool;

namespace Vampwolf.Inventory
{
    public class InventorySlotPool
    {
        private readonly InventorySlot prefab;
        private readonly Transform parent;
        private readonly ObjectPool<InventorySlot> pool;

        public InventorySlotPool(InventorySlot prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;

            pool = new ObjectPool<InventorySlot>(
                CreateSlot,
                GetSlot,
                ReleaseSlot,
                DestroySlot,
                false,
                10,
                20
            );
        }

        /// <summary>
        /// Get a slot from the pool
        /// </summary>
        public InventorySlot Get() => pool.Get();

        /// <summary>
        /// Release a slot back to the pool
        /// </summary>
        public void Release(InventorySlot slot) => pool.Release(slot);

        /// <summary>
        /// Create a inventory slot within the pool
        /// </summary>
        private InventorySlot CreateSlot() => Object.Instantiate(prefab, parent);

        /// <summary>
        /// Process a slot retrieved from the pool
        /// </summary>
        private void GetSlot(InventorySlot slot) => slot.gameObject.SetActive(true);

        /// <summary>
        /// Process a slot being Released back to the pool
        /// </summary>
        private void ReleaseSlot(InventorySlot slot) => slot.gameObject.SetActive(false);

        /// <summary>
        /// Destroy a slot within the pool
        /// </summary>
        private void DestroySlot(InventorySlot slot) => Object.Destroy(slot);

    }
}
