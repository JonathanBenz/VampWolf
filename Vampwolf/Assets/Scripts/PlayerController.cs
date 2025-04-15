using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vampwolf.Pathfinding;
using Vampwolf.Input;
using Vampwolf.Interfaces;
using Vampwolf.Spells;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf
{
    public class PlayerController : Trackable, IActor, ISelectable, ITargetable
    {
        [SerializeField] InputReader input;
        [SerializeField] [Range(1, 5)] float moveSpeed = 3f;
        [SerializeField] int startingHealth;
        [SerializeField] int moveRange = 3;
        [SerializeField] int attackRange = 3; //TODO: Get the attack range dynamically from the currently selected spell instead of assigning it here
        [SerializeField] Sprite[] characterSprites;
        [SerializeField] CharacterType character;

        int currentHealth;
        int initiative;
        bool isMoving;
        bool isAttacking;
        bool hasMoved = true;
        bool hasAttacked = true;
        bool hasCurrentTurn;
        Vector3Int playerCell;
        Pathfinder pathfinding;
        Tilemap groundMap;
        TileHighlighter tileHighlighter;
        SpriteRenderer spriteRenderer;

        int currentSelectedAttack = -1;

        public override int Initiative { get { return initiative; } }
        public override bool IsEnemy => false;

        private void Awake()
        {
            pathfinding = GetComponent<Pathfinder>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            groundMap = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();
            tileHighlighter = FindObjectOfType<TileHighlighter>();
        }

        private void Start()
        {
            input.EnablePlayerActions();
            currentHealth = startingHealth;
        }

        private void Update()
        {
            if (!hasCurrentTurn) return; // Don't run the Update loop if its not the player's turn
            UpdateCharacterSprite();
            tileHighlighter.HandleHoverIndicator(input.MousePos);

            // --- DEBUG Purposes ---
            if (UnityEngine.Input.GetKeyDown(KeyCode.F6)) EndTurn(); 
            // ----------------------
        }

        /// <summary>
        /// On new turn, get a reference to the player's current cell and highlight move range.
        /// </summary>
        public override void StartTurn()
        {
            SubscribeToInputEvents();

            hasMoved = false;
            hasAttacked = false;
            hasCurrentTurn = true;
            Vector3Int playerCell = groundMap.WorldToCell(this.transform.position);
            HighlightTiles(playerCell);
            Debug.Log($"{this.gameObject.name} has started their turn!");
        }

        /// <summary>
        /// When Player ends their turn, update hasCurrentTurn flag and raise TurnEndedEvent
        /// </summary>
        private void EndTurn()
        {
            UnsubscribeFromInputEvents();
            input.ResetAbilityFlags();
            ResetVariables();
            HighlightTiles(playerCell);
            EventBus<TurnEndedEvent>.Raise(new TurnEndedEvent() { });
            Debug.Log($"{this.gameObject.name} has ended their turn!");
        }

        /// <summary>
        /// Reset the variables to their default values when it is not the player's turn anymore
        /// </summary>
        private void ResetVariables()
        {
            hasMoved = true;
            hasAttacked = true;
            isAttacking = false;
            isMoving = false;
            hasCurrentTurn = false;
            EventBus<PlayerStateChangedEvent>.Raise(new PlayerStateChangedEvent() { AttackState = false });
        }

        /// <summary>
        /// Set whether is attacking or not, update currentSelectedAttack. Update highlighted cells.
        /// </summary>
        /// <param name="bValue"></param>
        /// <param name="selectedAttack">-1 for null attack, else 1 for attack1, 2 for attack2, 3 for attack3.</param>
        private void SetIsAttacking(bool bValue, int selectedAttack)
        {
            isAttacking = bValue;
            currentSelectedAttack = selectedAttack;
            Debug.Log("Current Selected Attack: Attack " + currentSelectedAttack + " by " + this.gameObject.name);
            EventBus<PlayerStateChangedEvent>.Raise(new PlayerStateChangedEvent() { AttackState = isAttacking });
            HighlightTiles(playerCell);
        }

        /// <summary>
        /// Highlight movement/ attack range tiles. Clears highlights if the player has already moved and attacked. 
        /// </summary>
        /// <param name="playerCell"> the tilemap WorldToCell position of the player.</param>
        private void HighlightTiles(Vector3Int playerCell)
        {
            // Highlight movement tiles
            if (!isAttacking && !hasMoved) tileHighlighter.HighlightMoveableTiles(playerCell, moveRange); //print("Highlighting movement.");

            // Highlight attack tiles
            else if (isAttacking && !hasAttacked) tileHighlighter.HighlightMoveableTiles(playerCell, attackRange); //print("Highlighting attack range."); 

            else tileHighlighter.ClearHighlights(); //print("Nothing to highlight."); 
        }

        /// <summary>
        /// Depending on position of the mouse, have the character sprite change to 
        /// make it look like it's facing the correct direction of the mouse. 
        /// </summary>
        private void UpdateCharacterSprite()
        {
            if (isMoving) return; // Sprite stays still during movement

            Vector2 directionToMouse = (Camera.main.ScreenToWorldPoint(input.MousePos) - this.transform.position).normalized;

            if (directionToMouse.y >= 0) spriteRenderer.sprite = characterSprites[1];
            else if (directionToMouse.y < 0) spriteRenderer.sprite = characterSprites[0];

            if (directionToMouse.x >= 0) transform.localScale = new Vector3(-1, 1, 1);
            else if (directionToMouse.x < 0) transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Either Move to, or Cast an attack, when a Select Input happens.
        /// </summary>
        public void Select() 
        {
            if (!isAttacking && !hasMoved) Move(input.MousePos);
            else if (isAttacking && !hasAttacked) Cast();
        }

        /// <summary>
        /// Move toward a position after generating a valid path to it.
        /// </summary>
        /// <param name="targetPos">the destination target location.</param>
        public void Move(Vector2 targetPos) 
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(targetPos);
            Vector3Int targetCell = groundMap.WorldToCell(mouseWorld);
            Vector3Int startCell = groundMap.WorldToCell(transform.position);

            if (!tileHighlighter.HighlightedTiles.Contains(targetCell)) // Prevent selecting outside of the move range
            {
                Debug.Log("Tile not reachable!"); return;
            }

            List<Vector3Int> path = pathfinding.FindPath(startCell, targetCell);
            if (path != null && path.Count > 0)
                StartCoroutine(MoveAlongPath(path));
        }

        public void Cast() 
        {
            Debug.Log($"{this.gameObject.name} has attacked with attack {currentSelectedAttack}!");
            hasAttacked = true;
            HighlightTiles(playerCell);
        }

        public override void RollForInitiative() 
        {
            initiative = Random.Range(1, 21); //D20 dice roll
        }

        /// <summary>
        /// Method to reduce the actor's currentHealth. 
        /// </summary>
        /// <param name="dmg">Amount of damage taken.</param>
        public void TakeDamage(int dmg)
        {
            currentHealth -= dmg;
            if (currentHealth <= 0) 
            { 
                currentHealth = 0; 
                Die(); 
            }
        }

        /// <summary>
        /// Handle Death Logic for the current character
        /// </summary>
        public void Die() { Debug.Log($"{this.gameObject.name} has lost all HP and is now dead!"); }

        /// <summary>
        /// Move towards the center position of each tile, moving tile by tile until reaching the end of the path. 
        /// Diagonal movement is discouraged in the spirit of grid-like movement. 
        /// </summary>
        /// <param name="path">the path generated by pathfinding.FindPath().</param>
        /// <returns></returns>
        IEnumerator MoveAlongPath(List<Vector3Int> path)
        {
            isMoving = true;
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
            isMoving = false;
            hasMoved = true;
            HighlightTiles(playerCell);
        }

        private void SubscribeToInputEvents()
        {
            input.Select += OnSelect;
            input.UseAbility1 += OnUseAbility1;
            input.UseAbility2 += OnUseAbility2;
            input.UseAbility3 += OnUseAbility3;
        }

        private void UnsubscribeFromInputEvents()
        {
            input.Select -= OnSelect;
            input.UseAbility1 -= OnUseAbility1;
            input.UseAbility2 -= OnUseAbility2;
            input.UseAbility3 -= OnUseAbility3;
        }

        private void OnSelect(bool isPressed)
        {
            if (isPressed) Select();
        }
        private void OnUseAbility1(bool isAttacking)
        {
            if (isAttacking) SetIsAttacking(true, 1);
            else SetIsAttacking(false, -1);
        }
        private void OnUseAbility2(bool isAttacking)
        {
            if (isAttacking) SetIsAttacking(true, 2);
            else SetIsAttacking(false, -1);
        }
        private void OnUseAbility3(bool isAttacking)
        {
            if (isAttacking) SetIsAttacking(true, 3);
            else SetIsAttacking(false, -1);
        }
    }
}