using UnityEngine;
using UnityEngine.Pool;

namespace Vampwolf.Grid
{
    public class GridHighlightPool
    {
        private readonly HighlightTile highlightPrefab;
        private readonly Transform parent;
        private readonly ObjectPool<HighlightTile> pool;

        public GridHighlightPool(HighlightTile highlightPrefab, Transform parent, int gridWidth, int gridHeight)
        {
            // Set variables
            this.highlightPrefab = highlightPrefab;
            this.parent = parent;

            // Calculate the max size of the pool
            int maxSize = gridWidth * gridHeight;

            // Create the object pool
            pool = new ObjectPool<HighlightTile>(
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
        public HighlightTile Get() => pool.Get();

        /// <summary>
        /// Release a grid highlight back to the pool
        /// </summary>
        public void Release(HighlightTile highlight) => pool.Release(highlight);

        /// <summary>
        /// Callback function for creating a grid highlight within the pool
        /// </summary>
        private HighlightTile CreateHighlight() => Object.Instantiate(highlightPrefab, parent).Initialize();

        /// <summary>
        /// Callback function for when a grid highlight is received from the pool
        /// </summary>
        private void OnGetHighlight(HighlightTile highlight) => highlight.gameObject.SetActive(true);

        /// <summary>
        /// Callback function for when a grid highlight is released back to the pool
        /// </summary>
        private void OnReleaseHighlight(HighlightTile highlight) => highlight.gameObject.SetActive(false);

        /// <summary>
        /// Callback function for when a grid highlight is destroyed within the pool
        /// </summary>
        private void OnDestroyHighlight(HighlightTile highlight) => Object.Destroy(highlight);
    }
}
