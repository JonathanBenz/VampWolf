using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HellPortal : MonoBehaviour
    {
        private bool isTurnedOn; // In case we want this for data collection

        [SerializeField] int GoldReward = 300;
        [SerializeField] Vector3Int gridPosition;
        [SerializeField] Sprite[] sprites;

        SpriteRenderer spriteRenderer;
        private GameObject glowVFX;

        private EventBinding<PortalOpened> onOpenPortal;

        public Vector3Int GridPosition => gridPosition;

        private void OnEnable()
        {
            onOpenPortal = new EventBinding<PortalOpened>(TurnOn);
            EventBus<PortalOpened>.Register(onOpenPortal);
        }

        private void OnDisable()
        {
            EventBus<PortalOpened>.Deregister(onOpenPortal);
        }

        private void Awake()
        {
            glowVFX = transform.GetChild(0).gameObject;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            spriteRenderer.sprite = sprites[0]; // Portal is off
            glowVFX.SetActive(false);
        }

        void TurnOn()
        {
            isTurnedOn = true;
            spriteRenderer.sprite = sprites[1]; // Portal is on
            glowVFX.SetActive(true);
            Bank.Instance.AddGold(GoldReward);
        }
    }
}
