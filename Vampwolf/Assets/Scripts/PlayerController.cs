using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vampwolf.Pathfinding;

namespace Vampwolf
{
    public class PlayerController : MonoBehaviour
    {

        public Tilemap tilemap;
        public float moveSpeed = 3f;

        public TileHighlighter tileHighlighter;
        public int moveRange = 5;

        Pathfinder pathfinding;
        private bool isMoving = false;

        private void Awake()
        {
            pathfinding = GetComponent<Pathfinder>();
        }
        private void Start()
        {
            Vector3Int playerCell = tilemap.WorldToCell(transform.position);
            tileHighlighter.HighlightMoveableTiles(playerCell, moveRange);
        }
        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0) && !isMoving)
            {
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                Vector3Int targetCell = tilemap.WorldToCell(mouseWorld);
                Vector3Int startCell = tilemap.WorldToCell(transform.position);

                if (!tileHighlighter.HighlightedTiles.Contains(targetCell))
                {
                    Debug.Log("Tile not reachable!");
                    return;
                }
                List<Vector3Int> path = pathfinding.FindPath(startCell, targetCell);
                if (path != null && path.Count > 0)
                    StartCoroutine(MoveAlongPath(path));
            }
        }

        IEnumerator MoveAlongPath(List<Vector3Int> path)
        {
            isMoving = true;

            foreach (Vector3Int cell in path)
            {
                Vector3 targetPos = tilemap.GetCellCenterWorld(cell);
                while (Vector3.Distance(transform.position, targetPos) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = targetPos;
                yield return new WaitForSeconds(0.05f); // Add delay to feel more turn-based
            }

            isMoving = false;
            tileHighlighter.ClearHighlights();
        }
    }
}
