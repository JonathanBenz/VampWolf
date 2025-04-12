using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vampwolf.Pathfinding;
using Vampwolf.Input;
using Vampwolf.Interfaces;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf
{
    public enum PlayerState
    {
        MoveState, AttackState, WaitingForTurn
    }
    public class PlayerController : MonoBehaviour, IActor, ITrackable, ISelectable, ITargetable
    {
        [SerializeField] InputReader input;
        PlayerState currentState;

        public PlayerState CurrentState { get { return currentState; } set { currentState = value; } }

        public Tilemap groundMap;
        public float moveSpeed = 3f;

        //public TileHighlighter tileHighlighter;
        public int moveRange = 3;

        Pathfinder pathfinding;
        bool hasMoved;
        bool hasAttacked;
        //private bool isMoving = false;

        private void Awake()
        {
            pathfinding = GetComponent<Pathfinder>();
        }
        private void Start()
        {
            input.Select += isMousePressed => { if (isMousePressed) Select(); };

            input.EnablePlayerActions();

            //Vector3Int playerCell = tilemap.WorldToCell(transform.position);
            //tileHighlighter.HighlightMoveableTiles(playerCell, moveRange);
        }
        public void Select() 
        { 
            switch(currentState)
            {
                case PlayerState.WaitingForTurn:
                    return;
                case PlayerState.MoveState:
                    if (hasMoved) return;
                    Move(input.MousePos);
                    break;
                case PlayerState.AttackState:
                    if (hasAttacked) return;
                    Cast();
                    break;
            } 
        }
        public void Move(Vector2 targetPos) 
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(targetPos);
            Vector3Int targetCell = groundMap.WorldToCell(mouseWorld);
            Vector3Int startCell = groundMap.WorldToCell(transform.position);

            /*if (!tileHighlighter.HighlightedTiles.Contains(targetCell)) // Prevent selecting outside of the move range
            {
                Debug.Log("Tile not reachable!"); return;
            }*/

            List<Vector3Int> path = pathfinding.FindPath(startCell, targetCell);
            if (path != null && path.Count > 0)
                StartCoroutine(MoveAlongPath(path));

            hasMoved = true;
        }
        IEnumerator MoveAlongPath(List<Vector3Int> path)
        {
            foreach (Vector3Int cell in path)
            {
                Vector3 targetPos = groundMap.GetCellCenterWorld(cell);
                while (Vector3.Distance(transform.position, targetPos) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = targetPos;
                yield return new WaitForSeconds(0.05f);
            }

            currentState = PlayerState.WaitingForTurn;
            //EventBus<TurnEndedEvent>.Raise(new TurnEndedEvent());
            //tileHighlighter.ClearHighlights();
        }

        public void Cast() { }
        public void Die() { }

        public void Initiative() { }
        public void Target() { }

    }
}