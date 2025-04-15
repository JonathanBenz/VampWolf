using UnityEngine;
using UnityEngine.Pool;

namespace Vampwolf.Grid
{
    public class GridHighlightPool
    {
        private readonly GameObject highlightPrefab;
        private readonly Transform parent;
        private readonly ObjectPool<GameObject> pool;

        public GridHighlightPool(GameObject highlightPrefab, Transform parent, int gridWidth, int gridHeight)
        {
            // Set variables
            this.highlightPrefab = highlightPrefab;
            this.parent = parent;

            // Calculate the max size of the pool
            int maxSize = gridWidth * gridHeight;

            // Create the object pool
            pool = new ObjectPool<GameObject>(
                CreateHighlight,
                OnGetHighlight,
                OnReleaseHighlight,
                OnDestroyHighlight,
                false,
                maxSize,
                maxSize
            );
        }

        /// <summary>
        /// Get a grid highlight from the pool
        /// </summary>
        public GameObject Get() => pool.Get();

        /// <summary>
        /// Release a grid highlight back to the pool
        /// </summary>
        public void Release(GameObject highlight) => pool.Release(highlight);

        /// <summary>
        /// Callback function for creating a grid highlight within the pool
        /// </summary>
        private GameObject CreateHighlight() => Object.Instantiate(highlightPrefab, parent);

        /// <summary>
        /// Callback function for when a grid highlight is received from the pool
        /// </summary>
        private void OnGetHighlight(GameObject highlight) => highlight.SetActive(true);

        /// <summary>
        /// Callback function for when a grid highlight is released back to the pool
        /// </summary>
        private void OnReleaseHighlight(GameObject highlight) => highlight.SetActive(false);

        /// <summary>
        /// Callback function for when a grid highlight is destroyed within the pool
        /// </summary>
        private void OnDestroyHighlight(GameObject highlight) => Object.Destroy(highlight);
    }
}
